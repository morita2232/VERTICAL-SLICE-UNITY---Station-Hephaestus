using UnityEngine;

/// <summary>
/// Prevents the Rigidbody from ever going to sleep.
/// Useful for physics objects that must always stay active.
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class NoSleep : MonoBehaviour
{
    // ================================
    // Rigidbody Configuration
    // ================================

    void Start()
    {
        // Get the attached Rigidbody
        var rb = GetComponent<Rigidbody>();

        // Disable sleeping so physics updates always run
        rb.sleepThreshold = 0f;
    }
}


