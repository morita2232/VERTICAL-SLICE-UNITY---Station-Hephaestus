using UnityEngine;

/// <summary>
/// Allows the player to exit the spaceship and return
/// to a predefined position when pressing the return key.
/// </summary>
public class TurnBack : MonoBehaviour
{
    // ================================
    // References
    // ================================

    [Header("References")]

    public InteractLocator interactLocator; // Tracks spaceship state and player refs
    public Transform returnPosition;        // Where the player is returned to


    void Update()
    {
        // Check if player is inside the spaceship and presses return key
        if (interactLocator.isInSpaceShip && Input.GetKeyDown(KeyCode.R))
        {
            Debug.Log("Exiting spaceship...");

            // Move player back to return position
            interactLocator.player.transform.position = returnPosition.position;

            // Reposition Sammy slightly beside the player
            interactLocator.sSammy.transform.position = new Vector3(
                returnPosition.position.x + 1f,
                0.3000002f,
                returnPosition.position.z + 1f
            );

            // Exit spaceship state
            interactLocator.isInSpaceShip = false;
        }
    }
}

