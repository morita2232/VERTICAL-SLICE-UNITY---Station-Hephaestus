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

    /*
    [Header("Choice UI")]
    public GameObject choicePanel;
    public UnityEngine.UI.Button letGoButton;
    public UnityEngine.UI.Button disposeButton;
    */
    private bool hasInteracted = false;

    private void Start()
    {
        /*
        choicePanel.SetActive(false);

        letGoButton.onClick.AddListener(() => Choose("letgo"));
        disposeButton.onClick.AddListener(() => Choose("dispose"));
        */
    }

    public void Interact()
    {
        if (hasInteracted)
            return;

        hasInteracted = true;

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
        /*
        // All dialogue done  show choices
        choicePanel.SetActive(true);
        */
    }

    /*
    private void Choose(string option)
    {
        choicePanel.SetActive(false);

        if (option == "letgo")
        {
            DialogueManager.Instance.SayLines(
                "Alien",
                new string[]{
                    "Thank you. I will leave now. My strawberries await me."
                },
                translationFont,
                alienPortrait
            );
        }
        else
        {
            DialogueManager.Instance.SayLines(
                "Spammy Sammy",
                new string[]{
                    "Correct decision, employee. The company appreciates your cooperation."
                }
            );
        }
    }
    */
}

