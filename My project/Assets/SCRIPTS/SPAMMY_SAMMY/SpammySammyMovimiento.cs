using UnityEngine;

/// <summary>
/// Controls Spammy Sammy's follow behavior.
/// Sammy follows the player at a fixed distance, rotates to face them,
/// and updates walking animations based on movement.
/// </summary>
public class SpammySammyMovimiento : MonoBehaviour
{
    // ================================
    // References
    // ================================

    public Transform playerPos;            // Player transform to follow
    public Animator animator;              // Animator controlling Sammy


    // ================================
    // Follow Settings
    // ================================

    [Header("Follow Settings")]

    public float followDistance = 2f;      // Desired distance from player
    public float moveSpeed = 4f;            // Movement speed
    public float rotateSpeed = 10f;         // Rotation smoothing speed

    public InteractLocator interactLocator; // Used to stop following in spaceship


    // ================================
    // Internal State
    // ================================

    private Vector3 lastPosition;           // Used to detect movement for animation


    void Start()
    {
        lastPosition = transform.position;
    }

    void Update()
    {
        if (!playerPos) return;

        // Do not follow while player is inside the spaceship
        if (interactLocator.isInSpaceShip) return;

        // Flatten player position so Sammy stays on ground plane
        Vector3 playerFlat = new Vector3(
            playerPos.position.x,
            transform.position.y,
            playerPos.position.z
        );

        Vector3 toPlayer = playerFlat - transform.position;
        float dist = toPlayer.magnitude;

        // ================================
        // Follow Movement
        // ================================

        if (dist > followDistance)
        {
            Vector3 dir = toPlayer.normalized;
            Vector3 targetPos = playerFlat - dir * followDistance;

            transform.position = Vector3.MoveTowards(
                transform.position,
                targetPos,
                moveSpeed * Time.deltaTime
            );
        }

        // ================================
        // Rotation
        // ================================

        if (toPlayer.sqrMagnitude > 0.0001f)
        {
            Quaternion targetRot = Quaternion.LookRotation(toPlayer.normalized, Vector3.up);

            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRot,
                rotateSpeed * Time.deltaTime
            );
        }

        // ================================
        // Animation
        // ================================

        if (animator != null)
        {
            float movedSqr = (transform.position - lastPosition).sqrMagnitude;

            // Simple movement check: did Sammy move this frame?
            bool isWalking = movedSqr > 0.00001f;

            animator.SetBool("IsWalking", isWalking);
        }

        lastPosition = transform.position;
    }
}



