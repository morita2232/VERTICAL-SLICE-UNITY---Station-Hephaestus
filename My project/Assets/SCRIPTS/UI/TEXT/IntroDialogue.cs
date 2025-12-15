using UnityEngine;
using TMPro;

/// <summary>
/// Plays the intro dialogue at the start of a level.
/// Locks player controls while dialogue is active and
/// restores control once the dialogue finishes.
/// </summary>
public class IntroDialogue : MonoBehaviour
{
    // ================================
    // References
    // ================================

    [Header("References")]

    public Fade fade;                          // Optional fade controller
    public Movement playerMovement;            // Player movement script
    public HorizontalMovement horizontalLook;  // Horizontal camera look
    public RotacionVertical verticalLook;      // Vertical camera look
    public bool isTutorial = false;             // Determines which intro dialogue plays


    // ================================
    // Speaker Assets
    // ================================

    [Header("Speaker")]

    public TMP_FontAsset sammyFont;             // Font used for Sammy's dialogue
    public Sprite sammyPortrait;                // Portrait shown during dialogue


    // ================================
    // Dialogue Content
    // ================================

    [Header("Dialogue")]

    [TextArea(3, 6)]
    public string[] lines;                      // (Currently unused, reserved for future use)


    void Start()
    {
        // Lock player movement and camera
        LockPlayer(true);

        // Listen for dialogue completion
        DialogueManager.OnDialogueSequenceFinished += OnIntroDialogueFinished;

        // ================================
        // Tutorial Intro
        // ================================

        if (isTutorial)
        {
            DialogueManager.Instance.SayLines(
                "Spammy Sammy",
                new string[]
                {
                    "Heya pal!! Welcome to your FIRST DAY at Station Hephaestus!",
                    "I’m Spammy Sammy! I smile so you don’t have to!",
                    "If we clean fast enough, we might even earn an extra five seconds of break time! We can DREAM!",
                    "Now grab that mop and start cleaning these machines so we can fix them!!!"
                },
                sammyFont,
                sammyPortrait
            );
        }
        // ================================
        // Main Game Intro
        // ================================
        else
        {
            DialogueManager.Instance.SayLines(
                "Spammy Sammy",
                new string[]
                {
                    "Ooooh! You’re awake! Hi there, EMPLOYEE #2163876!",
                    "It’s your buddy Spammy Sammy—your emotionally supportive workplace companion model!",
                    "I’m sooo excited to help you start your shift!",
                    "You clean… I cheer… and together we make the company proud until our circuits or souls give out!",
                    "Let’s do our best, okay? I believe in you!!!"
                },
                sammyFont,
                sammyPortrait
            );
        }
    }

    /// <summary>
    /// Called when the entire intro dialogue sequence finishes
    /// </summary>
    void OnIntroDialogueFinished()
    {
        // Unlock player controls
        LockPlayer(false);

        // Unsubscribe so this only runs once
        DialogueManager.OnDialogueSequenceFinished -= OnIntroDialogueFinished;
    }

    void OnDestroy()
    {
        // Safety: unsubscribe if object is destroyed mid-dialogue
        DialogueManager.OnDialogueSequenceFinished -= OnIntroDialogueFinished;
    }

    /// <summary>
    /// Enables or disables player movement and camera control
    /// </summary>
    void LockPlayer(bool locked)
    {
        bool canMove = !locked;

        if (playerMovement != null)
            playerMovement.canMove = canMove;

        if (horizontalLook != null)
            horizontalLook.canMove = canMove;

        if (verticalLook != null)
            verticalLook.canRotate = canMove;
    }
}




