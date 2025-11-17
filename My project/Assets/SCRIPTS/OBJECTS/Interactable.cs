using UnityEngine;

public class Interactable : MonoBehaviour
{

    public GameObject interactPopUp;

    public GameObject location;

    private void OnTriggerEnter(Collider other)
    {
           
        if(other.gameObject.CompareTag("Player"))
        {
            interactPopUp.SetActive(true);
            
            interactPopUp.transform.position = location.transform.position;

        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            interactPopUp.SetActive(false);
        }
    }

}

