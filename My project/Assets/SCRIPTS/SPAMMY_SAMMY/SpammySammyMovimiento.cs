using UnityEngine;

public class SpammySammyMovimiento : MonoBehaviour
{

    public Transform playerPos;
    public Animator animator;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if (playerPos)
        {
            float dist = Vector3.Distance(playerPos.position, transform.position);
            print("Distance to player: " + dist);
        }

    }
}
