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

    // When true the active dialogue coroutines will treat this as a dismissal
    private bool skipRequested = false;

    void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        // Cheat key: skip current dialogue sequence
        if (Input.GetKeyDown(KeyCode.F6))
        {
            RequestSkip();
        }
    }

    // Public helper to request a skip from other scripts if needed
    public void RequestSkip()
    {
        // mark request
        skipRequested = true;

        // immediately hide the panel so the player sees it disappear
        if (fade != null && fade.dialoguePanel != null && fade.dialoguePanel.activeSelf)
        {
            fade.dialoguePanel.SetActive(false);
        }
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

        // Wait until either the player dismisses OR a skip was requested
        yield return new WaitUntil(() => dismissed || skipRequested);

        fade.OnDismissed -= HandleDismiss;

        // single line finished & clicked (or skipped) -> hide
        if (fade.dialoguePanel != null)
            fade.dialoguePanel.SetActive(false);

        // If skip was requested, clear it now so future dialogues behave normally
        skipRequested = false;

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

            // wait until player clicks after the line is done typing OR skip requested
            yield return new WaitUntil(() => dismissed || skipRequested);

            fade.OnDismissed -= HandleDismiss;

            // If skip was requested, stop showing the remaining lines
            if (skipRequested)
                break;
        }

        // all lines done & last one clicked -> hide (or skipped)
        if (fade.dialoguePanel != null)
            fade.dialoguePanel.SetActive(false);

        // clear skip for future dialogues
        skipRequested = false;

        // notify listeners that the dialogue sequence is over
        OnDialogueSequenceFinished?.Invoke();
    }
}




