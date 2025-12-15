using UnityEngine;

/// <summary>
/// Manages the conduit charging puzzle:
/// - Disables player controls
/// - Handles tube selection and activation
/// - Checks for puzzle completion
/// - Restores gameplay when finished
/// </summary>
public class ConduitManager : MonoBehaviour
{
    // ================================
    // Puzzle Owner
    // ================================

    private ConduitObject currentOwner;   // Object that triggered the puzzle


    // ================================
    // Player & Interaction References
    // ================================

    [Header("Cameras / Player")]

    public InteractLocator playerInteractLocator; // Tracks minigame state

    public HorizontalMovement playerHorizontalMovement; // Horizontal look control
    public Movement playerMovement;                     // Player movement
    public RotacionVertical playerVerticalMovement;     // Vertical look control


    // ================================
    // Conduit Puzzle Setup
    // ================================

    [Header("Conduit Puzzle")]

    public ConduitTube[] tubes;           // All conduit tubes in the puzzle


    // ================================
    // Input Keys
    // ================================

    [Header("Input")]

    public KeyCode startKey = KeyCode.E;   // Start puzzle / activate first tube
    public KeyCode prevTubeKey = KeyCode.S; // Select previous tube
    public KeyCode nextTubeKey = KeyCode.W; // Select next tube
    public KeyCode exitKey = KeyCode.Q;    // Exit puzzle


    // ================================
    // Puzzle State
    // ================================

    public bool solved { get; private set; } // True once puzzle is completed

    int activeIndex = 0;                     // Currently active tube index
    bool puzzleStarted = false;              // Whether charging has begun
    bool isMinigameOpen = false;              // Whether the puzzle is active


    /// <summary>
    /// Opens the conduit puzzle for a specific object
    /// </summary>
    public void OpenForObject(ConduitObject owner)
    {
        currentOwner = owner;

        Debug.Log("Conduit minigame opened for " + owner.gameObject.name);

        solved = false;
        puzzleStarted = false;
        activeIndex = 0;

        // Reset all tubes
        if (tubes != null)
        {
            for (int i = 0; i < tubes.Length; i++)
            {
                if (tubes[i] == null) continue;
                tubes[i].ResetTube();
            }
        }

        Debug.Log("Conduit tubes reset.");

        // Ensure no tube is charging yet
        SetActiveTube(activeIndex, false);

        Debug.Log("Press " + startKey + " to start the conduit puzzle.");

        // Lock player controls
        playerInteractLocator.isInminigame = true;

        playerMovement.canMove = false;
        playerHorizontalMovement.canMove = false;
        playerVerticalMovement.canRotate = false;

        Debug.Log("Player controls disabled.");

        isMinigameOpen = true;

        Debug.Log("Conduit minigame is now active.");
    }

    void Update()
    {
        if (!isMinigameOpen) return;

        // Exit puzzle
        if (Input.GetKeyDown(exitKey))
        {
            EndMinigame();
            return;
        }

        if (solved) return;

        HandlePuzzleInput();
        CheckSolved();
    }

    /// <summary>
    /// Handles player input for starting and switching tubes
    /// </summary>
    void HandlePuzzleInput()
    {
        if (!puzzleStarted)
        {
            puzzleStarted = true;
            SetActiveTube(activeIndex, true);
            return;
        }

        int newIndex = activeIndex;

        if (Input.GetKeyDown(nextTubeKey))
        {
            newIndex = (activeIndex + 1) % tubes.Length;
        }
        else if (Input.GetKeyDown(prevTubeKey))
        {
            newIndex = (activeIndex - 1 + tubes.Length) % tubes.Length;
        }

        if (newIndex != activeIndex)
        {
            SwitchActiveTube(newIndex);
        }
    }

    /// <summary>
    /// Switches which tube is currently charging
    /// </summary>
    void SwitchActiveTube(int newIndex)
    {
        if (tubes == null || tubes.Length == 0) return;

        if (tubes[activeIndex] != null)
            tubes[activeIndex].SetActive(false);

        activeIndex = newIndex;

        if (tubes[activeIndex] != null)
            tubes[activeIndex].SetActive(true);
    }

    /// <summary>
    /// Sets the active tube and optionally starts charging it
    /// </summary>
    void SetActiveTube(int index, bool shouldCharge)
    {
        if (tubes == null || tubes.Length == 0) return;

        activeIndex = Mathf.Clamp(index, 0, tubes.Length - 1);

        for (int i = 0; i < tubes.Length; i++)
        {
            if (tubes[i] == null) continue;

            bool isThisActive = (i == activeIndex) && shouldCharge;
            tubes[i].SetActive(isThisActive);
        }
    }

    /// <summary>
    /// Checks whether all tubes are within their target ranges
    /// </summary>
    void CheckSolved()
    {
        if (tubes == null || tubes.Length == 0) return;

        foreach (var t in tubes)
        {
            if (t == null) return;
            if (!t.InTarget) return;
        }

        solved = true;
        OnSolved();
    }

    /// <summary>
    /// Called when the puzzle is successfully solved
    /// </summary>
    void OnSolved()
    {
        Debug.Log("Conduit puzzle solved!");

        if (currentOwner != null)
            currentOwner.MarkCompleted();

        EndMinigame();
    }

    /// <summary>
    /// Ends the minigame and restores player control
    /// </summary>
    void EndMinigame()
    {
        isMinigameOpen = false;

        // Stop all tubes from charging
        if (tubes != null)
        {
            foreach (var t in tubes)
            {
                if (t == null) continue;
                t.SetActive(false);
            }
        }

        // Restore interaction state
        if (playerInteractLocator != null)
            playerInteractLocator.isInminigame = false;

        // Restore player controls
        if (playerMovement != null) playerMovement.canMove = true;
        if (playerHorizontalMovement != null) playerHorizontalMovement.canMove = true;
        if (playerVerticalMovement != null) playerVerticalMovement.canRotate = true;

        Cursor.lockState = CursorLockMode.Locked;

        Debug.Log("Conduit minigame closed.");
    }
}



