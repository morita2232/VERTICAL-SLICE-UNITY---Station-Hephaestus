using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class WireStart : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IDragHandler
{
    public int id;
    public Image img;

    [HideInInspector] public Vector2 screenPos;
    [HideInInspector] public Vector3 initialPosition;
    [HideInInspector] public bool hasInitialPosition = false;

    Canvas parentCanvas;

    void Awake()
    {
        img = GetComponent<Image>();
        parentCanvas = GetComponentInParent<Canvas>();

    }

    // --- NEW: called by manager when starting a puzzle ---
    public void ResetState()
    {
        // first time we reset, remember where the layout put us
        if (!hasInitialPosition)
        {
            initialPosition = transform.position;
            hasInitialPosition = true;
        }

        screenPos = Vector2.zero;
        transform.position = initialPosition;
    }
    // -----------------------------------------------------

    public void OnPointerEnter(PointerEventData eventData) { }
    public void OnPointerExit(PointerEventData eventData) { }

    public void OnDrag(PointerEventData eventData)
    {
        screenPos = eventData.position;

        if (parentCanvas != null && parentCanvas.renderMode != RenderMode.WorldSpace)
        {
            transform.position = screenPos;
        }
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




