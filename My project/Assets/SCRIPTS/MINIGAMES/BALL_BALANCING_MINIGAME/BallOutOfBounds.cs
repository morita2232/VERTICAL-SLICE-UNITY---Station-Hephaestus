using UnityEngine;

/// <summary>
/// Detects when the ball leaves the playable area
/// and notifies the BallBalancingManager to respawn it.
/// </summary>
public class BallOutOfBounds : MonoBehaviour
{
    // ================================
    // Minigame Manager Reference
    // ================================

    [Header("Ball Minigame Manager")]

    public BallBalancingManager manager;   // Reference to the active minigame manager


    /// <summary>
    /// Triggered when an object exits the out-of-bounds trigger
    /// </summary>
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Ball"))
        {
            // Notify the manager that the ball left the play area
            manager.OnBallLeftPlayArea(other.gameObject);
        }
    }
}


