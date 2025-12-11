using UnityEngine;
using TMPro; // only needed if you supply a TMP font to DialogueManager

public class TrashPickup : MonoBehaviour
{
    [Header("Pickup / Throw")]
    public float pickUpRange = 3f;
    public float throwForce = 10f; // tuned as impulse (try ~5-20)
    public Transform holdParent;   // usually your camera transform (optional; auto-find if null)
    public Vector3 holdLocalPosition = new Vector3(0f, -0.2f, 2f);
    public Vector3 holdLocalRotation = Vector3.zero;

    [Header("One-time pickup dialogue")]
    public TMP_FontAsset sammyFont;    // optional; pass to DialogueManager if used
    public Sprite sammyPortrait;      // optional
    public string[] pickupLines;

    // static so it triggers only once per play session across all trash instances
    private static bool hasShownPickupDialogue = false;

    private Camera cam;
    private Rigidbody rb;
    private bool isHeld = false;

    void Start()
    {
        cam = Camera.main;
        if (holdParent == null && cam != null)
            holdParent = cam.transform;

        rb = GetComponent<Rigidbody>();
        if (rb == null)
            Debug.LogWarning($"{name}: Rigidbody missing — add one for proper physics.");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (!isHeld)
                TryPickUp();
            else
                DropTrash();
        }

        if (Input.GetKeyDown(KeyCode.G) && isHeld)
        {
            ThrowTrash();
        }
    }

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

    void PickUp()
    {
        isHeld = true;

        // disable physics while held to avoid teleport tunneling
        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.isKinematic = true;
            rb.freezeRotation = true;
            // keep collision detection default (no need for Continuous while kinematic)
        }

        // parent to camera so it follows smoothly and collides correctly when dropped
        transform.SetParent(holdParent);
        transform.localPosition = holdLocalPosition;
        transform.localEulerAngles = holdLocalRotation;

        // Play the one-time pickup dialogue (only first trash picked)
        if (!hasShownPickupDialogue)
        {
            hasShownPickupDialogue = true;

            // Use your existing DialogueManager (no changes required)
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

    void DropTrash()
    {
        isHeld = false;

        // unparent and re-enable physics
        transform.SetParent(null);

        if (rb != null)
        {
            rb.isKinematic = false;
            rb.freezeRotation = false;
            rb.collisionDetectionMode = CollisionDetectionMode.Discrete;
        }
    }

    void ThrowTrash()
    {
        isHeld = false;

        // unparent and re-enable physics
        transform.SetParent(null);

        if (rb != null)
        {
            rb.isKinematic = false;
            rb.freezeRotation = false;

            // reduce tunneling when thrown quickly
            rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;

            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;

            rb.AddForce((holdParent != null ? holdParent.forward : cam.transform.forward) * throwForce, ForceMode.Impulse);
        }
    }
}


