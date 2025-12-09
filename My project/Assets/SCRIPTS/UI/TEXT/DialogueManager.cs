using System;
using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;

    public Fade fade;

    //  Fired after the *whole* dialogue sequence is finished
    // (all lines shown AND last one dismissed by player)
    public static event Action OnDialogueSequenceFinished;

    void Awake()
    {
        Instance = this;
    }

    // Single line
    public void Say(string speaker, string text, TMP_FontAsset font = null, Sprite portrait = null)
    {
        StartCoroutine(SayRoutine(speaker, text, font, portrait));
    }

    // Multiple lines (for tutorials, conversations, etc.)
    public void SayLines(string speaker, string[] lines, TMP_FontAsset font = null, Sprite portrait = null)
    {
        StartCoroutine(SayLinesRoutine(speaker, lines, font, portrait));
    }

    IEnumerator SayRoutine(string speaker, string text, TMP_FontAsset font, Sprite portrait)
    {
        bool dismissed = false;
        void HandleDismiss() => dismissed = true;

        fade.OnDismissed += HandleDismiss;
        fade.ShowText(speaker, text, font, portrait);

        yield return new WaitUntil(() => dismissed);

        fade.OnDismissed -= HandleDismiss;

        // single line finished & clicked -> hide
        if (fade.dialoguePanel != null)
            fade.dialoguePanel.SetActive(false);

        // notify listeners that the dialogue sequence is over
        OnDialogueSequenceFinished?.Invoke();
    }

    IEnumerator SayLinesRoutine(string speaker, string[] lines, TMP_FontAsset font, Sprite portrait)
    {
        foreach (string line in lines)
        {
            bool dismissed = false;
            void HandleDismiss() => dismissed = true;

            fade.OnDismissed += HandleDismiss;
            fade.ShowText(speaker, line, font, portrait);

            // wait until player clicks after the line is done typing
            yield return new WaitUntil(() => dismissed);

            fade.OnDismissed -= HandleDismiss;
        }

        // all lines done & last one clicked -> hide
        if (fade.dialoguePanel != null)
            fade.dialoguePanel.SetActive(false);

        // notify listeners that the dialogue sequence is over
        OnDialogueSequenceFinished?.Invoke();
    }
}



