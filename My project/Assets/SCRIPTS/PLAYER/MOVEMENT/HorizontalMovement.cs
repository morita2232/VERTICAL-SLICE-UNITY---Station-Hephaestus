using UnityEngine;
using UnityEngine.InputSystem;

public class HorizontalMovement : MonoBehaviour
{
    [Header("References")]
    public Transform player;

    [Header("Attributes")]
    public float baseSensitivity = 1f;
    public bool canMove = true;

    InputSystem_Actions inputs;
    float rotationY;
    float sensitivity;

    [Tooltip("Ignores tiny input to prevent drift")]
    [Range(0f, 0.1f)]
    public float deadzone = 0.002f;


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
        rotationY = player.localEulerAngles.y;
        sensitivity = PlayerPrefs.GetFloat("MouseSensitivity", 0.5f);
    }

    void Update()
    {
        if (!canMove) return;

        float slider = PlayerPrefs.GetFloat("MouseSensitivity", 0.5f);
        float curved = slider * slider;
        float sensitivity = Mathf.Lerp(0.1f, 8f, curved);

        Vector2 look = inputs.Player.Look.ReadValue<Vector2>();
        Debug.Log(look);

        if (Mathf.Abs(look.x) < deadzone)
            return;


        float delta =
            look.x *
            sensitivity *
            baseSensitivity *
            Time.deltaTime;

        rotationY += delta;
        player.localEulerAngles = new Vector3(0f, rotationY, 0f);
    }


}

