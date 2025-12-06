using UnityEngine;

public class FakeSpaceShip : MonoBehaviour
{
    [Header("Movement")]
    public float speed = 1.5f;          // how fast it moves
    public BoxCollider area;            // drag your Limits collider here

    [Header("Barrel Roll")]
    public float rollSpeed = 90f;       // degrees per second

    private Vector3 direction;          // current movement direction (XY plane)
    private float rollAngle;            // current barrel roll angle

    void Start()
    {
        // ----- Random spawn position (z = 0) inside the area -----
        if (area != null)
        {
            Bounds b = area.bounds;

            float x = Random.Range(b.min.x, b.max.x);
            float y = Random.Range(b.min.y, b.max.y);

            transform.position = new Vector3(x, y, 0f);
        }
        else
        {
            Vector3 p = transform.position;
            p.z = 0f;
            transform.position = p;
        }

        // ----- Random 2D direction -----
        Vector2 dir2 = Random.insideUnitCircle;
        if (dir2.sqrMagnitude < 0.001f)
            dir2 = Vector2.right;

        direction = new Vector3(dir2.x, dir2.y, 0f).normalized;
    }

    void Update()
    {
        // ----- 1) Move -----
        transform.position += direction * speed * Time.deltaTime;

        // ----- 2) Wrap around the BoxCollider -----
        Bounds b = area.bounds;
        Vector3 pos = transform.position;
        bool wrapped = false;

        if (pos.x > b.max.x) { pos.x = b.min.x; wrapped = true; }
        else if (pos.x < b.min.x) { pos.x = b.max.x; wrapped = true; }

        if (pos.y > b.max.y) { pos.y = b.min.y; wrapped = true; }
        else if (pos.y < b.min.y) { pos.y = b.max.y; wrapped = true; }

        pos.z = 0f;
        transform.position = pos;

        // Optional: when we wrap, pick a new random direction
        if (wrapped)
        {
            Vector2 dir2 = Random.insideUnitCircle;
            if (dir2.sqrMagnitude < 0.001f)
                dir2 = Vector2.right;

            direction = new Vector3(dir2.x, dir2.y, 0f).normalized;
        }

        // ----- 3) Rotate so nose points along movement -----
        // Assumes the ship’s nose points along local +X in the model.
        Quaternion faceDir = Quaternion.FromToRotation(Vector3.right, direction);

        // ----- 4) Barrel roll around the nose direction -----
        rollAngle += rollSpeed * Time.deltaTime;
        Quaternion roll = Quaternion.AngleAxis(rollAngle, direction);

        transform.rotation = roll * faceDir;
    }
}




