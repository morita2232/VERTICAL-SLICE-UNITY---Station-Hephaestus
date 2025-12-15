using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Handles vertical (X-axis) camera rotation using the Input System.
/// Supports sensitivity scaling, clamping, and a deadzone to prevent drift.
/// </summary>
public class RotacionVertical : MonoBehaviour
{
    // ================================
    // Rotation Settings
    // ================================

    [Header("Attributes")]

    public float baseSensitivity = 1f;   // Base multiplier for mouse sensitivity
    private float sensitivity;           // Cached sensitivity value

    public float minAngle = -80f;         // Minimum vertical angle
    public float maxAngle = 80f;          // Maximum vertical angle
    public bool canRotate = true;         // Whether vertical look is enabled


    // ================================
    // Input Filtering
    // ================================

    [Tooltip("Ignores tiny input to prevent drift (mouse/controller noise)")]
    [Range(0f, 0.1f)]
    public float deadzone = 0.002f;


    // ================================
    // Internal State
    // ================================

    InputSystem_Actions inputs;           // Generated input actions
    float currentRotationX;               // Current vertical rotation (degrees)


    void OnEnable()
    {
        if (inputs == null)
            inputs = new InputSystem_Actions();

        inputs.Enable();
    }

    void OnDisable()
    {
        if (inputs != null)
            inputs.Disable();
    }

    void Start()
    {
        // Sync with current rotation and normalize to -180..180 range
        currentRotationX = transform.localEulerAngles.x;
        if (currentRotationX > 180f)
            currentRotationX -= 360f;

        // Load initial sensitivity
        sensitivity = PlayerPrefs.GetFloat("MouseSensitivity", 1.0f);
    }

    void Update()
    {
        if (!canRotate) return;

        // Read sensitivity from PlayerPrefs (supports live adjustment)
        float slider = PlayerPrefs.GetFloat("MouseSensitivity", 0.5f);
        float curved = slider * slider;
        float sensitivity = Mathf.Lerp(0.1f, 8f, curved);

        // Read look input
        Vector2 look = inputs.Player.Look.ReadValue<Vector2>();

        // Ignore tiny input to prevent drift
        if (Mathf.Abs(look.y) < deadzone)
            return;

        // Calculate rotation delta
        float deltaY =
            look.y *
            sensitivity *
            baseSensitivity *
            Time.deltaTime;

        // Apply and clamp vertical rotation
        currentRotationX -= deltaY;
        currentRotationX = Mathf.Clamp(currentRotationX, minAngle, maxAngle);

        transform.localEulerAngles = new Vector3(currentRotationX, 0f, 0f);
    }
}
