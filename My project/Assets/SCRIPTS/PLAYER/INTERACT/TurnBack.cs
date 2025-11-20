using UnityEngine;

public class TurnBack : MonoBehaviour
{
    public InteractLocator interactLocator;
    public Transform returnPosition;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if(interactLocator.isInSpaceShip && Input.GetKeyDown(KeyCode.E)) {

            Debug.Log("Exiting spaceship...");
            interactLocator.player.transform.position = returnPosition.position;
            interactLocator.isInSpaceShip = false;
        }


    }
}
