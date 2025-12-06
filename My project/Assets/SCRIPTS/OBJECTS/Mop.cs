//using UnityEngine;

//public class Mop : MonoBehaviour
//{
//    public Transform holdPos; // assign MopHoldPos in inspector
//    public bool holding;
//    private void Update()
//    {
//        if (holding)
//        {
//            if (Input.GetKeyDown(KeyCode.G))
//            {
//                Drop();
//            }
//        }
//    }
//    public void PickUp()
//    {
//        holding = true;

//        // Make mop follow the pivot
//        transform.SetParent(holdPos);

//        // Position at pivot
//        transform.localPosition = Vector3.zero;

//        // Rotate relative to pivot so it’s sideways in hand
//        // Tweak these values until it looks right in game view
//        transform.localRotation = Quaternion.Euler(0f, 90f, 0f);

//    }

//    public void Drop()
//    {
//        holding = false;

//        transform.rotation = Quaternion.Euler(0f, 0f, 0f);

//        transform.position = new Vector3(
//            transform.position.x,
//            0.3000245f,
//            transform.position.z
//        );


//        transform.SetParent(null);
//    }

//}

using UnityEngine;
using UnityEngine.InputSystem;

public class Mop : MonoBehaviour
{
    [Header("Setup")]
    public Transform holdPos;     // MopHoldPos on the player (hand)
    public Transform gripPoint;   // empty at top of handle

    [Header("References")]
    public Movement playerMovement;
    public HorizontalMovement horizontalLook;
    public RotacionVertical verticalLook;

    [Header("Mop Control")]
    public KeyCode mopModeKey = KeyCode.Mouse1; // cleaning mode
    public KeyCode dropKey = KeyCode.G;
    public float rotateSpeed = 0.2f;
    public float minPitch = -70f;
    public float maxPitch = 70f;

    [HideInInspector]
    public bool holding;

    bool isMopping = false;

    bool prevMove;
    bool prevLookH;
    bool prevLookV;

    // Hand rotation while cleaning
    Quaternion baseHandRot;
    float mopPitch;
    float mopYaw;

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
            if (!isMopping) EnterMopMode();
            else ExitMopMode();
        }

        if (isMopping)
            UpdateMopRotation();
    }

    // Called by your interact script when pressing E near the mop
    public void PickUp()
    {
        holding = true;

        // Parent to hand
        transform.SetParent(holdPos);

        // 1) Match hand rotation
        transform.rotation = holdPos.rotation;

        // 2) Move mop so GripPoint is exactly at hand position
        Vector3 worldOffset = transform.position - gripPoint.position;
        transform.position = holdPos.position + worldOffset;
        // At this point: GripPoint.position == holdPos.position

        // Initialize pitch/yaw relative to current hand rotation
        baseHandRot = holdPos.localRotation;
        mopPitch = 0f;
        mopYaw = 0f;
    }

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

        // Save the hand's current local rotation (0,90,0 from your screenshot)
        baseHandRot = holdPos.localRotation;
        mopPitch = 0f;
        mopYaw = 0f;
    }

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

        // Restore original hand rotation (so your MopHoldPos values stay)
        holdPos.localRotation = baseHandRot;
    }

    void UpdateMopRotation()
    {
        if (Mouse.current == null) return;

        Vector2 delta = Mouse.current.delta.ReadValue();

        mopYaw += delta.x * rotateSpeed;
        mopPitch -= delta.y * rotateSpeed;
        mopPitch = Mathf.Clamp(mopPitch, minPitch, maxPitch);

        // Rotate around the hand pivot (MopHoldPos)
        holdPos.localRotation = baseHandRot * Quaternion.Euler(mopPitch, mopYaw, 0f);
        // Because the mop is a child and we aligned GripPoint to the hand,
        // the visible pivot is now the top of the handle, not the sponge.
    }
}







