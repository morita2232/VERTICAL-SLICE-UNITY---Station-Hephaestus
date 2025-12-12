using UnityEngine;
using UnityEngine.Rendering;

public class BallBalancingManager : MonoBehaviour
{
    private BallBalanceObject currentOwner;

    [Header("Cameras / Ball")]
    public GameObject mainCam;
    public GameObject mingameCam;
    public GameObject ballPrefab;
    private GameObject currentBall;
    public Transform ballSpawnPoint;
    public PlaneMovement planeMovementScript;
    public InteractLocator playerInteractLocator;

    private bool isMinigameOpen = false;
    public HorizontalMovement playerHorizontalMovement;
    public Movement playerMovement;
    public RotacionVertical playerVerticalMovement;
    public GameObject crosshair;

    [Header("Holes")]
    // Assign these in the inspector – one Renderer for each hole or ring mesh
    public Renderer[] holeRenderers;
    public Color normalHoleColor = Color.white;
    public Color correctHoleColor = Color.red;

    // Index of the currently correct hole
    private int correctHoleIndex = -1;

    public void OpenForObject(BallBalanceObject owner)
    {
        currentOwner = owner;

        Debug.Log("Ball minigame opened for " + owner.gameObject.name);

        mingameCam.SetActive(true);
        mainCam.SetActive(false);
        crosshair.SetActive(false);

        currentBall = Instantiate(ballPrefab, ballSpawnPoint.position, Quaternion.identity);

        planeMovementScript.movePlane = true;

        ChooseRandomCorrectHole();
        playerInteractLocator.isInminigame = true;
        isMinigameOpen = true;
        playerMovement.canMove = false;
        playerHorizontalMovement.canMove = false;
        playerVerticalMovement.canRotate = false;
    }

    void ChooseRandomCorrectHole()
    {
        if (holeRenderers == null || holeRenderers.Length == 0)
        {
            Debug.LogWarning("No hole renderers assigned on BallBalancingManager.");
            return;
        }

        // Pick random index
        correctHoleIndex = Random.Range(0, holeRenderers.Length);

        // Update the colors so only the chosen hole is highlighted
        for (int i = 0; i < holeRenderers.Length; i++)
        {
            if (holeRenderers[i] == null) continue;

            Color targetColor = (i == correctHoleIndex) ? correctHoleColor : normalHoleColor;
            holeRenderers[i].material.color = targetColor;

        }

        Debug.Log("Correct hole index: " + correctHoleIndex);
    }
    public void OnBallLeftPlayArea(GameObject ball)
    {

        if (currentBall == ball)
        {
            Destroy(currentBall);
            planeMovementScript.movePlane = false;

            currentBall = Instantiate(ballPrefab, ballSpawnPoint.position, Quaternion.identity);
            planeMovementScript.movePlane = true;
        }
    }

    public void OnBallEnteredHole(int holeIndex, GameObject ball)
    {
        // Ignore if it's not the active ball
        if (ball != currentBall) return;

        Debug.Log("Ball entered hole index: " + holeIndex);

        if (holeIndex == correctHoleIndex)
        {
            Debug.Log("Correct hole! Minigame success.");
            currentOwner.MarkCompleted();
            // reward the player here if you want
            if (!isMinigameOpen) return;
            EndMinigame();
        }
        else
        {
            Debug.Log("Wrong hole, respawning ball.");

            // WRONG HOLE  respawn ball
            Destroy(currentBall);
            planeMovementScript.movePlane = false;

            currentBall = Instantiate(ballPrefab, ballSpawnPoint.position, Quaternion.identity);
            planeMovementScript.movePlane = true;

            // Optionally choose a new correct hole:
            // ChooseRandomCorrectHole();
        }
    }


    private void EndMinigame()
    {
        planeMovementScript.movePlane = false;
        Destroy(currentBall);
        crosshair.SetActive(true);
        playerInteractLocator.isInminigame = false;
        isMinigameOpen = false;
        playerMovement.canMove = true;
        playerHorizontalMovement.canMove = true;
        playerVerticalMovement.canRotate = true;
        Cursor.lockState = CursorLockMode.Locked;

        mingameCam.SetActive(false);
        mainCam.SetActive(true);

        Debug.Log("Minigame closed.");
    }
    void Start()
    {
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (!isMinigameOpen) return;
            EndMinigame();
            return;
        }

    }
}

