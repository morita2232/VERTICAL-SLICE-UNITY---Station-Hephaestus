using UnityEngine;

/// <summary>
/// Represents a conduit object in the world that hosts
/// a conduit charging puzzle. Handles interaction, ambient
/// broken sounds, visual feedback, and completion state.
/// </summary>
public class ConduitObject : MonoBehaviour
{
    // ================================
    // Script References
    // ================================

    [Header("Script References")]

    public ConduitManager manager;        // Conduit puzzle manager


    // ================================
    // Object State & Visual Feedback
    // ================================

    [Header("Object Attributes")]

    public bool completed;                // Whether this conduit has been fixed
    public ParticleSystem activeParticles; // Visual indicator while broken


    // ================================
    // Audio Settings
    // ================================

    [Header("Audio")]

    public AudioSource sfxSource;          // Audio source for ambient & completion sounds
    public AudioClip completionSfx;        // Played once when fixed
    public AudioClip brokenSfx;            // Looping sound while broken


    // ================================
    // Distance-Based Audio Settings
    // ================================

    [Header("Distance Settings")]

    public float hearRange = 5f;           // Distance at which broken SFX is audible


    // ================================
    // Internal State
    // ================================

    private Transform player;              // Cached player transform
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
        // Skip if already completed or audio not set up
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
    /// Called when the player interacts with this conduit
    /// </summary>
    public void Interact()
    {
        if (completed)
        {
            Debug.Log(name + ": conduit puzzle already completed.");
            return;
        }

        manager.OpenForObject(this);
    }

    /// <summary>
    /// Marks this conduit puzzle as completed
    /// </summary>
    public void MarkCompleted()
    {
        completed = true;

        Debug.Log(name + ": conduit puzzle completed!");

        // Stop visual feedback
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


