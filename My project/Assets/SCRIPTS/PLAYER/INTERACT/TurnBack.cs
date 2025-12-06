using UnityEngine;

public class TurnBack : MonoBehaviour
{
    [Header("References")]
    public InteractLocator interactLocator;
    public Transform returnPosition;

    // Update is called once per frame
    void Update()
    {

        if(interactLocator.isInSpaceShip && Input.GetKeyDown(KeyCode.R)) {

            Debug.Log("Exiting spaceship...");
            interactLocator.player.transform.position = returnPosition.position;
            interactLocator.sSammy.transform.position = new Vector3(
                returnPosition.position.x + 1f,
                0.3000002f,
                returnPosition.position.z + 1f
            );
            interactLocator.isInSpaceShip = false;
        }


    }
}
