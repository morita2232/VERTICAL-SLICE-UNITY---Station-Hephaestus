using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Handles horizontal (Y-axis) player rotation using the Input System.
/// Sensitivity is read from PlayerPrefs and supports a configurable deadzone.
/// </summary>
public class HorizontalMovement : MonoBehaviour
{
    // ================================
    // References
    // ================================

    [Header("References")]

    public Transform player;              // Player transform to rotate


    // ================================
    // Rotation Settings
    // ================================

    [Header("Attributes")]

    public float baseSensitivity = 1f;    // Base multiplier for mouse sensitivity
    public bool canMove = true;           // Whether horizontal look is allowed

    [Tooltip("Ignores tiny input to prevent drift")]
    [Range(0f, 0.1f)]
    public float deadzone = 0.002f;       // Minimum input threshold


    // ================================
    // Internal State
    // ================================

    InputSystem_Actions inputs;           // Generated input actions
    float rotationY;                      // Current Y rotation
    float sensitivity;                    // Cached sensitivity value


    void OnEnable()
    {
        if (inputs == null)
            inputs = new InputSystem_Actions();

        inputs.Enable();
    }

    void OnDisable()
    {
        inputs.Disable();
    }

    void Start()
    {
        // Initialize rotation from player
        rotationY = player.localEulerAngles.y;

        // Load initial sensitivity
        sensitivity = PlayerPrefs.GetFloat("MouseSensitivity", 0.5f);
    }

    void Update()
    {
        if (!canMove) return;

        // Read sensitivity from PlayerPrefs each frame (supports live changes)
        float slider = PlayerPrefs.GetFloat("MouseSensitivity", 0.5f);
        float curved = slider * slider;
        float sensitivity = Mathf.Lerp(0.1f, 8f, curved);

        // Read look input
        Vector2 look = inputs.Player.Look.ReadValue<Vector2>();
        Debug.Log(look);

        // Ignore tiny input to prevent drift
        if (Mathf.Abs(look.x) < deadzone)
            return;

        // Calculate rotation delta
        float delta =
            look.x *
            sensitivity *
            baseSensitivity *
            Time.deltaTime;

        rotationY += delta;

        // Apply rotation to player
        player.localEulerAngles = new Vector3(0f, rotationY, 0f);
    }
}


