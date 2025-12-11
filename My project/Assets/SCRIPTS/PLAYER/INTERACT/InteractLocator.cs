using NUnit.Framework;
using System.ComponentModel;
using System.Threading;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;
using UnityEngine.Rendering.VirtualTexturing;

public class InteractLocator : MonoBehaviour
{
    [Header("Attributes")]
    public LayerMask interactMask;
    public bool isInminigame = false;
    public bool canInteract = false;
    public bool isInSpaceShip = false;
    private bool oneTime = false;

    [Header("References")]
    public GameObject miniGame;
    public GameObject player;
    public GameObject sSammy;
    public Transform spaceshipSpawn;
    public TMP_FontAsset sammyFont;
    public Sprite sammyPortrait;

    public GameObject conduitObject;
    public GameObject ballBalanceObject;
    public GameObject wireComputerObject;

    private Camera mainCamera;
    private InputSystem_Actions inputs;
    private bool clicked;
    public bool isTutorial = false;

    void Start()
    {
        mainCamera = Camera.main;
        //inputs = new InputSystem_Actions();
        //inputs.Enable();
    }


    //void OnDisable()
    //{
    //    if (inputs != null)
    //        inputs.Disable();
    //}

    void Update()
    {

        //clicked = inputs.Player.Interact.ReadValue<float>() > 0;
        clicked = Input.GetKeyDown(KeyCode.E);

        Debug.Log($"InteractLocator Update() | clicked: {clicked}");

        Ray ray = mainCamera.ScreenPointToRay(
            new Vector3(Screen.width / 2, Screen.height / 2, 0));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 2f, interactMask))
        {
            canInteract = true;
   

            bool isMinigameObject = hit.collider.CompareTag("Miniggame");
            bool isSpaceShipObject = hit.collider.CompareTag("SpaceShip");
            bool isMopObject = hit.collider.CompareTag("Mop");
            bool isAlien = hit.collider.CompareTag("Alien");

            // Only DO things when the interact button is actually pressed
            if (clicked)
            {
                // ---- MINIGAME ----
                if (isMinigameObject)
                {
                    var wireComp = hit.collider.GetComponent<WireComputer>();
                    var balanceComp = hit.collider.GetComponent<BallBalanceObject>();
                    var conduitComp = hit.collider.GetComponent<ConduitObject>();


                    var cleanable = hit.collider.GetComponent<CleanableSurface>();

                    Debug.Log(
                        $"Hit {hit.collider.name} | Layer:{LayerMask.LayerToName(hit.collider.gameObject.layer)} " +
                        $"| Tag:{hit.collider.tag} | Cleanable:{(cleanable ? "YES" : "NO")} " +
                        $"| IsClean:{(cleanable ? cleanable.IsClean : false)}"
                    );

                    if (cleanable != null && !cleanable.IsClean)
                    {
                        DialogueManager.Instance.SayLines(
                           "Spammy Sammy",
                           new string[]
                           {
                                "Uhm, I may need to give you a cerebral inspection clearly this machine is dirty!"
                           }, sammyFont, sammyPortrait
                        );
                        return;
                    }

                   else if (wireComp != null)
                   {
                        if (isTutorial)
                        {
                            DialogueManager.Instance.SayLines(
                               "Spammy Sammy",
                               new string[]
                               {
                                "To fix this machine you will have to reconnect each power source to its appropriate connector",
                                 "With your MOUSE, CLICK and DRAG the power sources from the LEFT to the connectors in the RIGHT"},
                                 sammyFont, sammyPortrait
                               );
                        }
                        wireComp.Interact();   // this calls WireMGManager.OpenForComputer(...)
                        wireComp = null;
                    }

                   else if(balanceComp != null)
                    {
                        if (isTutorial)
                        {
                            DialogueManager.Instance.SayLines(
                               "Spammy Sammy",
                               new string[]
                               {
                                "To fix this machine you will have to recalibrate the balance mechanism",
                                 "Use W/A/S/D to balance the ball and guide it to the RED hole"},
                                 sammyFont, sammyPortrait
                               );

                        }
                        balanceComp.Interact();
                        balanceComp = null;
                    }
                    else if (conduitComp != null)
                    {
                        if (isTutorial)
                        {
                            // 1. Disable interaction until Sammy finishes talking
                            DialogueManager.OnDialogueSequenceFinished += StartConduitAfterDialogue;

                            // 2. Sammy explains how the minigame works
                            DialogueManager.Instance.SayLines(
                                "Spammy Sammy",
                                new string[]
                                {
                                    "To fix this machine you will have to restore the energy from the systems.",
                                    "Use your W to change to the next conduit, S to change to the previous conduit.",
                                    "Balance their energy until they reach the red bars at the same time."
                                },
                                sammyFont, sammyPortrait
                            );

                        }
                        else
                        {

                        conduitComp.Interact();
                            conduitComp = null;
                        }

                        
                    }

                }

                // ---- SPACESHIP ----
                if (isSpaceShipObject)
                {
                    if (!oneTime)
                    {
                        // 1. Disable interaction until Sammy finishes talking
                        DialogueManager.OnDialogueSequenceFinished += EnterSpaceShip;

                        // 2. Sammy explains how the minigame works
                        DialogueManager.Instance.SayLines(
                        "Spammy Sammy",
                        new string[]
                        {
                            "Attention. Prior to entry, be aware that a classified alien entity has been detected inside.",
                            "Company threat assessment rates the subject as high-risk. ",
                            "Engage with caution and follow all safety procedures."
                            },
                        sammyFont, sammyPortrait
                        );
                        oneTime = true;
                    }
                   
                    else { 
                        Debug.Log("Entering spaceship...");
                        player.transform.position = spaceshipSpawn.position;
                        isInSpaceShip = true;
                    } 
                }
                if (isMopObject)
                {
                    var mopComp = hit.collider.GetComponent<Mop>();
                    mopComp.PickUp();
                    mopComp = null;
                    Debug.Log("A limpiar guarrilla");
                }
                if (isAlien)
                {
                    var alienComp = hit.collider.GetComponent<AlienInteraction>();
                    alienComp.Interact();
                    alienComp = null;
                    Debug.Log("Interacting with Alien");
                }
            }
        }
        else
        {
            canInteract = false;
            Debug.Log("Raycast hit nothing in interactMask");
        }
    }
    void StartConduitAfterDialogue()
    {
        // Always unsubscribe so it doesn't trigger again
        DialogueManager.OnDialogueSequenceFinished -= StartConduitAfterDialogue;

        // Start the conduit minigame AFTER Sammy finishes talking
        var ray = mainCamera.ScreenPointToRay(
            new Vector3(Screen.width / 2, Screen.height / 2, 0));

        if (Physics.Raycast(ray, out RaycastHit hit, 2f, interactMask))
        {
            var conduitComp = hit.collider.GetComponentInParent<ConduitObject>();
            if (conduitComp != null)
            {
                conduitComp.Interact();
                conduitComp = null;
            }
        }
    }

    void EnterSpaceShip()
    {
        // Always unsubscribe so it only fires once
        DialogueManager.OnDialogueSequenceFinished -= EnterSpaceShip;

        // Optional safety: check references
        if (player == null || spaceshipSpawn == null)
        {
            Debug.LogWarning("EnterSpaceShip: player or spaceshipSpawn is null");
            return;
        }

        Debug.Log("Entering spaceship...");
        player.transform.position = spaceshipSpawn.position;
        isInSpaceShip = true;
    }


    private void OnDrawGizmos()
    {
        if (mainCamera == null)
            mainCamera = Camera.main;

        Gizmos.color = Color.red;
        Ray ray = mainCamera.ScreenPointToRay(
            new Vector3(Screen.width / 2, Screen.height / 2, 0));
        Gizmos.DrawRay(ray.origin, ray.direction * 5f);
    }
}


