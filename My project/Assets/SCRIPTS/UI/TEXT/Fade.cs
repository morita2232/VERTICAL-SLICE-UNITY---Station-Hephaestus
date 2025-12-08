using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Fade : MonoBehaviour
{
    [Header("Typing")]
    public float delayPerCharacter = 0.05f;

    [Header("UI References")]
    public TextMeshProUGUI speakerText;   // name (Spammy Sammy)
    public TextMeshProUGUI textMesh;      // dialogue text
    public Image portraitImage;           // optional
    public GameObject dialoguePanel;      // entire panel (for hiding/showing)
    public GameObject continueIcon;      // optional "press to continue" icon

    [Header("Fonts")]
    public TMP_FontAsset defaultFont;     // font applied to BOTH name + text

    private Coroutine typingRoutine;
    private bool waitingForDismiss;

    public bool IsFinished { get; private set; }

    /// <summary> Fired when the last character of the current line appears. </summary>
    public event Action OnFinished;

    /// <summary> Fired when player clicks/presses a key AFTER a line is finished. </summary>
    public event Action OnDismissed;

    void Awake()
    {
        if (dialoguePanel == null)
            dialoguePanel = gameObject;

        if (textMesh != null)
        {
            textMesh.ForceMeshUpdate();
            textMesh.maxVisibleCharacters = 0;
        }

        if (defaultFont != null)
        {
            if (speakerText != null) speakerText.font = defaultFont;
            if (textMesh != null) textMesh.font = defaultFont;
        }
    }

    void Update()
    {
        // After finishing typing, wait for any key / click to advance
        if (IsFinished && waitingForDismiss)
        {
            if (Input.anyKeyDown || Input.GetMouseButtonDown(0))
            {
                waitingForDismiss = false;
                OnDismissed?.Invoke();   // tell listener: "player wants to continue"
            }
        }
    }

    /// <summary>
    /// Show text, set speaker and optional portrait.
    /// </summary>
    public void ShowText(
        string speakerName,
        string textToShow,
        TMP_FontAsset overrideFont = null,
        Sprite portrait = null)
    {
        continueIcon.SetActive(false);
        if (dialoguePanel != null)
            dialoguePanel.SetActive(true);   // make sure the box is visible

        if (typingRoutine != null)
            StopCoroutine(typingRoutine);

        waitingForDismiss = false;
        IsFinished = false;

        // Decide which font to use
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
        continueIcon.SetActive(true);
        waitingForDismiss = true;   // now wait for click/key
        OnFinished?.Invoke();       // finished typing this line
    }
}




