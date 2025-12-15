using UnityEngine;
using TMPro; // only needed if you supply a TMP font to DialogueManager

/// <summary>
/// Allows the player to pick up, drop, and throw trash objects.
/// Also plays a one-time dialogue the first time trash is picked up.
/// </summary>
public class TrashPickup : MonoBehaviour
{
    // ================================
    // Pickup & Throw Settings
    // ================================

    [Header("Pickup / Throw")]

    public float pickUpRange = 3f;          // Max distance to pick up trash
    public float throwForce = 10f;           // Force applied when throwing (impulse)
    public Transform holdParent;             // Usually the camera transform
    public Vector3 holdLocalPosition = new Vector3(0f, -0.2f, 2f); // Held position
    public Vector3 holdLocalRotation = Vector3.zero;              // Held rotation


    // ================================
    // One-Time Pickup Dialogue
    // ================================

    [Header("One-time pickup dialogue")]

    public TMP_FontAsset sammyFont;          // Optional dialogue font
    public Sprite sammyPortrait;             // Optional dialogue portrait
    public string[] pickupLines;              // Dialogue lines played once


    // ================================
    // Internal State
    // ================================

    // Static so dialogue triggers only once across all trash objects
    private static bool hasShownPickupDialogue = false;

    private Camera cam;                       // Cached main camera
    private Rigidbody rb;                     // Rigidbody on this trash object
    private bool isHeld = false;              // Whether the trash is currently held


    void Start()
    {
        // Cache camera
        cam = Camera.main;

        // Auto-assign hold parent if not set
        if (holdParent == null && cam != null)
            holdParent = cam.transform;

        // Cache rigidbody
        rb = GetComponent<Rigidbody>();
        if (rb == null)
            Debug.LogWarning($"{name}: Rigidbody missing — add one for proper physics.");
    }

    void Update()
    {
        // Pick up / drop
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (!isHeld)
                TryPickUp();
            else
                DropTrash();
        }

        // Throw while holding
        if (Input.GetKeyDown(KeyCode.G) && isHeld)
        {
            ThrowTrash();
        }
    }

    /// <summary>
    /// Attempts to pick up the trash using a camera raycast
    /// </summary>
    void TryPickUp()
    {
        if (cam == null) return;

        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, pickUpRange))
        {
            if (hit.collider.gameObject == gameObject)
            {
                PickUp();
            }
        }
    }

    /// <summary>
    /// Picks up and holds the trash object
    /// </summary>
    void PickUp()
    {
        isHeld = true;

        // Disable physics while held
        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.isKinematic = true;
            rb.freezeRotation = true;
        }

        // Parent to camera for smooth movement
        transform.SetParent(holdParent);
        transform.localPosition = holdLocalPosition;
        transform.localEulerAngles = holdLocalRotation;

        // Play pickup dialogue only once per session
        if (!hasShownPickupDialogue)
        {
            hasShownPickupDialogue = true;

            if (DialogueManager.Instance != null)
            {
                DialogueManager.Instance.SayLines(
                    "Spammy Sammy",
                    pickupLines,
                    sammyFont,
                    sammyPortrait
                );
            }
            else
            {
                Debug.Log("Pickup dialogue: " + string.Join(" ", pickupLines));
            }
        }
    }

    /// <summary>
    /// Drops the trash object without force
    /// </summary>
    void DropTrash()
    {
        isHeld = false;

        // Unparent and re-enable physics
        transform.SetParent(null);

        if (rb != null)
        {
            rb.isKinematic = false;
            rb.freezeRotation = false;
            rb.collisionDetectionMode = CollisionDetectionMode.Discrete;
        }
    }

    /// <summary>
    /// Throws the trash object forward
    /// </summary>
    void ThrowTrash()
    {
        isHeld = false;

        // Unparent and re-enable physics
        transform.SetParent(null);

        if (rb != null)
        {
            rb.isKinematic = false;
            rb.freezeRotation = false;

            // Reduce tunneling when thrown
            rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;

            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;

            rb.AddForce(
                (holdParent != null ? holdParent.forward : cam.transform.forward) * throwForce,
                ForceMode.Impulse
            );
        }
    }
}



