using TMPro;
using UnityEngine;

public class IntroDialogue : MonoBehaviour
{
    [Header("References")]
    public Fade fade;
    public Movement playerMovement;
    public HorizontalMovement horizontalLook;
    public RotacionVertical verticalLook;

    [Header("Speaker")]
    public string robotName = "Spammy Sammy";
    public TMP_FontAsset robotFont;
    public Sprite robotPortrait;

    [Header("Dialogue")]
    [TextArea(3, 6)]
    public string[] lines;

    private int currentLineIndex = 0;

    void Start()
    {
        LockPlayer(true);

        if (lines == null || lines.Length == 0)
        {
            LockPlayer(false);
            return;
        }

        // Listen for "finished typing"
        fade.OnFinished += HandleTextFinished;

        // Listen for "player pressed key, box hidden"
        fade.OnDismissed += HandleDialogueDismissed;

        PlayCurrentLine();
    }

    void PlayCurrentLine()
    {
        fade.ShowText(
            robotName,
            lines[currentLineIndex],
            robotFont,
            robotPortrait
        );
    }

    void HandleTextFinished()
    {
        // If there are more lines, start the next one immediately
        currentLineIndex++;

        if (currentLineIndex < lines.Length)
        {
            PlayCurrentLine();
        }
        else
        {
            // Last line has finished typing, now we just wait for a key press
            // Unlock will happen in HandleDialogueDismissed
        }
    }

    void HandleDialogueDismissed()
    {
        // All done, let the player move and look around
        LockPlayer(false);

        // No need for further events
        fade.OnFinished -= HandleTextFinished;
        fade.OnDismissed -= HandleDialogueDismissed;
    }

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


