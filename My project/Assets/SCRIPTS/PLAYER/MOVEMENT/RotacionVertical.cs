//using UnityEngine;
//using UnityEngine.InputSystem;

//public class RotacionVertical : MonoBehaviour
//{
//    [Header("Attributes")]
//    public float speed = 0.2f;
//    public float minAngle = -80f;
//    public float maxAngle = 80f;
//    public bool canRotate = true;

//    InputSystem_Actions inputs;
//    private float currentRotationX;

//    void Start()
//    {
//        inputs = new InputSystem_Actions();
//        inputs.Enable();
//    }

//    void Update()
//    {
//        if (!canRotate) return;

//        Vector2 look = inputs.Player.Look.ReadValue<Vector2>();

//        // tiny deadzone to avoid drift
//        if (Mathf.Abs(look.y) < 0.001f) look.y = 0f;

//        currentRotationX -= look.y * speed;
//        currentRotationX = Mathf.Clamp(currentRotationX, minAngle, maxAngle);

//        transform.localEulerAngles = new Vector3(currentRotationX, 0, 0);
//    }

//}

using UnityEngine;
using UnityEngine.InputSystem;

public class RotacionVertical : MonoBehaviour
{
    [Header("Attributes")]
    public float sensitivity = 0.1f;
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
    }

    void Update()
    {
        if (!canRotate) return;

        Vector2 look = inputs.Player.Look.ReadValue<Vector2>();

        // DEADZONE: if the stick/mouse is almost not moving, ignore it
        if (look.sqrMagnitude < deadzone * deadzone)
            return;

        // look.y > 0 usually means "move mouse up" look down
        float deltaY = look.y * sensitivity;

        currentRotationX -= deltaY;
        currentRotationX = Mathf.Clamp(currentRotationX, minAngle, maxAngle);

        transform.localEulerAngles = new Vector3(currentRotationX, 0f, 0f);
    }
}
