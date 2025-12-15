using UnityEngine;
using UnityEngine.Rendering;

/// <summary>
/// Manages the ball balancing minigame:
/// - Switches cameras
/// - Spawns and resets the ball
/// - Chooses a correct hole
/// - Locks and restores player controls
/// </summary>
public class BallBalancingManager : MonoBehaviour
{
    // ================================
    // Minigame State
    // ================================

    private BallBalanceObject currentOwner;   // Object that triggered the minigame
    private bool isMinigameOpen = false;       // Tracks if the minigame is active


    // ================================
    // Cameras & Ball Setup
    // ================================

    [Header("Cameras & Ball")]

    public GameObject mainCam;                // Main gameplay camera
    public GameObject mingameCam;             // Minigame camera
    public GameObject ballPrefab;             // Ball prefab to spawn
    public Transform ballSpawnPoint;          // Where the ball spawns

    private GameObject currentBall;            // Active ball instance

    public PlaneMovement planeMovementScript; // Controls tilting plane movement
    public InteractLocator playerInteractLocator; // Tracks minigame interaction state


    // ================================
    // Player Control References
    // ================================

    [Header("Player Controls")]

    public Movement playerMovement;            // Player movement controller
    public HorizontalMovement playerHorizontalMovement; // Horizontal look control
    public RotacionVertical playerVerticalMovement;     // Vertical look control
    public GameObject crosshair;               // Player crosshair UI


    // ================================
    // Hole Configuration
    // ================================

    [Header("Holes")]

    // Assign one Renderer per hole or ring mesh
    public Renderer[] holeRenderers;

    public Color normalHoleColor = Color.white;  // Default hole color
    public Color correctHoleColor = Color.red;   // Highlighted correct hole color

    private int correctHoleIndex = -1;           // Index of the correct hole


    /// <summary>
    /// Opens the minigame for a specific object
    /// </summary>
    public void OpenForObject(BallBalanceObject owner)
    {
        currentOwner = owner;

        Debug.Log("Ball minigame opened for " + owner.gameObject.name);

        // Switch cameras and UI
        mingameCam.SetActive(true);
        mainCam.SetActive(false);
        crosshair.SetActive(false);

        // Spawn ball
        currentBall = Instantiate(ballPrefab, ballSpawnPoint.position, Quaternion.identity);

        // Enable plane movement
        planeMovementScript.movePlane = true;

        // Setup gameplay state
        ChooseRandomCorrectHole();
        playerInteractLocator.isInminigame = true;
        isMinigameOpen = true;

        // Lock player controls
        playerMovement.canMove = false;
        playerHorizontalMovement.canMove = false;
        playerVerticalMovement.canRotate = false;
    }

    /// <summary>
    /// Randomly selects which hole is correct and updates visuals
    /// </summary>
    void ChooseRandomCorrectHole()
    {
        if (holeRenderers == null || holeRenderers.Length == 0)
        {
            Debug.LogWarning("No hole renderers assigned on BallBalancingManager.");
            return;
        }

        // Pick a random hole index
        correctHoleIndex = Random.Range(0, holeRenderers.Length);

        // Update hole colors
        for (int i = 0; i < holeRenderers.Length; i++)
        {
            if (holeRenderers[i] == null) continue;

            Color targetColor = (i == correctHoleIndex)
                ? correctHoleColor
                : normalHoleColor;

            holeRenderers[i].material.color = targetColor;
        }

        Debug.Log("Correct hole index: " + correctHoleIndex);
    }

    /// <summary>
    /// Called when the ball leaves the play area
    /// </summary>
    public void OnBallLeftPlayArea(GameObject ball)
    {
        if (currentBall == ball)
        {
            Destroy(currentBall);
            planeMovementScript.movePlane = false;

            // Respawn ball
            currentBall = Instantiate(ballPrefab, ballSpawnPoint.position, Quaternion.identity);
            planeMovementScript.movePlane = true;
        }
    }

    /// <summary>
    /// Called when the ball enters a hole
    /// </summary>
    public void OnBallEnteredHole(int holeIndex, GameObject ball)
    {
        // Ignore if this isn't the active ball
        if (ball != currentBall) return;

        Debug.Log("Ball entered hole index: " + holeIndex);

        if (holeIndex == correctHoleIndex)
        {
            Debug.Log("Correct hole! Minigame success.");

            // Mark puzzle as completed
            currentOwner.MarkCompleted();

            if (!isMinigameOpen) return;
            EndMinigame();
        }
        else
        {
            Debug.Log("Wrong hole, respawning ball.");

            // Wrong hole  respawn ball
            Destroy(currentBall);
            planeMovementScript.movePlane = false;

            currentBall = Instantiate(ballPrefab, ballSpawnPoint.position, Quaternion.identity);
            planeMovementScript.movePlane = true;

        }
    }

    /// <summary>
    /// Ends the minigame and restores gameplay state
    /// </summary>
    private void EndMinigame()
    {
        planeMovementScript.movePlane = false;

        Destroy(currentBall);

        // Restore UI and player control
        crosshair.SetActive(true);
        playerInteractLocator.isInminigame = false;
        isMinigameOpen = false;

        playerMovement.canMove = true;
        playerHorizontalMovement.canMove = true;
        playerVerticalMovement.canRotate = true;

        Cursor.lockState = CursorLockMode.Locked;

        // Switch cameras back
        mingameCam.SetActive(false);
        mainCam.SetActive(true);

        Debug.Log("Minigame closed.");
    }

    void Start()
    {
    }

    void Update()
    {
        // Emergency exit from minigame
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (!isMinigameOpen) return;
            EndMinigame();
        }
    }
}


