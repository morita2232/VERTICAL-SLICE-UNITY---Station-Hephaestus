using UnityEngine;

public class MopCleaner : MonoBehaviour
{
    [Header("Cleaning Settings")]
    public float shrinkRate = 0.5f;         // units of scale per second
    public float minScaleToDestroy = 0.1f;  // destroy when smaller than this
    public float minMoveSpeed = 0.1f;       // how fast the mop must move to clean

    public CheckList checklist; // assign Checklist in inspector
    private Vector3 lastPosition;
    private float currentSpeed;

    void Start()
    {
        lastPosition = transform.position;
    }

    void Update()
    {
        // Calculate how fast the mop is moving
        Vector3 delta = transform.position - lastPosition;
        currentSpeed = delta.magnitude / Time.deltaTime;
        lastPosition = transform.position;
    }

    void OnTriggerStay(Collider other)
    {
        // Must be touching dirt
        if (!other.CompareTag("Dirt"))
            return;

        // Mop must be moving
        if (currentSpeed < minMoveSpeed)
            return;

        // Shrink the dirt
        Transform dirt = other.transform;
        Vector3 scale = dirt.localScale;

        float shrinkAmount = shrinkRate * Time.deltaTime;
        scale -= Vector3.one * shrinkAmount;

        // Clamp so it doesn't go negative
        float maxAxis = Mathf.Max(scale.x, Mathf.Max(scale.y, scale.z));
        if (maxAxis <= minScaleToDestroy)
        {
            checklist.allDirt--;
            checklist.remaining--;
            Destroy(dirt.gameObject);
            return;
        }

        dirt.localScale = scale;
    }
}
