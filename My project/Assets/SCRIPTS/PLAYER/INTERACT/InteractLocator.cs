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

    private Camera mainCamera;
    private InputSystem_Actions inputs;
    private bool clicked;

    void Start()
    {
        mainCamera = Camera.main;
        inputs = new InputSystem_Actions();
        inputs.Enable();
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

                    if (wireComp != null)
                    {
                        wireComp.Interact();   // this calls WireMGManager.OpenForComputer(...)
                    }

                    if(balanceComp != null)
                    {
                        balanceComp.Interact();
                    }

                    if(conduitComp != null)
                    {
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


