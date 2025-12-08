using TMPro;
using UnityEngine;

public class InteractLocator : MonoBehaviour
{
    [Header("Attributes")]
    public LayerMask interactMask;
    public bool isInminigame = false;
    public bool canInteract = false;
    public bool isInSpaceShip = false;
    
    [Header("References")]
    public GameObject miniGame;
    public GameObject player;
    public GameObject sSammy;
    public Transform spaceshipSpawn;
    public TMP_FontAsset sammyFont;
    public Sprite sammyPortrait;

    private Camera mainCamera;
    private InputSystem_Actions inputs;
    private bool clicked;
    public bool isTutorial = false;

    void Start()
    {
        mainCamera = Camera.main;
        inputs = new InputSystem_Actions();
        inputs.Enable();
    }


    void OnDisable()
    {
        if (inputs != null)
            inputs.Disable();
    }

    void Update()
    {

        clicked = inputs.Player.Interact.ReadValue<float>() > 0;

        Ray ray = mainCamera.ScreenPointToRay(
            new Vector3(Screen.width / 2, Screen.height / 2, 0));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 2f, interactMask))
        {
            canInteract = true;
            Debug.Log("Interacted with: " + hit.collider.gameObject.name);

            bool isMinigameObject = hit.collider.CompareTag("Miniggame");
            bool isSpaceShipObject = hit.collider.CompareTag("SpaceShip");
            bool isMopObject = hit.collider.CompareTag("Mop");

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

                    if (wireComp != null)
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
                    }

                    if(balanceComp != null)
                    {
                        if (isTutorial)
                        {
                            DialogueManager.Instance.SayLines(
                               "Spammy Sammy",
                               new string[]
                               {
                                "To fix this machine you will have to recalibrate the balance mechanism",
                                 "Use your MOVEMENT KEYS to balance the ball and guide it to the RED hole"},
                                 sammyFont, sammyPortrait
                               );

                        }
                        balanceComp.Interact();
                    }

                    if(conduitComp != null)
                    {
                        if (isTutorial)
                        {
                            DialogueManager.Instance.SayLines(
                               "Spammy Sammy",
                               new string[]
                               {
                                 "To fix this machine you will have to restore the energy from the systems",
                                 "Use your W to change to the next conduit, S to change to the previous conduit",
                                 "Balance their energy until they reach the red bars at the same time"},
                                 sammyFont, sammyPortrait
                               );

                        }
                        conduitComp.Interact();
                    }

                }

                // ---- SPACESHIP ----
                if (isSpaceShipObject)
                {
                    Debug.Log("Entering spaceship...");
                    player.transform.position = spaceshipSpawn.position;
                    sSammy.transform.position = new Vector3(spaceshipSpawn.position.x + 1f, 0.3000002f, spaceshipSpawn.position.z);
                    isInSpaceShip = true;
                }

                if (isMopObject)
                {
                    var mopComp = hit.collider.GetComponent<Mop>();
                    mopComp.PickUp();
                    Debug.Log("A limpiar guarrilla");
                }
            }
        }
        else
        {
            canInteract = false;
        }
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


