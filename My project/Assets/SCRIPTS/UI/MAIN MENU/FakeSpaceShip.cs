using UnityEngine;

/// <summary>
/// Simulates a decorative spaceship that moves inside a bounded area,
/// wraps around the edges, and continuously performs a barrel roll.
/// Used purely for visual background motion.
/// </summary>
public class FakeSpaceShip : MonoBehaviour
{
    // ================================
    // Movement Settings
    // ================================

    [Header("Movement")]

    public float speed = 1.5f;       // How fast the ship moves
    public BoxCollider area;          // Bounds the ship can move within


    // ================================
    // Barrel Roll Settings
    // ================================

    [Header("Barrel Roll")]

    public float rollSpeed = 90f;     // Degrees per second


    // ================================
    // Internal State
    // ================================

    private Vector3 direction;        // Current movement direction (XY plane)
    private float rollAngle;          // Accumulated barrel roll angle


    void Start()
    {
        // ================================
        // Random Spawn Position
        // ================================

        // Spawn inside the defined area (Z forced to 0)
        if (area != null)
        {
            Bounds b = area.bounds;

            float x = Random.Range(b.min.x, b.max.x);
            float y = Random.Range(b.min.y, b.max.y);

            transform.position = new Vector3(x, y, 0f);
        }
        else
        {
            // Fallback: force Z position to 0
            Vector3 p = transform.position;
            p.z = 0f;
            transform.position = p;
        }

        // ================================
        // Random Initial Direction
        // ================================

        Vector2 dir2 = Random.insideUnitCircle;

        // Safety: avoid near-zero vectors
        if (dir2.sqrMagnitude < 0.001f)
            dir2 = Vector2.right;

        direction = new Vector3(dir2.x, dir2.y, 0f).normalized;
    }

    void Update()
    {
        // ================================
        // 1) Movement
        // ================================

        transform.position += direction * speed * Time.deltaTime;

        // ================================
        // 2) Wrap Around Bounds
        // ================================

        Bounds b = area.bounds;
        Vector3 pos = transform.position;
        bool wrapped = false;

        if (pos.x > b.max.x) { pos.x = b.min.x; wrapped = true; }
        else if (pos.x < b.min.x) { pos.x = b.max.x; wrapped = true; }

        if (pos.y > b.max.y) { pos.y = b.min.y; wrapped = true; }
        else if (pos.y < b.min.y) { pos.y = b.max.y; wrapped = true; }

        pos.z = 0f;
        transform.position = pos;

        // When wrapping, choose a new random direction
        if (wrapped)
        {
            Vector2 dir2 = Random.insideUnitCircle;
            if (dir2.sqrMagnitude < 0.001f)
                dir2 = Vector2.right;

            direction = new Vector3(dir2.x, dir2.y, 0f).normalized;
        }

        // ================================
        // 3) Face Movement Direction
        // ================================

        // Assumes the ship model faces +X locally
        Quaternion faceDir = Quaternion.FromToRotation(Vector3.right, direction);

        // ================================
        // 4) Barrel Roll
        // ================================

        rollAngle += rollSpeed * Time.deltaTime;
        Quaternion roll = Quaternion.AngleAxis(rollAngle, direction);

        transform.rotation = roll * faceDir;
    }
}





