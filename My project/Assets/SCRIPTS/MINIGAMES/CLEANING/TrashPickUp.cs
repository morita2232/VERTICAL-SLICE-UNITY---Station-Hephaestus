using UnityEngine;

public class TrashPickup : MonoBehaviour
{
    public float pickUpRange = 3f;
    public float throwForce = 500f;

    private Camera cam;
    private Rigidbody rb;
    private bool isHeld = false;

    void Start()
    {
        cam = Camera.main;
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Left click = pick up / drop
        {
            if (!isHeld)
                TryPickUp();
            else
                DropTrash();
        }

        if (isHeld)
        {
            transform.position = cam.transform.position + cam.transform.forward * 2f; // follow camera
        }

        if (Input.GetMouseButtonDown(1) && isHeld) // Right click = throw
        {
            ThrowTrash();
        }
    }

    void TryPickUp()
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, pickUpRange))
        {
            if (hit.collider.gameObject == gameObject)
            {
                isHeld = true;
                rb.useGravity = false;
                rb.linearVelocity = Vector3.zero;
                rb.freezeRotation = true;
            }
        }
    }

    void DropTrash()
    {
        isHeld = false;
        rb.useGravity = true;
        rb.freezeRotation = false;
    }

    void ThrowTrash()
    {
        DropTrash();
        rb.AddForce(cam.transform.forward * throwForce);
    }
}

