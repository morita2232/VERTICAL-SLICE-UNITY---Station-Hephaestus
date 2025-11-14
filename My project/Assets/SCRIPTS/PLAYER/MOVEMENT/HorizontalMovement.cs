using UnityEngine;
using UnityEngine.InputSystem;

public class HorizontalMovement : MonoBehaviour
{
    public GameObject player;

    public float minBaseY = -60f;
    public float maxBaseY = 60f;

    private float RotationY = 0f;

    private Transform TransformY;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        TransformY = player != null ? player.transform : null;
        RotationY = TransformY.localEulerAngles.y;
    }

    // Update is called once per frame
    void Update()
    {

        Vector2 mouseDelta = Mouse.current.delta.value;

        RotationY += mouseDelta.x;

        RotationY = Mathf.Clamp(RotationY, minBaseY, maxBaseY);

        Vector3 euler = TransformY.localEulerAngles;
        euler.y = RotationY;
        TransformY.localEulerAngles = euler;

    }
}
