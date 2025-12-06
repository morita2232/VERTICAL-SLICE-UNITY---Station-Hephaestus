using UnityEngine;

public class MopCleaner : MonoBehaviour
{
    [Header("Cleaning Settings")]
    public float shrinkRate = 0.5f;         // units of scale per second (for dirt)
    public float minScaleToDestroy = 0.1f;  // destroy when smaller than this
    public float minMoveSpeed = 0.1f;       // how fast the mop must move to clean

    public float surfaceCleanRate = 0.2f;   // how fast Shader “Slide” increases

    public CheckList checklist; // assign Checklist in inspector
    private Vector3 lastPosition;
    private float currentSpeed;

    void Start()
    {
        lastPosition = transform.position;
    }

    void Update()
    {
        Vector3 delta = transform.position - lastPosition;
        currentSpeed = delta.magnitude / Time.deltaTime;
        lastPosition = transform.position;
    }

    void OnTriggerStay(Collider other)
    {
        // Mop must be moving
        if (currentSpeed < minMoveSpeed)
            return;

        // ----- 1) Old: shrink DIRT objects -----
        if (other.CompareTag("Dirt"))
        {
            Transform dirt = other.transform;
            Vector3 scale = dirt.localScale;

            float shrinkAmount = shrinkRate * Time.deltaTime;
            scale -= Vector3.one * shrinkAmount;

            float maxAxis = Mathf.Max(scale.x, Mathf.Max(scale.y, scale.z));
            if (maxAxis <= minScaleToDestroy)
            {
                checklist.allDirt--;
                checklist.remaining--;
                Destroy(dirt.gameObject);
                return;
            }

            dirt.localScale = scale;
            return;
        }

        // ----- 2) New: clean Shader on surfaces -----
        CleanableSurface surface = other.GetComponent<CleanableSurface>();
        if (surface != null)
        {
            float amount = surfaceCleanRate * Time.deltaTime;
            surface.Clean(amount);
        }
    }
}

