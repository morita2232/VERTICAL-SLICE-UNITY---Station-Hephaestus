using UnityEngine;
using TMPro;
using System.Collections;

/// <summary>
/// Controls the alien interaction sequence:
/// - Locks player movement
/// - Plays alien + translated dialogue
/// - Shows final choice UI
/// - Triggers an ending
/// </summary>
public class AlienInteraction : MonoBehaviour
{
    // ================================
    // Alien Dialogue Setup (Inspector)
    // ================================

    [Header("Alien Dialogue")]

    public TMP_FontAsset alienFont;          // Font for alien language (unreadable)
    public TMP_FontAsset translationFont;    // Font for translated dialogue (readable)
    public Sprite alienPortrait;             // Portrait used during dialogue

    [TextArea]
    public string[] alienLines;              // Alien language lines (must match translations)

    [TextArea]
    public string[] translatedLines;         // Human translations (same length as alienLines)


    // ================================
    // Choice UI References
    // ================================

    [Header("Choice UI")]

    public GameObject choicePanel;            // Panel containing choice buttons
    public UnityEngine.UI.Button letGoButton; // Button for letting the alien go
    public UnityEngine.UI.Button disposeButton; // Button for disposing of the alien
    public GameObject crossHair;              // Player crosshair UI


    // ================================
    // Player Control Scripts
    // ================================

    public Movement playerMovement;           // Player movement controller
    public HorizontalMovement horizontalLook; // Horizontal camera control
    public RotacionVertical verticalLook;     // Vertical camera control


    // ================================
    // State & Ending Logic
    // ================================

    private bool hasInteracted = false;        // Prevents repeat interaction
    public endings endingScript;               // Reference to ending handler


    private void Start()
    {
        // Hide choice UI at start
        choicePanel.SetActive(false);

        // Assign button callbacks
        letGoButton.onClick.AddListener(() => Choose("letgo"));
        disposeButton.onClick.AddListener(() => Choose("dispose"));
    }

    /// <summary>
    /// Called when the player interacts with the alien
    /// </summary>
    public void Interact()
    {
        if (hasInteracted)
            return;

        hasInteracted = true;

        // Disable player movement and camera
        if (playerMovement != null)
            playerMovement.canMove = false;

        if (horizontalLook != null)
            horizontalLook.canMove = false;

        if (verticalLook != null)
            verticalLook.canRotate = false;

        // Start dialogue sequence
        StartCoroutine(PlayAlienDialogue());
    }

    /// <summary>
    /// Plays each alien line followed by its translation
    /// </summary>
    private IEnumerator PlayAlienDialogue()
    {
        // Ensure dialogue arrays match
        if (alienLines.Length != translatedLines.Length)
        {
            Debug.LogError("AlienInteraction: alienLines and translatedLines must be the same length.");
            yield break;
        }

        for (int i = 0; i < alienLines.Length; i++)
        {
            // Alien language line
            bool finished = false;
            void Done1() => finished = true;
            DialogueManager.OnDialogueSequenceFinished += Done1;

            DialogueManager.Instance.Say("Alien", alienLines[i], alienFont, alienPortrait);
            yield return new WaitUntil(() => finished);

            DialogueManager.OnDialogueSequenceFinished -= Done1;

            // Translated line
            finished = false;
            void Done2() => finished = true;
            DialogueManager.OnDialogueSequenceFinished += Done2;

            DialogueManager.Instance.Say("Alien", translatedLines[i], translationFont, alienPortrait);
            yield return new WaitUntil(() => finished);

            DialogueManager.OnDialogueSequenceFinished -= Done2;
        }

        // Show final choice
        crossHair.SetActive(false);
        Cursor.lockState = CursorLockMode.None;
        choicePanel.SetActive(true);
    }

    /// <summary>
    /// Handles the player's final decision
    /// </summary>
    private void Choose(string option)
    {
        choicePanel.SetActive(false);
        crossHair.SetActive(true);
        Cursor.lockState = CursorLockMode.Locked;

        if (option == "letgo")
            endingScript.EndingOne();
        else
            endingScript.EndingTwo();

        // Restore player controls
        if (playerMovement != null)
            playerMovement.canMove = true;

        if (horizontalLook != null)
            horizontalLook.canMove = true;

        if (verticalLook != null)
            verticalLook.canRotate = true;
    }
}



