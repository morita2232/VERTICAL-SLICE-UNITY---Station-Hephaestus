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

            if (Physics.Raycast(ray, out hit, 5f, interactMask))
            {
                Debug.Log("Interacted with: " + hit.collider.gameObject.name);
                if (clicked)
                {
                    isInminigame = true;
                    miniGame.SetActive(true);
                }
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

