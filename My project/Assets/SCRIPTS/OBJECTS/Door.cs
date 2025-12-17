using UnityEngine;

/// <summary>
/// Controls a door that opens when a linked puzzle is completed.
/// Supports multiple possible trigger objects.
/// Rotation is relative to the door's starting orientation.
/// </summary>
public class Door : MonoBehaviour
{
    // ================================
    // Puzzle Triggers
    // ================================

    [Header("Possible triggers")]
    public WireComputer wireComputer;            // Opens door when wire puzzle is completed
    public BallBalanceObject ballBalanceObject;  // Opens door when ball puzzle is completed
    public ConduitObject conduitObject;          // Opens door when conduit puzzle is completed


    // ================================
    // Door Setup
    // ================================

    [Header("Door Settings")]
    public MeshCollider doorCollider;            // Collider blocking the player
    public GameObject doorPrefab;                // Visual door object to rotate


    // ================================
    // Rotation Settings
    // ================================

    [Header("Rotation")]
    public float openAngle = 90f;                // How much the door opens (degrees)
    public float rotationSpeed = 90f;            // Degrees per second


    // ================================
    // Internal State
    // ================================

    private bool hasOpened = false;               // Whether the door is currently opening
    private float startAngleY;                    // Door's starting Y rotation
    private float targetAngleY;                   // Final Y rotation


    void Start()
    {
        if (doorPrefab == null)
            return;

        // Cache starting rotation
        startAngleY = doorPrefab.transform.localEulerAngles.y;

        // Normalize to -180..180
        if (startAngleY > 180f)
            startAngleY -= 360f;

        // Target is relative to start
        targetAngleY = startAngleY + openAngle;
    }

    void Update()
    {
        // Check if any assigned trigger has been completed
        if (!hasOpened && IsTriggerCompleted())
        {
            hasOpened = true;

            // Disable collision once door starts opening
            if (doorCollider != null)
                doorCollider.enabled = false;
        }

        // Rotate door while opening
        if (hasOpened)
            RotateDoor();
    }

    /// <summary>
    /// Checks whether any assigned trigger object has been completed
    /// </summary>
    private bool IsTriggerCompleted()
    {
        if (wireComputer != null && wireComputer.completed)
            return true;

        if (ballBalanceObject != null && ballBalanceObject.completed)
            return true;

        if (conduitObject != null && conduitObject.completed)
            return true;

        return false;
    }

    /// <summary>
    /// Smoothly rotates the door toward its open position
    /// </summary>
    private void RotateDoor()
    {
        if (doorPrefab == null)
            return;

        float currentY = doorPrefab.transform.localEulerAngles.y;

        // Normalize to -180..180
        if (currentY > 180f)
            currentY -= 360f;

        // Move toward target angle
        float newY = Mathf.MoveTowards(
            currentY,
            targetAngleY,
            rotationSpeed * Time.deltaTime
        );

        doorPrefab.transform.localEulerAngles = new Vector3(
            doorPrefab.transform.localEulerAngles.x,
            newY,
            doorPrefab.transform.localEulerAngles.z
        );

        // Stop once target is reached
        if (Mathf.Approximately(newY, targetAngleY))
        {
            hasOpened = false;
        }
    }
}



