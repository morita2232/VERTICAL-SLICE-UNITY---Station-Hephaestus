using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Handles basic player movement using the Input System.
/// Movement is camera-relative and physics-based.
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class Movement : MonoBehaviour
{
    // ================================
    // Movement Settings
    // ================================

    [Header("Attributes")]

    public bool canMove = true;   // Whether the player can move
    public float speed = 5f;      // Movement speed


    // ================================
    // Internal References
    // ================================

    InputSystem_Actions inputs;   // Generated input actions
    Rigidbody rb;                 // Player rigidbody
    Camera cam;                   // Main camera reference


    void Awake()
    {
        // Initialize input system
        inputs = new InputSystem_Actions();

        // Cache components
        rb = GetComponent<Rigidbody>();
        cam = Camera.main;

        // Lock cursor for FPS-style control
        Cursor.lockState = CursorLockMode.Locked;

        // Prevent physics rotation
        rb.freezeRotation = true;
    }

    void OnEnable() => inputs.Enable();
    void OnDisable() => inputs.Disable();

    void FixedUpdate()
    {
        if (!canMove) return;

        // Read movement input
        Vector2 dir = inputs.Player.Move.ReadValue<Vector2>();

        // Camera-relative directions (flattened on Y axis)
        Vector3 forward = new Vector3(
            cam.transform.forward.x,
            0,
            cam.transform.forward.z
        ).normalized;

        Vector3 right = new Vector3(
            cam.transform.right.x,
            0,
            cam.transform.right.z
        ).normalized;

        // Calculate movement
        Vector3 move =
            (forward * dir.y + right * dir.x) *
            speed *
            Time.fixedDeltaTime;

        // Apply movement via Rigidbody
        rb.MovePosition(rb.position + move);

        // Force the player to stay upright
        Vector3 rot = transform.rotation.eulerAngles;
        transform.rotation = Quaternion.Euler(0f, rot.y, rot.z);
    }
}






