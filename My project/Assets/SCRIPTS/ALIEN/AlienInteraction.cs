using UnityEngine;
using TMPro;
using System.Collections;

public class AlienInteraction : MonoBehaviour
{
    [Header("Alien Dialogue")]
    public TMP_FontAsset alienFont;          // unreadable/alien font
    public TMP_FontAsset translationFont;    // normal readable font
    public Sprite alienPortrait;

    [TextArea] public string[] alienLines;       // original alien language lines
    [TextArea] public string[] translatedLines;  // human translations


    [Header("Choice UI")]
    public GameObject choicePanel;
    public UnityEngine.UI.Button letGoButton;
    public UnityEngine.UI.Button disposeButton;
    public GameObject crossHair;
    public Movement playerMovement;
    public HorizontalMovement horizontalLook;
    public RotacionVertical verticalLook;
    private bool hasInteracted = false;
    public endings endingScript;

    private void Start()
    {

        choicePanel.SetActive(false);

        letGoButton.onClick.AddListener(() => Choose("letgo"));
        disposeButton.onClick.AddListener(() => Choose("dispose"));

    }

    public void Interact()
    {
        if (hasInteracted)
            return;

        hasInteracted = true;
        // Save & disable controls
        if (playerMovement != null)
        {

            playerMovement.canMove = false;
        }
        if (horizontalLook != null)
        {

            horizontalLook.canMove = false;
        }
        if (verticalLook != null)
        {

            verticalLook.canRotate = false;
        }

        // Begin the dual-language dialogue sequence
        StartCoroutine(PlayAlienDialogue());
    }

    private IEnumerator PlayAlienDialogue()
    {
        // Safety: both arrays must match
        if (alienLines.Length != translatedLines.Length)
        {
            Debug.LogError("AlienInteraction: alienLines and translatedLines must be the same length.");
            yield break;
        }

        for (int i = 0; i < alienLines.Length; i++)
        {
            // 1) Alien gibberish line
            bool finished = false;
            void Done1() => finished = true;
            DialogueManager.OnDialogueSequenceFinished += Done1;

            DialogueManager.Instance.Say("Alien", alienLines[i], alienFont, alienPortrait);
            yield return new WaitUntil(() => finished);
            DialogueManager.OnDialogueSequenceFinished -= Done1;

            // 2) English translation line
            finished = false;
            void Done2() => finished = true;
            DialogueManager.OnDialogueSequenceFinished += Done2;

            DialogueManager.Instance.Say("Alien", translatedLines[i], translationFont, alienPortrait);
            yield return new WaitUntil(() => finished);
            DialogueManager.OnDialogueSequenceFinished -= Done2;
        }

        // All dialogue done  show choices
        crossHair.SetActive(false);
        Cursor.lockState = CursorLockMode.None;
        choicePanel.SetActive(true);

    }


    private void Choose(string option)
    {
        choicePanel.SetActive(false);
        crossHair.SetActive(true);
        Cursor.lockState = CursorLockMode.Locked;

        if (option == "letgo")
        {
            endingScript.EndingOne();
        }
        else
        {
            endingScript.EndingTwo();
        }
        if (playerMovement != null)
            playerMovement.canMove = true;
        if (horizontalLook != null)
            horizontalLook.canMove = true;
        if (verticalLook != null)
            verticalLook.canRotate = true;
    }

}

