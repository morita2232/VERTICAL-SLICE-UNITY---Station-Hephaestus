using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Handles dialogue text display with a typewriter effect.
/// Supports speaker name, portrait, font overrides, and
/// user-driven dismissal after text finishes typing.
/// </summary>
public class Fade : MonoBehaviour
{
    // ================================
    // Typing Settings
    // ================================

    [Header("Typing")]

    public float delayPerCharacter = 0.05f;   // Speed of typewriter effect


    // ================================
    // UI References
    // ================================

    [Header("UI References")]

    public TextMeshProUGUI speakerText;        // Speaker name (e.g. Spammy Sammy)
    public TextMeshProUGUI textMesh;           // Dialogue body text
    public Image portraitImage;                // Optional portrait image
    public GameObject dialoguePanel;            // Entire dialogue UI panel
    public GameObject continueIcon;             // Optional "press to continue" indicator


    // ================================
    // Font Settings
    // ================================

    [Header("Fonts")]

    public TMP_FontAsset defaultFont;           // Font applied to both speaker and text


    // ================================
    // Internal State
    // ================================

    private Coroutine typingRoutine;            // Active typing coroutine
    private bool waitingForDismiss;             // Waiting for player input


    // ================================
    // Public State
    // ================================

    public bool IsFinished { get; private set; } // True when line finished typing


    // ================================
    // Events
    // ================================

    /// <summary>
    /// Fired when the last character of the current line appears.
    /// </summary>
    public event Action OnFinished;

    /// <summary>
    /// Fired when the player clicks or presses a key after typing finishes.
    /// </summary>
    public event Action OnDismissed;


    void Awake()
    {
        // Default dialogue panel to this GameObject if none assigned
        if (dialoguePanel == null)
            dialoguePanel = gameObject;

        // Prepare text mesh for character revealing
        if (textMesh != null)
        {
            textMesh.ForceMeshUpdate();
            textMesh.maxVisibleCharacters = 0;
        }

        // Apply default font to speaker and dialogue text
        if (defaultFont != null)
        {
            if (speakerText != null) speakerText.font = defaultFont;
            if (textMesh != null) textMesh.font = defaultFont;
        }
    }

    void Update()
    {
        // After typing finishes, wait for any input to continue
        if (IsFinished && waitingForDismiss)
        {
            if (Input.anyKeyDown || Input.GetMouseButtonDown(0))
            {
                waitingForDismiss = false;
                OnDismissed?.Invoke();   // Notify listeners to advance dialogue
            }
        }
    }

    /// <summary>
    /// Displays dialogue text with optional speaker name, font override, and portrait.
    /// </summary>
    public void ShowText(
        string speakerName,
        string textToShow,
        TMP_FontAsset overrideFont = null,
        Sprite portrait = null)
    {
        if (continueIcon != null)
            continueIcon.SetActive(false);

        if (dialoguePanel != null)
            dialoguePanel.SetActive(true);

        if (typingRoutine != null)
            StopCoroutine(typingRoutine);

        waitingForDismiss = false;
        IsFinished = false;

        // Choose which font to use
        TMP_FontAsset useFont = overrideFont != null ? overrideFont : defaultFont;

        if (speakerText != null)
        {
            speakerText.text = speakerName;
            if (useFont != null) speakerText.font = useFont;
        }

        if (textMesh != null)
        {
            textMesh.text = textToShow;
            textMesh.maxVisibleCharacters = 0;
            if (useFont != null) textMesh.font = useFont;
        }

        if (portraitImage != null)
        {
            portraitImage.sprite = portrait;
            portraitImage.enabled = (portrait != null);
        }

        typingRoutine = StartCoroutine(RevealCharacters());
    }

    IEnumerator RevealCharacters()
    {
        int totalCharacters = textMesh.text.Length;

        for (int i = 0; i <= totalCharacters; i++)
        {
            textMesh.maxVisibleCharacters = i;
            yield return new WaitForSeconds(delayPerCharacter);
        }

        typingRoutine = null;
        IsFinished = true;

        if (continueIcon != null)
            continueIcon.SetActive(true);

        waitingForDismiss = true;
        OnFinished?.Invoke();   // Notify listeners that typing is complete
    }
}




