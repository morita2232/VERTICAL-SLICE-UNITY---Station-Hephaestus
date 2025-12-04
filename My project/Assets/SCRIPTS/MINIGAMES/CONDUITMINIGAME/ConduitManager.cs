using UnityEngine;

public class ConduitManager : MonoBehaviour
{
    private ConduitObject currentOwner;

    [Header("Cameras / Player")]
    public InteractLocator playerInteractLocator;

    public HorizontalMovement playerHorizontalMovement;
    public Movement playerMovement;
    public RotacionVertical playerVerticalMovement;

    [Header("Conduit Puzzle")]
    public ConduitTube[] tubes;

    [Header("Input")]
    public KeyCode startKey = KeyCode.E;
    public KeyCode prevTubeKey = KeyCode.A; 
    public KeyCode nextTubeKey = KeyCode.D;
    public KeyCode exitKey = KeyCode.Q; 

    public bool solved { get; private set; }

    int activeIndex = 0;
    bool puzzleStarted = false;
    bool isMinigameOpen = false;

    public void OpenForObject(ConduitObject owner)
    {
        currentOwner = owner;

        Debug.Log("Conduit minigame opened for " + owner.gameObject.name);

        solved = false;
        puzzleStarted = false;
        activeIndex = 0;

        if (tubes != null)
        {
            for (int i = 0; i < tubes.Length; i++)
            {
                if (tubes[i] == null) continue;
                tubes[i].ResetTube();
            }
        }
        Debug.Log("Conduit tubes reset.");

        SetActiveTube(activeIndex, false);

        Debug.Log("Press " + startKey + " to start the conduit puzzle.");

        playerInteractLocator.isInminigame = true;

        Debug.Log("Player controls disabled.");

        playerMovement.canMove = false;

        Debug.Log("Player movement disabled.");

        playerHorizontalMovement.canMove = false;

        Debug.Log("Player horizontal movement disabled.");

        playerVerticalMovement.canRotate = false;

        Debug.Log("Player vertical movement disabled.");

        isMinigameOpen = true;

        Debug.Log("Conduit minigame is now active.");
    }

    void Update()
    {
        if (!isMinigameOpen) return;

        if (Input.GetKeyDown(exitKey))
        {
            EndMinigame();
            return;
        }

        if (solved) return;

        HandlePuzzleInput();
        CheckSolved();
    }

    void HandlePuzzleInput()
    {
        if (!puzzleStarted)
        {
            if (Input.GetKeyDown(startKey))
            {
                puzzleStarted = true;
                SetActiveTube(activeIndex, true);
            }
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

    void SwitchActiveTube(int newIndex)
    {
        if (tubes == null || tubes.Length == 0) return;

        if (tubes[activeIndex] != null)
            tubes[activeIndex].SetActive(false);

        activeIndex = newIndex;

        if (tubes[activeIndex] != null)
            tubes[activeIndex].SetActive(true);
    }

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

    void OnSolved()
    {
        Debug.Log("Conduit puzzle solved!");

        if (currentOwner != null)
            currentOwner.MarkCompleted();

        EndMinigame();
    }

    void EndMinigame()
    {
        isMinigameOpen = false;

      
        if (tubes != null)
        {
            foreach (var t in tubes)
            {
                if (t == null) continue;
                t.SetActive(false);
            }
        }

        if (playerInteractLocator != null)
            playerInteractLocator.isInminigame = false;

        if (playerMovement != null) playerMovement.canMove = true;
        if (playerHorizontalMovement != null) playerHorizontalMovement.canMove = true;
        if (playerVerticalMovement != null) playerVerticalMovement.canRotate = true;

        Cursor.lockState = CursorLockMode.Locked;

        Debug.Log("Conduit minigame closed.");
    }
}


