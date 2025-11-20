using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class WireStart : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IDragHandler
{
    public int id;
    public Image img;

    // current pointer/screen position of this start (updated while dragging)
    [HideInInspector] public Vector2 screenPos;

    Canvas parentCanvas;

    void Awake()
    {
        img = GetComponent<Image>();
        parentCanvas = GetComponentInParent<Canvas>();
    }

    public void OnPointerEnter(PointerEventData eventData) { /* optional debug */ }
    public void OnPointerExit(PointerEventData eventData) { /* optional debug */ }

    // called during drag — update screenPos and move the UI element
    public void OnDrag(PointerEventData eventData)
    {
        // store screen position for manager checks
        screenPos = eventData.position;

        // move the visual.
        if (parentCanvas != null && parentCanvas.renderMode != RenderMode.WorldSpace)
        {
            transform.position = screenPos;
        }
        else
        {
            // convert for camera/world canvases
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


