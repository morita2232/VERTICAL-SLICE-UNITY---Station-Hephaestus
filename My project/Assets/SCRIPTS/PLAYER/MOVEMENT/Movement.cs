using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class Movement : MonoBehaviour
{
    [Header("Attributes")]
    public bool canMove = true;
    public float speed = 5f;

    InputSystem_Actions inputs;
    Rigidbody rb;
    Camera cam;

    void Awake()
    {
        inputs = new InputSystem_Actions();
        rb = GetComponent<Rigidbody>();
        cam = Camera.main;
        Cursor.lockState = CursorLockMode.Locked;
        rb.freezeRotation = true;
    }

    void OnEnable() => inputs.Enable();
    void OnDisable() => inputs.Disable();

    void FixedUpdate()
    {
        if (!canMove) return;

        Vector2 dir = inputs.Player.Move.ReadValue<Vector2>();

        // Flat camera directions
        Vector3 forward = new Vector3(cam.transform.forward.x, 0, cam.transform.forward.z).normalized;
        Vector3 right = new Vector3(cam.transform.right.x, 0, cam.transform.right.z).normalized;

        Vector3 move = (forward * dir.y + right * dir.x) * speed * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + move);

        //  Force the player to stay upright
        Vector3 rot = transform.rotation.eulerAngles;
        transform.rotation = Quaternion.Euler(0f, rot.y, rot.z);
    }
}





