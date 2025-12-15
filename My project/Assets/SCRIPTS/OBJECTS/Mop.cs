using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Handles picking up, holding, and using the mop:
/// - Attaches mop to the player's hand
/// - Toggles cleaning mode
/// - Rotates mop based on mouse movement
/// - Disables player movement while mopping
/// </summary>
public class Mop : MonoBehaviour
{
    // ================================
    // Setup & Transforms
    // ================================

    [Header("Setup")]

    public Transform holdPos;      // MopHoldPos on the player (hand)
    public Transform gripPoint;    // Empty transform at top of the handle


    // ================================
    // Player & Dialogue References
    // ================================

    [Header("References")]

    public Movement playerMovement;
    public HorizontalMovement horizontalLook;
    public RotacionVertical verticalLook;

    public TMP_FontAsset sammyFont;
    public Sprite sammyPortrait;


    // ================================
    // Mop Controls
    // ================================

    [Header("Mop Control")]

    public KeyCode mopModeKey = KeyCode.Mouse1; // Toggle cleaning mode
    public KeyCode dropKey = KeyCode.G;         // Drop mop
    public float rotateSpeed = 0.2f;            // Mouse sensitivity
    public float minPitch = -70f;                // Minimum mop pitch
    public float maxPitch = 70f;                 // Maximum mop pitch


    // ================================
    // Runtime State
    // ================================

    [HideInInspector]
    public bool holding;                         // Whether the mop is currently held

    bool isMopping = false;                      // Whether cleaning mode is active

    bool prevMove;                               // Cached player movement state
    bool prevLookH;                              // Cached horizontal look state
    bool prevLookV;                              // Cached vertical look state


    // ================================
    // Mop Rotation State
    // ================================

    Quaternion baseHandRot;                      // Hand rotation before mopping
    float mopPitch;                              // Current pitch
    float mopYaw;                                // Current yaw


    // ================================
    // One-Time Pickup Logic
    // ================================

    public static bool firstTimePickedUp = false;
    public static System.Action OnFirstPickup;


    void Update()
    {
        if (!holding) return;

        // Drop mop
        if (Input.GetKeyDown(dropKey))
        {
            ExitMopMode();
            Drop();
            return;
        }

        // Toggle cleaning mode
        if (Input.GetKeyDown(mopModeKey))
        {
            if (!isMopping)
                EnterMopMode();
            else
                ExitMopMode();
        }

        // Rotate mop while cleaning
        if (isMopping)
            UpdateMopRotation();
    }

    /// <summary>
    /// Called by the interaction system when picking up the mop
    /// </summary>
    public void PickUp()
    {
        holding = true;

        // Parent to player's hand
        transform.SetParent(holdPos);

        // Match hand rotation
        transform.rotation = holdPos.rotation;

        // Align grip point with hand position
        Vector3 worldOffset = transform.position - gripPoint.position;
        transform.position = holdPos.position + worldOffset;

        // Play first-time pickup dialogue
        if (!firstTimePickedUp)
        {
            firstTimePickedUp = true;

            DialogueManager.Instance.SayLines(
                "Spammy Sammy",
                new string[]
                {
                    "CONGRATS!!! You did it! I always knew you could.",
                    "To activate cleaning mode press RIGHT CLICK and move your MOUSE around.",
                    "Also if you want to leave the mop press G to drop it anywhere you like."
                },
                sammyFont,
                sammyPortrait
            );
        }

        // Initialize rotation state
        baseHandRot = holdPos.localRotation;
        mopPitch = 0f;
        mopYaw = 0f;
    }

    /// <summary>
    /// Drops the mop back into the world
    /// </summary>
    public void Drop()
    {
        holding = false;
        transform.SetParent(null);

        transform.rotation = Quaternion.identity;
        transform.position = new Vector3(
            transform.position.x,
            0.3000245f,
            transform.position.z
        );
    }

    /// <summary>
    /// Enters cleaning mode and locks player controls
    /// </summary>
    void EnterMopMode()
    {
        if (isMopping) return;
        isMopping = true;

        // Save & disable controls
        if (playerMovement != null)
        {
            prevMove = playerMovement.canMove;
            playerMovement.canMove = false;
        }

        if (horizontalLook != null)
        {
            prevLookH = horizontalLook.canMove;
            horizontalLook.canMove = false;
        }

        if (verticalLook != null)
        {
            prevLookV = verticalLook.canRotate;
            verticalLook.canRotate = false;
        }

        // Cache hand rotation
        baseHandRot = holdPos.localRotation;
        mopPitch = 0f;
        mopYaw = 0f;
    }

    /// <summary>
    /// Exits cleaning mode and restores player controls
    /// </summary>
    void ExitMopMode()
    {
        if (!isMopping) return;
        isMopping = false;

        // Restore controls
        if (playerMovement != null)
            playerMovement.canMove = prevMove;

        if (horizontalLook != null)
            horizontalLook.canMove = prevLookH;

        if (verticalLook != null)
            verticalLook.canRotate = prevLookV;

        // Restore original hand rotation
        holdPos.localRotation = baseHandRot;
    }

    /// <summary>
    /// Updates mop rotation based on mouse movement
    /// </summary>
    void UpdateMopRotation()
    {
        if (Mouse.current == null) return;

        Vector2 delta = Mouse.current.delta.ReadValue();

        mopYaw += delta.x * rotateSpeed;
        mopPitch -= delta.y * rotateSpeed;
        mopPitch = Mathf.Clamp(mopPitch, minPitch, maxPitch);

        // Rotate around the hand pivot
        holdPos.localRotation =
            baseHandRot * Quaternion.Euler(mopPitch, mopYaw, 0f);
    }
}








