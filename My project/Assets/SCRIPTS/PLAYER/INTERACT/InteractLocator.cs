using UnityEngine;
using TMPro;

/// <summary>
/// Central interaction handler.
/// Casts a ray from the camera, determines what the player is looking at,
/// and routes interaction input to the correct system (minigames, mop,
/// spaceship, alien dialogue, etc.).
/// </summary>
public class InteractLocator : MonoBehaviour
{
    // ================================
    // Interaction State
    // ================================

    [Header("Attributes")]

    public LayerMask interactMask;     // What layers can be interacted with
    public bool isInminigame = false;  // True while any minigame is active
    public bool canInteract = false;   // Used by UI popups
    public bool isInSpaceShip = false; // True when player is inside spaceship

    private bool oneTime = false;      // Used for one-time spaceship dialogue


    // ================================
    // Scene References
    // ================================

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


    // ================================
    // Internal State
    // ================================

    private Camera mainCamera;
    private bool clicked;              // Interaction input (E key)
    public bool isTutorial = false;    // Tutorial mode flag


    void Start()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        // Read interaction input
        clicked = Input.GetKeyDown(KeyCode.E);

        Debug.Log($"InteractLocator Update() | clicked: {clicked}");

        // Raycast from center of the screen
        Ray ray = mainCamera.ScreenPointToRay(
            new Vector3(Screen.width / 2, Screen.height / 2, 0));

        if (Physics.Raycast(ray, out RaycastHit hit, 2f, interactMask))
        {
            canInteract = true;

            bool isMinigameObject = hit.collider.CompareTag("Miniggame");
            bool isSpaceShipObject = hit.collider.CompareTag("SpaceShip");
            bool isMopObject = hit.collider.CompareTag("Mop");
            bool isAlien = hit.collider.CompareTag("Alien");

            // Only act when interaction key is pressed
            if (clicked)
            {
                // ================================
                // MINIGAME OBJECTS
                // ================================

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

                    // Block interaction if surface is dirty
                    if (cleanable != null && !cleanable.IsClean)
                    {
                        DialogueManager.Instance.SayLines(
                            "Spammy Sammy",
                            new string[]
                            {
                                "Uhm, I may need to give you a cerebral inspection clearly this machine is dirty!"
                            },
                            sammyFont,
                            sammyPortrait
                        );
                        return;
                    }

                    // ---- WIRE COMPUTER ----
                    else if (wireComp != null)
                    {
                        if (isTutorial)
                        {
                            DialogueManager.Instance.SayLines(
                                "Spammy Sammy",
                                new string[]
                                {
                                    "To fix this machine you will have to reconnect each power source to its appropriate connector",
                                    "With your MOUSE, CLICK and DRAG the power sources from the LEFT to the connectors in the RIGHT"
                                },
                                sammyFont,
                                sammyPortrait
                            );
                        }

                        wireComp.Interact();
                        wireComp = null;
                    }

                    // ---- BALL BALANCE ----
                    else if (balanceComp != null)
                    {
                        if (isTutorial)
                        {
                            DialogueManager.Instance.SayLines(
                                "Spammy Sammy",
                                new string[]
                                {
                                    "To fix this machine you will have to recalibrate the balance mechanism",
                                    "Use W/A/S/D to balance the ball and guide it to the RED hole"
                                },
                                sammyFont,
                                sammyPortrait
                            );
                        }

                        balanceComp.Interact();
                        balanceComp = null;
                    }

                    // ---- CONDUIT ----
                    else if (conduitComp != null)
                    {
                        if (isTutorial)
                        {
                            // Delay start until dialogue finishes
                            DialogueManager.OnDialogueSequenceFinished += StartConduitAfterDialogue;

                            DialogueManager.Instance.SayLines(
                                "Spammy Sammy",
                                new string[]
                                {
                                    "To fix this machine you will have to restore the energy from the systems.",
                                    "Use your W to change to the next conduit, S to change to the previous conduit.",
                                    "Balance their energy until they reach the red bars at the same time."
                                },
                                sammyFont,
                                sammyPortrait
                            );
                        }
                        else
                        {
                            conduitComp.Interact();
                            conduitComp = null;
                        }
                    }
                }

                // ================================
                // SPACESHIP
                // ================================

                if (isSpaceShipObject)
                {
                    if (!oneTime)
                    {
                        DialogueManager.OnDialogueSequenceFinished += EnterSpaceShip;

                        DialogueManager.Instance.SayLines(
                            "Spammy Sammy",
                            new string[]
                            {
                                "Attention. Prior to entry, be aware that a classified alien entity has been detected inside.",
                                "Company threat assessment rates the subject as high-risk.",
                                "Engage with caution and follow all safety procedures.",
                                "For safety reasons I must remain outside, but do not worry employee, I will be looking at the cameras :)",
                                "(Also do not forget you can come out the spaceship pressing your R KEY)"
                            },
                            sammyFont,
                            sammyPortrait
                        );

                        oneTime = true;
                    }
                    else
                    {
                        Debug.Log("Entering spaceship...");
                        player.transform.position = spaceshipSpawn.position;
                        isInSpaceShip = true;
                    }
                }

                // ================================
                // MOP PICKUP
                // ================================

                if (isMopObject)
                {
                    var mopComp = hit.collider.GetComponent<Mop>();
                    mopComp.PickUp();
                    mopComp = null;
                    Debug.Log("A limpiar guarrilla");
                }

                // ================================
                // ALIEN INTERACTION
                // ================================

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

    // ================================
    // Dialogue Callbacks
    // ================================

    void StartConduitAfterDialogue()
    {
        DialogueManager.OnDialogueSequenceFinished -= StartConduitAfterDialogue;

        Ray ray = mainCamera.ScreenPointToRay(
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
        DialogueManager.OnDialogueSequenceFinished -= EnterSpaceShip;

        if (player == null || spaceshipSpawn == null)
        {
            Debug.LogWarning("EnterSpaceShip: player or spaceshipSpawn is null");
            return;
        }

        Debug.Log("Entering spaceship...");
        player.transform.position = spaceshipSpawn.position;
        isInSpaceShip = true;
    }

    // ================================
    // Debug Gizmos
    // ================================

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



