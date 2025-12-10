using UnityEngine;
using TMPro;
using UnityEngine.UI;
using static UnityEngine.EventSystems.EventTrigger;
using Unity.VisualScripting;

public class IntroDialogue : MonoBehaviour
{
    [Header("References")]
    public Fade fade;
    public Movement playerMovement;
    public HorizontalMovement horizontalLook;
    public RotacionVertical verticalLook;
    public bool isTutorial = false;

    [Header("Speaker")]

    public TMP_FontAsset sammyFont;
    public Sprite sammyPortrait;

    [Header("Dialogue")]
    [TextArea(3, 6)]
    public string[] lines;


    void Start()
    {
        LockPlayer(true);
        DialogueManager.OnDialogueSequenceFinished += OnIntroDialogueFinished;

        if (isTutorial)
        {

        DialogueManager.Instance.SayLines(
            "Spammy Sammy",
            new string[]
            {
                "Heya pal!! Welcome to your FIRST DAY at Station Hephaestus!",
                "I’m Spammy Sammy! I smile so you don’t have to!",
                "If we clean fast enough, we might even earn an extra five seconds of break time! We can DREAM!",
                "Now grab that mop and start cleaning these machines so we can fix them!!!"
            },
            sammyFont,
            sammyPortrait
        );
        }
        else
        {

        // 2) Subscribe to the dialogue finished event
        DialogueManager.Instance.SayLines(
            "Spammy Sammy",
            new string[]
            {
                "Ooooh! You’re awake! Hi there, EMPLOYEE #2163876!",
                "It’s your buddy Spammy Sammy—your emotionally supportive workplace companion model!",
                "I’m sooo excited to help you start your shift!",
                "You clean… I cheer… and together we make the company proud until our circuits or souls give out!",
                "Let’s do our best, okay? I believe in you!!!"
            },
            sammyFont,
            sammyPortrait
        );       
        }

    }
    void OnIntroDialogueFinished()
    {
        // 4) Unlock player when the whole dialogue sequence is done
        LockPlayer(false);

        // 5) Unsubscribe so this only runs once
        DialogueManager.OnDialogueSequenceFinished -= OnIntroDialogueFinished;
    }

    void OnDestroy()
    {
        // Safety: in case the object is destroyed mid-dialogue
        DialogueManager.OnDialogueSequenceFinished -= OnIntroDialogueFinished;
    }

    void LockPlayer(bool locked)
    {
        bool canMove = !locked;

        if (playerMovement != null)
            playerMovement.canMove = canMove;

        if (horizontalLook != null)
            horizontalLook.canMove = canMove;

        if (verticalLook != null)
            verticalLook.canRotate = canMove;
    }
}



