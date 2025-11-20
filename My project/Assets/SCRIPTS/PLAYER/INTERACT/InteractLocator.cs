using UnityEngine;

public class InteractLocator : MonoBehaviour
{
    public LayerMask interactMask;
    public GameObject miniGame;   // you can ignore / remove this if WireMGManager handles UI

    private Camera mainCamera;
    private InputSystem_Actions inputs;
    private bool clicked;
    public bool isInminigame = false;
    public bool canInteract = false;
    public GameObject player;
    public Transform spaceshipSpawn;
    public bool isInSpaceShip = false;

    void Start()
    {
        mainCamera = Camera.main;
        inputs = new InputSystem_Actions();
        inputs.Enable();
    }

    void Update()
    {
        // your interact button (F or whatever you mapped)
        clicked = inputs.Player.Interact.ReadValue<float>() > 0;

        Debug.Log("Clicked: " + clicked);

        Ray ray = mainCamera.ScreenPointToRay(
            new Vector3(Screen.width / 2, Screen.height / 2, 0));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 2f, interactMask))
        {
            canInteract = true;
            Debug.Log("Interacted with: " + hit.collider.gameObject.name);

            bool isMinigameObject = hit.collider.CompareTag("Miniggame");
            bool isSpaceShipObject = hit.collider.CompareTag("SpaceShip");

            // Only DO things when the interact button is actually pressed
            if (clicked)
            {
                // ---- WIRE MINIGAME ----
                if (isMinigameObject)
                {
                    var wireComp = hit.collider.GetComponent<WireComputer>();
                    if (wireComp != null)
                    {
                        wireComp.Interact();   // this calls WireMGManager.OpenForComputer(...)
                    }
                }

                // ---- SPACESHIP ----
                if (isSpaceShipObject)
                {
                    Debug.Log("Entering spaceship...");
                    player.transform.position = spaceshipSpawn.position;
                    isInSpaceShip = true;
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


