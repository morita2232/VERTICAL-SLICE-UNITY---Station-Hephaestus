using UnityEngine;
using UnityEngine.InputSystem;

public class RotacionVertical : MonoBehaviour
{
    InputSystem_Actions inputs;
    public float speed = 0.2f;
    public float minAngle = -80f;
    public float maxAngle = 80f;

    public bool canRotate = true;

    private float currentRotationX;

    void Start()
    {
        inputs = new InputSystem_Actions();
        inputs.Enable();
    }

    void Update()
    {
        if (canRotate)
        {

        Vector2 look = inputs.Player.Look.ReadValue<Vector2>();

        // Invertir eje Y para comportamiento natural de cámara
        currentRotationX -= look.y * speed;
        currentRotationX = Mathf.Clamp(currentRotationX, minAngle, maxAngle);

        transform.localEulerAngles = new Vector3(currentRotationX, 0, 0);
        }
    }
}
