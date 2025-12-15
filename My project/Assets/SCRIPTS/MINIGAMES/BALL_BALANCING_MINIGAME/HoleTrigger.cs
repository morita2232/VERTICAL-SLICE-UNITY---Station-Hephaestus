using UnityEngine;

/// <summary>
/// Detects when the ball enters a hole
/// and informs the BallBalancingManager which hole was hit.
/// </summary>
public class HoleTrigger : MonoBehaviour
{
    // ================================
    // Hole Configuration
    // ================================

    [Header("Hole Settings")]

    public int holeIndex;                // Unique index for this hole (0, 1, 2, ...)
    public BallBalancingManager manager; // Reference to the minigame manager


    /// <summary>
    /// Triggered when an object enters the hole trigger
    /// </summary>
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ball")) // Ball prefab must be tagged "Ball"
        {
            manager.OnBallEnteredHole(holeIndex, other.gameObject);
        }
    }
}

