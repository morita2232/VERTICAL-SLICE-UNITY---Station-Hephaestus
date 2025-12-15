using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Controls plane tilting during the ball-balancing minigame.
/// Reads keyboard input and smoothly rotates the plane
/// around a fixed base rotation.
/// </summary>
public class PlaneMovement : MonoBehaviour
{
    // ================================
    // Minigame State
    // ================================

    public bool movePlane = false;    // Enables or disables plane movement


    // ================================
    // Input System
    // ================================

    private InputSystem_Actions inputs; // Generated Input System actions


    // ================================
    // Rotation Settings (Inspector)
    // ================================

    [Header("Rotation Settings")]

    public float baseX = -90f;        // Base X rotation of the plane
    public float maxTiltX = 25f;      // How far from baseX the plane can tilt
    public float smooth = 8f;         // How fast the plane moves toward target rotation


    // ================================
    // Current Rotation State
    // ================================

    private float currentX;           // Current X rotation
    private float currentY;           // Current Y rotation
    private float currentZ;           // Current Z rotation


    void Start()
    {
        // Initialize input system
        inputs = new InputSystem_Actions();
        inputs.Enable();

        // Cache starting rotation
        Vector3 e = transform.eulerAngles;
        currentX = e.x;
        currentY = e.y;
        currentZ = e.z;
    }

    void OnDisable()
    {
        if (inputs != null)
            inputs.Disable();
    }

    void Update()
    {
        // Do nothing if minigame is inactive
        if (!movePlane) return;

        // Read keyboard input
        Keyboard kb = Keyboard.current;
        bool a = kb.aKey.isPressed;
        bool d = kb.dKey.isPressed;
        bool w = kb.wKey.isPressed;
        bool s = kb.sKey.isPressed;

        // Default target rotation (resting state)
        float targetX = baseX;
        float targetY = 0f;
        float targetZ = 0f;

        // ================================
        // Rotation Rules
        // ================================

        // D key: tilt right
        if (d && !a && !w && !s)
        {
            targetX = baseX + maxTiltX;
            targetY = 90f;
            targetZ = -90f;
        }
        // A key: tilt left
        else if (a && !d && !w && !s)
        {
            targetX = baseX + maxTiltX;
            targetY = -90f;
            targetZ = 90f;
        }
        // W key: tilt forward
        else if (w && !s && !a && !d)
        {
            targetX = baseX + maxTiltX;
            targetY = 0f;
            targetZ = 0f;
        }
        // S key: tilt backward
        else if (s && !w && !a && !d)
        {
            targetX = baseX - maxTiltX;
            targetY = 0f;
            targetZ = 0f;
        }
        // Multiple keys pressed  resting state (or add priority later)

        // ================================
        // Smooth Rotation
        // ================================

        float t = smooth * Time.deltaTime;

        currentX = Mathf.LerpAngle(currentX, targetX, t);
        currentY = Mathf.LerpAngle(currentY, targetY, t);
        currentZ = Mathf.LerpAngle(currentZ, targetZ, t);

        transform.rotation = Quaternion.Euler(currentX, currentY, currentZ);
    }
}








