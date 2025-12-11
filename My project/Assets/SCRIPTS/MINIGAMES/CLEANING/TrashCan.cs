using UnityEngine;
using TMPro;

public class TrashCan : MonoBehaviour
{
    public bool acceptedWay;
    public bool alreadyTalked = false;
    private int items = 0;
    public TMP_FontAsset sammyFont;
    public Sprite sammyPortrait;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Trash"))
            return;

        // Destroy the trash immediately (or do any effects first)
        Destroy(other.gameObject);

        if (acceptedWay)
        {
            if (!alreadyTalked)
            {
                DialogueManager.Instance.SayLines(
                    "Spammy Sammy",
                    new string[] { "Great job! That’s how we keep Station Hephaestus sparkling clean! Keep it up!" },
                    sammyFont,
                    sammyPortrait
                );
                alreadyTalked = true;
            }
        }
        else
        {
            // Use else-if so only one message is shown per triggered trash
            if (items == 0)
            {
                DialogueManager.Instance.SayLines(
                    "Spammy Sammy",
                    new string[] { "I am going to let it pass this one time, but PLEASE don't throw trash like that again!" },
                    sammyFont,
                    sammyPortrait
                );
                items++;
            }
            else if (items == 1)
            {
                DialogueManager.Instance.SayLines(
                    "Spammy Sammy",
                    new string[] { "Seriously? I thought we talked about this! Please be more careful with the trash!" },
                    sammyFont,
                    sammyPortrait
                );
                items++;
            }
            else if (items == 2)
            {
                DialogueManager.Instance.SayLines(
                    "Spammy Sammy",
                    new string[] { "That's it! I'm reporting you to HR for improper waste disposal! You're on thin ice, employee!" },
                    sammyFont,
                    sammyPortrait
                );
                items++;
            }
            else if (items >= 3)
            {
                DialogueManager.Instance.SayLines(
                    "Spammy Sammy",
                    new string[] { "STOP >:(" },
                    sammyFont,
                    sammyPortrait
                );
                items++;
            }
        }
    }
}

