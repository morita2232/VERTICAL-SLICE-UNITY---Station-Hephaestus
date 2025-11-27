using UnityEngine;

public class BallOutOfBounds : MonoBehaviour
{
    public BallBalancingManager manager;   // drag your manager here in Inspector

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Ball"))
        {
            // Tell the manager the ball left the area
            manager.OnBallLeftPlayArea(other.gameObject);
        }
    }
}

