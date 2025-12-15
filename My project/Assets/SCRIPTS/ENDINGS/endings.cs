using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Handles good and bad endings:
/// - Dialogue
/// - Visual state changes
/// - Scene transitions
/// </summary>
public class endings : MonoBehaviour
{
    // ================================
    // Player & Scene References
    // ================================

    [Header("Player & Scene References")]

    public InteractLocator interactLocator; // Tracks spaceship interaction state
    public GameObject player;               // Player object
    public Transform returnPosition;        // Player teleport position


    // ================================
    // Good Ending (Sammy)
    // ================================

    [Header("Good Ending - Sammy Dialogue")]

    public TMP_FontAsset sammyFont;          // Font for Sammy's dialogue
    public Sprite sammyPortrait;             // Sammy's portrait


    // ================================
    // Bad Ending (Alien)
    // ================================

    [Header("Bad Ending - Alien Dialogue & Visuals")]

    public TMP_FontAsset translationFont;    // Readable alien dialogue font
    public Sprite alienPortrait;             // Alien portrait
    public GameObject alien;                 // Alien character object
    public GameObject exes;                  // Visual effect shown after alien collapses


    /// <summary>
    /// GOOD ENDING:
    /// Player lets the alien go.
    /// </summary>
    public void EndingOne()
    {
        // Return player to position
        player.transform.position = returnPosition.position;
        interactLocator.isInSpaceShip = false;

        // Sammy reacts
        DialogueManager.Instance.SayLines(
            "Spammy Sammy",
            new string[]
            {
                "Uhm,buddy?, why is the ship leaving?",
                "What did you do?",
                "I thought we were friends...",
                "The company is not going to be happy about this..."
            },
            sammyFont,
            sammyPortrait
        );

        // Load good ending scene
        StartCoroutine(LoadNextSceneAfterDelay(5f, true));
    }

    /// <summary>
    /// BAD ENDING:
    /// Player disposes of the alien.
    /// </summary>
    public void EndingTwo()
    {
        // Wait for dialogue to finish before triggering collapse
        DialogueManager.OnDialogueSequenceFinished += LayDown;

        DialogueManager.Instance.SayLines(
            "Alien",
            new string[]
            {
                "I understand...",
                "You had to make a choice.",
                "I hope you find peace with it.",
                "Farewell, human."
            },
            translationFont,
            alienPortrait
        );
    }

    /// <summary>
    /// Triggered after alien dialogue finishes
    /// </summary>
    void LayDown()
    {
        DialogueManager.OnDialogueSequenceFinished -= LayDown;

        // Position alien as collapsed
        alien.transform.rotation = Quaternion.Euler(-90f, 0f, 0f);
        alien.transform.position = new Vector3(85.25f, 0.402f, 22.33f);

        // Show visual effect
        exes.SetActive(true);

        // Load bad ending scene
        StartCoroutine(LoadNextSceneAfterDelay(5f, false));
    }

    /// <summary>
    /// Loads the appropriate ending scene after a delay
    /// </summary>
    IEnumerator LoadNextSceneAfterDelay(float delay, bool ending)
    {
        yield return new WaitForSeconds(delay);

        if (ending)
            SceneManager.LoadScene("GoodEnding");
        else
            SceneManager.LoadScene("BadEnding");
    }
}



