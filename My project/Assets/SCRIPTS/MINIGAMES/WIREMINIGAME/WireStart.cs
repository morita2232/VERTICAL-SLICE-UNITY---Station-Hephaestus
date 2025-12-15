using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// Represents a draggable wire start point in the wire puzzle.
/// Handles drag input and resets itself when the puzzle starts.
/// </summary>
public class WireStart : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IDragHandler
{
    // ================================
    // Wire Attributes
    // ================================

    [Header("Attributes")]

    public int id;             // Unique identifier for this wire
    public Image img;          // Image component for UI visuals


    // ================================
    // Canvas References
    // ================================

    Canvas parentCanvas;       // Parent canvas used for coordinate conversion


    // ================================
    // Runtime State (Hidden)
    // ================================

    [HideInInspector] public Vector2 screenPos;          // Current screen position
    [HideInInspector] public Vector3 initialPosition;    // Starting position
    [HideInInspector] public bool hasInitialPosition = false; // Cached initial state


    void Awake()
    {
        // Cache image and parent canvas
        img = GetComponent<Image>();
        parentCanvas = GetComponentInParent<Canvas>();
    }

    // ================================
    // Puzzle Lifecycle
    // ================================

    /// <summary>
    /// Called by the manager when the puzzle starts.
    /// Resets the wire back to its original position.
    /// </summary>
    public void ResetState()
    {
        if (!hasInitialPosition)
        {
            initialPosition = transform.position;
            hasInitialPosition = true;
        }

        screenPos = Vector2.zero;
        transform.position = initialPosition;
    }

    // ================================
    // Pointer Events (Unused Hooks)
    // ================================

    public void OnPointerEnter(PointerEventData eventData) { }
    public void OnPointerExit(PointerEventData eventData) { }


    // ================================
    // Drag Handling
    // ================================

    public void OnDrag(PointerEventData eventData)
    {
        screenPos = eventData.position;

        // Screen Space (Overlay / Camera)
        if (parentCanvas != null && parentCanvas.renderMode != RenderMode.WorldSpace)
        {
            transform.position = screenPos;
        }
        // World Space Canvas
        else
        {
            RectTransformUtility.ScreenPointToWorldPointInRectangle(
                parentCanvas != null ? (parentCanvas.transform as RectTransform) : null,
                screenPos,
                parentCanvas != null ? parentCanvas.worldCamera : null,
                out Vector3 worldPos
            );

            transform.position = worldPos;
        }
    }
}




