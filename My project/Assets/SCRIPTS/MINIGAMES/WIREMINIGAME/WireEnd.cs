using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// Represents a wire endpoint in the wire-connection minigame.
/// Tracks whether it has been connected to its matching wire start.
/// </summary>
public class WireEnd : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    // ================================
    // Wire Attributes
    // ================================

    [Header("Attributes")]

    public int id;               // Unique identifier for this wire end
    public Image img;            // Image component for visual representation


    // ================================
    // Connection State
    // ================================

    [Header("Connection State")]

    public bool connected = false; // True once a wire start snaps to this end


    void Awake()
    {
        // Cache Image component
        img = GetComponent<Image>();
    }

    /// <summary>
    /// Debug: called when pointer enters this wire end
    /// </summary>
    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("Mouse entered " + gameObject.name);
    }

    /// <summary>
    /// Debug: called when pointer exits this wire end
    /// </summary>
    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("Mouse exited " + gameObject.name);
    }
}

