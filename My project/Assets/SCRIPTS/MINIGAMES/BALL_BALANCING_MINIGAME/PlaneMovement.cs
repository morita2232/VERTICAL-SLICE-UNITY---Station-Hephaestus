using UnityEngine;
using UnityEngine.Windows;

public class PlaneMovement : MonoBehaviour
{
    public bool movePlane = false;
    InputSystem_Actions inputs;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        inputs = new InputSystem_Actions();
        inputs.Enable();

    }

    // Update is called once per frame
    void Update()
    {
        if (movePlane)
        {

            Vector2 dir = inputs.Player.Move.ReadValue<Vector2>();

            transform.rotation *= Quaternion.Euler(
                dir.y * 50f * Time.deltaTime,
                0,
                -dir.x * 50f * Time.deltaTime);

        } 
    }
}
