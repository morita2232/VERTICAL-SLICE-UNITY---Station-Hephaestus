using UnityEngine;

public class SpammySammyMovimiento : MonoBehaviour
{
    public Transform playerPos;
    public Animator animator;

    [Header("Follow Settings")]
    public float followDistance = 2f;
    public float moveSpeed = 4f;
    public float rotateSpeed = 10f;

    private Vector3 lastPosition;

    void Start()
    {
        lastPosition = transform.position;
    }

    void Update()
    {
        if (!playerPos) return;

        Vector3 playerFlat = new Vector3(playerPos.position.x, transform.position.y, playerPos.position.z);
        Vector3 toPlayer = playerFlat - transform.position;
        float dist = toPlayer.magnitude;

        // Follow logic (same as before, or your version)
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

        if (toPlayer.sqrMagnitude > 0.0001f)
        {
            Quaternion targetRot = Quaternion.LookRotation(toPlayer.normalized, Vector3.up);
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRot,
                rotateSpeed * Time.deltaTime
            );
        }

        // ---- ANIMATION: use a bool instead of float threshold ----
        if (animator != null)
        {
            float movedSqr = (transform.position - lastPosition).sqrMagnitude;
            bool isWalking = movedSqr > 0.00001f;  // tiny value, just "did he move?"

            animator.SetBool("IsWalking", isWalking);

            // Debug if you want:
            // Debug.Log("Sammy IsWalking = " + isWalking + " (movedSqr = " + movedSqr + ")");
        }

        lastPosition = transform.position;
    }
}


