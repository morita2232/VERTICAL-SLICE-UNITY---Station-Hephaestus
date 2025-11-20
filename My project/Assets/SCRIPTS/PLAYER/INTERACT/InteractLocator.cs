using UnityEditor;
using UnityEngine;

public class InteractLocator : MonoBehaviour
{
    public LayerMask interactMask;
    public GameObject miniGame;

    private Camera mainCamera;
    private InputSystem_Actions inputs;
    private bool clicked;
    public bool isInminigame = false;
    public bool canInteract = false;
    public GameObject player;
    public Transform spaceshipSpawn;
    public bool isInSpaceShip = false;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        mainCamera = Camera.main;
        inputs = new InputSystem_Actions();
        inputs.Enable();
    }

    // Update is called once per frame
    void Update()
    {
        clicked = inputs.Player.Interact.ReadValue<float>() > 0;

        Debug.Log("Clicked: " + clicked);

        Ray ray = mainCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 2f, interactMask))
        {
            canInteract = true;
            Debug.Log("Interacted with: " + hit.collider.gameObject.name);
            bool isMinigameObject = hit.collider.CompareTag("Miniggame");
            bool isSpaceShipObject = hit.collider.CompareTag("SpaceShip");

            if (clicked && isMinigameObject)
            {
                isInminigame = true;
                miniGame.SetActive(true);
            }
            
            if (clicked && isSpaceShipObject)
            {
                Debug.Log("Entering spaceship...");
                player.transform.position = spaceshipSpawn.position;
                isInSpaceShip = true;

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
        Ray ray = mainCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        Gizmos.DrawRay(ray.origin, ray.direction * 5f);
    }
}

