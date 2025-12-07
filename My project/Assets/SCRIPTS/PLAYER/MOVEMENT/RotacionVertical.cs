using UnityEngine;
using UnityEngine.InputSystem;

public class RotacionVertical : MonoBehaviour
{
    [Header("Attributes")]
    public float baseSensitivity = 1f;
    private float sensitivity;
    public float minAngle = -80f;
    public float maxAngle = 80f;
    public bool canRotate = true;

    [Tooltip("Ignores tiny input to prevent drift (mouse/controller noise)")]
    [Range(0f, 0.1f)]
    public float deadzone = 0.002f;

    InputSystem_Actions inputs;
    float currentRotationX;

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
        // sync with current rotation, convert to -180..180
        currentRotationX = transform.localEulerAngles.x;
        if (currentRotationX > 180f)
            currentRotationX -= 360f;

        sensitivity = PlayerPrefs.GetFloat("MouseSensitivity", 1.0f);
    }

    void Update()
    {
        if (!canRotate) return;

        // Always read latest sensitivity value
        float sensitivity = PlayerPrefs.GetFloat("MouseSensitivity", 5.0f);

        Debug.Log("Loaded MouseSensitivity from PlayerPrefs (Rotacion Vertical): " + sensitivity);

        Vector2 look = inputs.Player.Look.ReadValue<Vector2>();

        if (Mathf.Abs(look.y) < deadzone)
            return;

        float deltaY = look.y * sensitivity * baseSensitivity;

        currentRotationX -= deltaY;
        currentRotationX = Mathf.Clamp(currentRotationX, minAngle, maxAngle);

        transform.localEulerAngles = new Vector3(currentRotationX, 0f, 0f);
    }



}
