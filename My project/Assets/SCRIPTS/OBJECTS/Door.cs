using UnityEngine;

/// <summary>
/// Controls a door that opens when a linked puzzle is completed.
/// Supports multiple possible trigger objects.
/// </summary>
public class Door : MonoBehaviour
{
    // ================================
    // Puzzle Triggers
    // ================================

    [Header("Possible triggers")]

    public WireComputer wireComputer;          // Opens door when wire puzzle is completed
    public BallBalanceObject ballBalanceObject; // Opens door when ball puzzle is completed


    // ================================
    // Door Setup
    // ================================

    [Header("Door Settings")]

    public MeshCollider doorCollider;          // Collider blocking the player
    public GameObject doorPrefab;              // Visual door object to rotate


    // ================================
    // Internal State
    // ================================

    private bool hasOpened = false;             // Whether the door has started opening
    private float targetAngle = 90f;            // Final Y rotation angle
    private float rotationSpeed = 90f;          // Degrees per second


    void Update()
    {
        // Check if either assigned trigger has been completed
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
        // Wire puzzle trigger
        if (wireComputer != null && wireComputer.completed)
            return true;

        // Ball balance puzzle trigger
        if (ballBalanceObject != null && ballBalanceObject.completed)
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

        // Normalize angle for safety
        if (currentY > 180f)
            currentY -= 360f;

        // Stop rotating once target is reached
        if (currentY >= targetAngle)
        {
            doorPrefab.transform.localEulerAngles =
                new Vector3(
                    doorPrefab.transform.localEulerAngles.x,
                    targetAngle,
                    doorPrefab.transform.localEulerAngles.z
                );

            hasOpened = false; // Stop further rotation
            return;
        }

        // Rotate smoothly toward target angle
        float newY = Mathf.MoveTowards(
            currentY,
            targetAngle,
            rotationSpeed * Time.deltaTime
        );

        doorPrefab.transform.localEulerAngles =
            new Vector3(
                doorPrefab.transform.localEulerAngles.x,
                newY,
                doorPrefab.transform.localEulerAngles.z
            );
    }
}



