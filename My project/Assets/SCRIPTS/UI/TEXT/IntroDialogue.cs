using UnityEngine;
using TMPro;
using UnityEngine.UI;

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

        if (fade == null || lines == null || lines.Length == 0)
        {
            LockPlayer(false);
            return;
        }

        // We only care about "player clicked to continue"
        fade.OnDismissed += HandleLineAdvance;

        // Start first line
        ShowCurrentLine();
    }

    void ShowCurrentLine()
    {
        fade.ShowText(
            robotName,
            lines[currentLineIndex],
            robotFont,
            robotPortrait
        );
    }

    void HandleLineAdvance()
    {
        currentLineIndex++;

        // More lines? Show next one.
        if (currentLineIndex < lines.Length)
        {
            ShowCurrentLine();
        }
        else
        {
            // No more lines: end dialogue, unlock player, hide panel
            fade.dialoguePanel.SetActive(false);
            fade.OnDismissed -= HandleLineAdvance;
            LockPlayer(false);
        }
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



