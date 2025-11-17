using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    InputSystem_Actions inputs;
    public float speed;
    Camera cam;

    public bool canMove = true;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        inputs = new InputSystem_Actions();
        inputs.Enable();
        cam = Camera.main;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (canMove)
        {

        Vector2 dir = inputs.Player.Move.ReadValue<Vector2>();

                transform.position +=
                    Vector3.ProjectOnPlane(cam.transform.forward, Vector3.up).normalized
                    * dir.y * speed * Time.fixedDeltaTime;

        transform.position +=
            Vector3.ProjectOnPlane(cam.transform.right, Vector3.up).normalized
            * dir.x * speed * Time.fixedDeltaTime;
        }
    }
}
