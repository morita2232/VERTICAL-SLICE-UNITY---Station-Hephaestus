using UnityEngine;

public class FakeSpaceShip : MonoBehaviour
{
    public float speed = 5f;
    public float rotationSpeed = 45f;

    private Vector3 direction;
    private float zRotationDirection;

    void Start()
    {
        float horizontal = Random.Range(-1f, 1f);
        float vertical = Random.Range(-1f, 1f);

        direction = new Vector3(horizontal, vertical, 0f).normalized;

        //zRotationDirection = Random.Range(-1f, 1f);
        //if (zRotationDirection > 0f) zRotationDirection = 1f;
        //else zRotationDirection = -1f;
    }

    void Update()
    {
        transform.position += direction * speed * Time.deltaTime;

        //if (direction.sqrMagnitude > 0.001f)
        //{
        //    float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        //    // This is the key fix — adjust angle offset to match your ship's model orientation
        //    angle -= 90f;

        //    transform.rotation = Quaternion.Euler(0, 0, angle);
        //}

        //transform.Rotate(0f, 0f, zRotationDirection * rotationSpeed * Time.deltaTime, Space.Self);
    }
}


