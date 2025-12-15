using UnityEngine;

/// <summary>
/// Displays an interaction popup when the player
/// is able to interact with this object.
/// </summary>
public class Interactable : MonoBehaviour
{
    // ================================
    // Popup Display Settings
    // ================================

    [Header("Pop up attributes")]

    public GameObject interactPopUp;          // UI element shown when interaction is possible
    public Vector3 offsetFromCamera = new Vector3(0, 1f, 2.5f); // Adjustable popup offset
    private Transform cam;                    // Cached main camera transform


    // ================================
    // Interaction Reference
    // ================================

    [Header("Script references")]

    public InteractLocator interactLocator;   // Tracks whether interaction is allowed


    private void Start()
    {
        // Cache main camera
        cam = Camera.main != null ? Camera.main.transform : null;

        if (cam == null)
        {
            Debug.LogError("No Camera with tag 'MainCamera' found in the scene.");
        }

        // Start with popup hidden
        if (interactPopUp != null)
        {
            interactPopUp.SetActive(false);
        }
    }

    private void Update()
    {
        if (interactPopUp == null || cam == null || interactLocator == null)
            return;

        // Show or hide popup based on interaction state
        if (interactLocator.canInteract)
        {
            if (!interactPopUp.activeSelf)
                interactPopUp.SetActive(true);
        }
        else
        {
            if (interactPopUp.activeSelf)
                interactPopUp.SetActive(false);
        }
    }
}




