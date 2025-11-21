using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    [Header("References")]
    InputSystem_Actions inputs;
    public Transform pivot;

    [Header("Attributes")]
    public float sensitivity;
    public float distance = 5;
    public float minY;
    void Start()
    {
        inputs = new InputSystem_Actions();
        inputs.Enable();
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 look = inputs.Player.Move.ReadValue<Vector2>();
        pivot.localEulerAngles += new Vector3(look.y, look.x, 0) * sensitivity * Time.deltaTime;

        RaycastHit hit;
        if (Physics.Raycast(pivot.position, -pivot.forward, out hit, distance))
        {
            transform.localPosition = new Vector3(0, minY, -hit.distance);
        }
        else
        {
            transform.localPosition = new Vector3(0, minY, -distance);
        }
    }
}
