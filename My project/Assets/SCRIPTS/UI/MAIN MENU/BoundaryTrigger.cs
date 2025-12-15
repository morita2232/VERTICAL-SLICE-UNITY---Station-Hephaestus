using UnityEngine;

/// <summary>
/// Wraps a target Transform inside a 3D BoxCollider.
/// When the target exits one side of the box, it reappears on the opposite side.
/// Useful for looping space objects, ships, or background elements.
/// </summary>
public class ScreenWrap3D : MonoBehaviour
{
    // ================================
    // References
    // ================================

    public Transform target;      // Object to wrap (e.g. a ship)


    // ================================
    // Internal References
    // ================================

    private BoxCollider box;      // Defines the wrapping bounds


    void Awake()
    {
        // Cache the BoxCollider used as bounds
        box = GetComponent<BoxCollider>();
    }

    void Update()
    {
        WrapTarget();
    }

    /// <summary>
    /// Checks the target position and wraps it
    /// around the BoxCollider bounds on each axis.
    /// </summary>
    void WrapTarget()
    {
        Vector3 pos = target.position;
        Bounds b = box.bounds;

        // ================================
        // X Axis Wrap
        // ================================

        if (pos.x > b.max.x)
            pos.x = b.min.x;
        else if (pos.x < b.min.x)
            pos.x = b.max.x;

        // ================================
        // Y Axis Wrap
        // ================================

        if (pos.y > b.max.y)
            pos.y = b.min.y;
        else if (pos.y < b.min.y)
            pos.y = b.max.y;

        // ================================
        // Z Axis Wrap
        // ================================

        // Optional for 2D-style setups
        if (pos.z > b.max.z)
            pos.z = b.min.z;
        else if (pos.z < b.min.z)
            pos.z = b.max.z;

        target.position = pos;
    }
}


