using UnityEngine;

/// <summary>
/// Handles cleaning behavior for the mop:
/// - Shrinks and removes dirt objects
/// - Cleans shader-based surfaces
/// - Requires the mop to be moving to clean
/// </summary>
public class MopCleaner : MonoBehaviour
{
    // ================================
    // Cleaning Settings
    // ================================

    [Header("Cleaning Settings")]

    public float shrinkRate = 0.5f;         // Units of scale reduced per second (dirt)
    public float minScaleToDestroy = 0.1f;  // Destroy dirt when smaller than this
    public float minMoveSpeed = 0.1f;       // Mop must move at least this fast to clean

    public float surfaceCleanRate = 0.2f;   // How fast shader "Slide" increases


    // ================================
    // Checklist & State
    // ================================

    public CheckList checklist;              // Checklist reference (assigned in Inspector)

    private Vector3 lastPosition;            // Position last frame
    private float currentSpeed;              // Calculated movement speed


    void Start()
    {
        // Initialize last position
        lastPosition = transform.position;
    }

    void Update()
    {
        // Calculate movement speed
        Vector3 delta = transform.position - lastPosition;
        currentSpeed = delta.magnitude / Time.deltaTime;
        lastPosition = transform.position;
    }

    void OnTriggerStay(Collider other)
    {
        // Mop must be moving to clean
        if (currentSpeed < minMoveSpeed)
            return;


        // ================================
        // Clean Shader-Based Surfaces
        // ================================

        CleanableSurface surface = other.GetComponent<CleanableSurface>();
        if (surface != null)
        {
            float amount = surfaceCleanRate * Time.deltaTime;
            surface.Clean(amount);
        }
    }
}

