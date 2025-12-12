using UnityEngine;

public class Door : MonoBehaviour
{
    [Header("Possible triggers")]
    public WireComputer wireComputer;
    public BallBalanceObject ballBalanceObject;

    [Header("Door Settings")]
    public MeshCollider doorCollider;
    public GameObject doorPrefab;

    private bool hasOpened = false;
    private float targetAngle = 90f;
    private float rotationSpeed = 90f; // degrees per second

    void Update()
    {
        // Check if either assigned object is completed
        if (!hasOpened && IsTriggerCompleted())
        {
            hasOpened = true;
            if (doorCollider != null)
                doorCollider.enabled = false;
        }

        if (hasOpened)
            RotateDoor();
    }

    private bool IsTriggerCompleted()
    {
        // If WireComputer assigned, check it
        if (wireComputer != null && wireComputer.completed)
            return true;

        // If BallBalance assigned, check it
        if (ballBalanceObject != null && ballBalanceObject.completed)
            return true;

        // If nothing assigned or nothing completed, return false
        return false;
    }

    private void RotateDoor()
    {
        if (doorPrefab == null)
            return;

        float currentY = doorPrefab.transform.localEulerAngles.y;

        // Normalize angle for safety
        if (currentY > 180f)
            currentY -= 360f;

        // Stop if door reached target
        if (currentY >= targetAngle)
        {
            doorPrefab.transform.localEulerAngles =
                new Vector3(doorPrefab.transform.localEulerAngles.x, targetAngle, doorPrefab.transform.localEulerAngles.z);

            hasOpened = false; // stop rotating
            return;
        }

        // Rotate smoothly toward target
        float newY = Mathf.MoveTowards(currentY, targetAngle, rotationSpeed * Time.deltaTime);

        doorPrefab.transform.localEulerAngles =
            new Vector3(doorPrefab.transform.localEulerAngles.x, newY, doorPrefab.transform.localEulerAngles.z);
    }
}


