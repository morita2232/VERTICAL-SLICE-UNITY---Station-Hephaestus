using UnityEngine;
using UnityEngine.InputSystem;

public class PlaneMovement : MonoBehaviour
{
    public bool movePlane = false;

    private InputSystem_Actions inputs;

    public float maxTiltX = 25f;      // how far from baseX it tilts
    public float smooth = 8f;         // how fast it moves toward target

    // current rotation values
    private float currentX;
    private float currentY;
    private float currentZ;

    // base X rotation (you said it's -90)
    public float baseX = -90f;

    void Start()
    {
        inputs = new InputSystem_Actions();
        inputs.Enable();

        // start from whatever the object currently has
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
        if (!movePlane) return;

        Keyboard kb = Keyboard.current;
        bool a = kb.aKey.isPressed;
        bool d = kb.dKey.isPressed;
        bool w = kb.wKey.isPressed;
        bool s = kb.sKey.isPressed;

        // Default target = rest rotation
        float targetX = baseX;
        float targetY = 0f;
        float targetZ = 0f;

        // --- Apply your exact rules ---

        // D -> x+ & y = 90 & z = -90
        if (d && !a && !w && !s)
        {
            targetX = baseX + maxTiltX;
            targetY = 90f;
            targetZ = -90f;
        }
        // A -> x- & y = -90 & z = 90
        else if (a && !d && !w && !s)
        {
            targetX = baseX + maxTiltX;
            targetY = -90f;
            targetZ = 90f;
        }
        // W -> x+ & y = 0 & z = 0
        else if (w && !s && !a && !d)
        {
            targetX = baseX + maxTiltX;
            targetY = 0f;
            targetZ = 0f;
        }
        // S -> x- & y = 0 & z = 0
        else if (s && !w && !a && !d)
        {
            targetX = baseX - maxTiltX;
            targetY = 0f;
            targetZ = 0f;
        }
        // If multiple keys are pressed, you can decide priority here if you want.

        // Smoothly move each axis toward target
        float t = smooth * Time.deltaTime;
        currentX = Mathf.LerpAngle(currentX, targetX, t);
        currentY = Mathf.LerpAngle(currentY, targetY, t);
        currentZ = Mathf.LerpAngle(currentZ, targetZ, t);

        transform.rotation = Quaternion.Euler(currentX, currentY, currentZ);
    }
}







