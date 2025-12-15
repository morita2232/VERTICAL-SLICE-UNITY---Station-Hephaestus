using UnityEngine;
using TMPro;

/// <summary>
/// Handles trash disposal logic.
/// Reacts differently depending on whether trash is thrown correctly
/// or incorrectly, and plays escalating dialogue responses.
/// </summary>
public class TrashCan : MonoBehaviour
{
    // ================================
    // Trash Behavior Settings
    // ================================

    [Header("Trash Behavior")]

    public bool acceptedWay;           // True if this trash can is the correct disposal method
    public bool alreadyTalked = false; // Prevents repeated praise dialogue


    // ================================
    // Dialogue Tracking
    // ================================

    private int items = 0;             // Number of improperly disposed trash items


    // ================================
    // Dialogue Assets
    // ================================

    [Header("Dialogue Assets")]

    public TMP_FontAsset sammyFont;     // Font for Sammy's dialogue
    public Sprite sammyPortrait;        // Sammy's portrait


    /// <summary>
    /// Triggered when trash enters the can's trigger
    /// </summary>
    private void OnTriggerEnter(Collider other)
    {
        // Only react to trash objects
        if (!other.CompareTag("Trash"))
            return;

        // Destroy the trash immediately
        Destroy(other.gameObject);

        if (acceptedWay)
        {
            // Correct disposal path
            if (!alreadyTalked)
            {
                DialogueManager.Instance.SayLines(
                    "Spammy Sammy",
                    new string[]
                    {
                        "Great job! That’s how we keep Station Hephaestus sparkling clean! Keep it up!"
                    },
                    sammyFont,
                    sammyPortrait
                );

                alreadyTalked = true;
            }
        }
        else
        {
            // Incorrect disposal path (escalating responses)
            if (items == 0)
            {
                DialogueManager.Instance.SayLines(
                    "Spammy Sammy",
                    new string[]
                    {
                        "I am going to let it pass this one time, but PLEASE don't throw trash like that again!"
                    },
                    sammyFont,
                    sammyPortrait
                );
                items++;
            }
            else if (items == 1)
            {
                DialogueManager.Instance.SayLines(
                    "Spammy Sammy",
                    new string[]
                    {
                        "Seriously? I thought we talked about this! Please be more careful with the trash!"
                    },
                    sammyFont,
                    sammyPortrait
                );
                items++;
            }
            else if (items == 2)
            {
                DialogueManager.Instance.SayLines(
                    "Spammy Sammy",
                    new string[]
                    {
                        "That's it! I'm reporting you to HR for improper waste disposal! You're on thin ice, employee!"
                    },
                    sammyFont,
                    sammyPortrait
                );
                items++;
            }
            else if (items >= 3)
            {
                DialogueManager.Instance.SayLines(
                    "Spammy Sammy",
                    new string[]
                    {
                        "STOP >:("
                    },
                    sammyFont,
                    sammyPortrait
                );
                items++;
            }
        }
    }
}


