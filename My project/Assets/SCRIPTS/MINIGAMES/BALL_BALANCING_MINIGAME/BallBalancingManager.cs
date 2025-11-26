using UnityEngine;
using UnityEngine.Rendering;

public class BallBalancingManager : MonoBehaviour
{
    private BallBalanceObject currentOwner;
    public GameObject mainCam;
    public GameObject mingameCam;
    public GameObject ballPrefab;
    public Transform ballSpawnPoint;
    public PlaneMovement planeMovementScript;

    public void OpenForObject(BallBalanceObject owner)
    {

        currentOwner = owner;

        Debug.Log("Ball minigame opened for " + owner.gameObject.name);

        mingameCam.SetActive(true);
        mainCam.SetActive(false);

        Instantiate(ballPrefab, ballSpawnPoint.position, Quaternion.identity);

        planeMovementScript.movePlane = true;

    }



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
  
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
