using UnityEngine;

public class Interactable : MonoBehaviour
{
    [Header("Pop up attributes")]
    public GameObject interactPopUp;
    public Vector3 offsetFromCamera = new Vector3(0, 1f, 2.5f); // adjustable offset
    private Transform cam;


    [Header("Script references")]
    public InteractLocator interactLocator;

    private void Start()
    {
        cam = Camera.main != null ? Camera.main.transform : null;

        if (cam == null)
        {
            Debug.LogError("No Camera with tag 'MainCamera' found in the scene.");
        }

        if (interactPopUp != null)
        {
            interactPopUp.SetActive(false); // start hidden
        }
    }

    private void Update()
    {
        if (interactPopUp == null || cam == null || interactLocator == null)
            return;

        // Show / hide based on canInteract
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



