using UnityEngine;

/// <summary>
/// Represents a computer that hosts a wire-connection minigame.
/// Handles interaction, ambient broken sounds, completion feedback,
/// and communicates with the WireMGManager.
/// </summary>
public class WireComputer : MonoBehaviour
{
    // ================================
    // Script References
    // ================================

    [Header("Script References")]

    public WireMGManager wireManager;   // Wire minigame manager


    // ================================
    // Object State & Feedback
    // ================================

    [Header("Object Attributes")]

    public bool completed;              // Whether this computer has been fixed
    public ParticleSystem activeParticles; // Visual indicator while broken
    public AudioSource sfxSource;        // Audio source for ambient & completion sounds
    public AudioClip completionSfx;      // Played once when fixed
    public AudioClip brokenSfx;          // Looping sound while broken


    // ================================
    // Distance-Based Audio Settings
    // ================================

    [Header("Distance Settings")]

    public float hearRange = 5f;         // Distance at which broken SFX can be heard


    // ================================
    // Internal State
    // ================================

    private Transform player;            // Cached player transform
    private bool isBrokenSoundPlaying = false; // Tracks ambient sound state


    void Start()
    {
        // Cache player reference
        player = GameObject.FindGameObjectWithTag("Player").transform;

        // Play visual feedback if not completed
        if (!completed && activeParticles != null)
            activeParticles.Play();

        // Prepare audio source (do not auto-play)
        if (sfxSource != null)
        {
            sfxSource.loop = true;
            sfxSource.playOnAwake = false;
        }
    }

    void Update()
    {
        // Skip if already completed or audio is not set up
        if (completed || sfxSource == null || brokenSfx == null)
            return;

        float distance = Vector3.Distance(player.position, transform.position);

        if (distance <= hearRange)
        {
            // Player is close enough to hear broken sound
            if (!isBrokenSoundPlaying)
            {
                sfxSource.clip = brokenSfx;
                sfxSource.Play();
                isBrokenSoundPlaying = true;
            }
        }
        else
        {
            // Player moved out of range
            if (isBrokenSoundPlaying)
            {
                sfxSource.Stop();
                isBrokenSoundPlaying = false;
            }
        }
    }

    /// <summary>
    /// Called when the player interacts with this computer
    /// </summary>
    public void Interact()
    {
        if (completed)
        {
            Debug.Log(name + ": already completed.");
            return;
        }

        wireManager.OpenForComputer(this);
    }

    /// <summary>
    /// Marks this computer as repaired
    /// </summary>
    public void MarkCompleted()
    {
        completed = true;

        Debug.Log(name + ": puzzle completed!");

        // Stop visual effects
        if (activeParticles != null)
            activeParticles.Stop();

        // Stop broken ambient sound immediately
        if (sfxSource != null)
        {
            sfxSource.Stop();
            isBrokenSoundPlaying = false;
        }

        // Play completion sound once
        if (sfxSource != null && completionSfx != null)
            sfxSource.PlayOneShot(completionSfx);
    }
}


