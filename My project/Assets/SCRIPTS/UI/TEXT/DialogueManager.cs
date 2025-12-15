using System;
using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

/// <summary>
/// Central controller for all dialogue sequences.
/// Coordinates Fade display, line progression, skipping,
/// and notifies listeners when a dialogue sequence ends.
/// </summary>
public class DialogueManager : MonoBehaviour
{
    // ================================
    // Singleton
    // ================================

    public static DialogueManager Instance;


    // ================================
    // References
    // ================================

    public Fade fade;   // Handles dialogue UI and typewriter effect


    // ================================
    // Events
    // ================================

    /// <summary>
    /// Fired after the entire dialogue sequence finishes
    /// (all lines shown and last one dismissed or skipped).
    /// </summary>
    public static event Action OnDialogueSequenceFinished;


    // ================================
    // Internal State
    // ================================

    // When true, active dialogue coroutines treat this as a dismissal
    private bool skipRequested = false;


    void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        // Cheat key: skip the current dialogue sequence
        if (Input.GetKeyDown(KeyCode.F6))
        {
            RequestSkip();
        }
    }

    /// <summary>
    /// Requests skipping the current dialogue sequence.
    /// Can be called internally or by other scripts.
    /// </summary>
    public void RequestSkip()
    {
        // Mark skip request
        skipRequested = true;

        // Immediately hide dialogue panel for instant feedback
        if (fade != null && fade.dialoguePanel != null && fade.dialoguePanel.activeSelf)
        {
            fade.dialoguePanel.SetActive(false);
        }
    }

    /// <summary>
    /// Displays a single dialogue line.
    /// </summary>
    public void Say(string speaker, string text, TMP_FontAsset font = null, Sprite portrait = null)
    {
        StartCoroutine(SayRoutine(speaker, text, font, portrait));
    }

    /// <summary>
    /// Displays multiple dialogue lines sequentially.
    /// </summary>
    public void SayLines(string speaker, string[] lines, TMP_FontAsset font = null, Sprite portrait = null)
    {
        StartCoroutine(SayLinesRoutine(speaker, lines, font, portrait));
    }


    // ================================
    // Coroutines
    // ================================

    IEnumerator SayRoutine(string speaker, string text, TMP_FontAsset font, Sprite portrait)
    {
        bool dismissed = false;
        void HandleDismiss() => dismissed = true;

        fade.OnDismissed += HandleDismiss;
        fade.ShowText(speaker, text, font, portrait);

        // Wait until the player dismisses or a skip is requested
        yield return new WaitUntil(() => dismissed || skipRequested);

        fade.OnDismissed -= HandleDismiss;

        // Hide dialogue panel after single line
        if (fade.dialoguePanel != null)
            fade.dialoguePanel.SetActive(false);

        // Reset skip flag for future dialogues
        skipRequested = false;

        // Notify listeners that the dialogue sequence finished
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

            // Wait for dismissal or skip request
            yield return new WaitUntil(() => dismissed || skipRequested);

            fade.OnDismissed -= HandleDismiss;

            // If skip requested, stop showing remaining lines
            if (skipRequested)
                break;
        }

        // Hide dialogue panel after final line or skip
        if (fade.dialoguePanel != null)
            fade.dialoguePanel.SetActive(false);

        // Reset skip flag for future dialogues
        skipRequested = false;

        // Notify listeners that the dialogue sequence finished
        OnDialogueSequenceFinished?.Invoke();
    }
}





