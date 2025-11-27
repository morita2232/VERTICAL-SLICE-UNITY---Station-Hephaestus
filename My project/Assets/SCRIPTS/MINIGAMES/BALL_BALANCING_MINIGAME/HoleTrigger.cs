using UnityEngine;

public class HoleTrigger : MonoBehaviour
{
    public int holeIndex;                          // set this in the Inspector (0,1,2,...)
    public BallBalancingManager manager;           // drag the manager here

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ball"))             // tag your ball prefab as "Ball"
        {
            manager.OnBallEnteredHole(holeIndex, other.gameObject);
        }
    }
}
