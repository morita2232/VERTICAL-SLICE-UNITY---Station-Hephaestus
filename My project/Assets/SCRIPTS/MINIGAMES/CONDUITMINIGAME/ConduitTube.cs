using UnityEngine;

/// <summary>
/// Controls a chargeable conduit tube:
/// - Fills while active
/// - Drains while inactive
/// - Randomizes a target (red) zone
/// - Updates shader visuals based on state
/// </summary>
[RequireComponent(typeof(Renderer))]
public class ConduitTube : MonoBehaviour
{
    // ================================
    // Charge Settings
    // ================================

    [Header("Charge Settings")]

    public float chargeSpeed = 0.5f;    // How fast the tube fills when active
    public float drainSpeed = 0.2f;     // How fast the tube drains when inactive
    public Vector2 targetRange = new Vector2(0.3f, 0.7f); // Range where target zone can appear


    // ================================
    // Debug / Runtime State (Read-Only)
    // ================================

    [Header("Debug (read-only)")]

    [Range(0f, 1f)] public float targetMin;   // Lower bound of target zone
    [Range(0f, 1f)] public float targetMax;   // Upper bound of target zone
    [Range(0f, 1f)] public float currentFill; // Current fill amount


    // ================================
    // State Flags
    // ================================

    public bool isActive { get; private set; } // Whether the tube is currently charging


    // ================================
    // Internal References
    // ================================

    Renderer rend;           // Cached renderer
    Material matInstance;    // Instance material (unique per tube)


    /// <summary>
    /// True if current fill is within the target zone
    /// </summary>
    public bool InTarget =>
        currentFill >= targetMin && currentFill <= targetMax;


    void Awake()
    {
        // Cache renderer and create material instance
        rend = GetComponent<Renderer>();
        matInstance = rend.material;

        // Randomize target zone within allowed range
        float center = Random.Range(targetRange.x, targetRange.y);
        float halfSize = 0.05f;

        targetMin = Mathf.Clamp01(center - halfSize);
        targetMax = Mathf.Clamp01(center + halfSize);

        matInstance.SetFloat("_TargetMin", targetMin);
        matInstance.SetFloat("_TargetMax", targetMax);

        // Initialize fill state
        currentFill = 0f;
        matInstance.SetFloat("_Fill", currentFill);

        // Default fill color
        matInstance.SetColor("_FillColor", Color.cyan);
    }

    void Update()
    {
        float dt = Time.deltaTime;

        if (isActive)
        {
            // Fill while active
            currentFill += chargeSpeed * dt;
            currentFill = Mathf.Clamp01(currentFill);
        }
        else
        {
            // Drain while inactive
            if (currentFill > 0f)
            {
                currentFill -= drainSpeed * dt;
                if (currentFill < 0f)
                    currentFill = 0f;
            }
        }

        // Update shader fill value
        matInstance.SetFloat("_Fill", currentFill);

        // Update fill color based on target state
        if (InTarget)
            matInstance.SetColor("_FillColor", Color.green);
        else
            matInstance.SetColor("_FillColor", Color.cyan);
    }

    /// <summary>
    /// Enables or disables charging
    /// </summary>
    public void SetActive(bool value)
    {
        isActive = value;
    }

    /// <summary>
    /// Resets the tube to its initial state
    /// </summary>
    public void ResetTube()
    {
        isActive = false;
        currentFill = 0f;

        matInstance.SetFloat("_Fill", currentFill);
        matInstance.SetColor("_FillColor", Color.cyan);
    }
}


