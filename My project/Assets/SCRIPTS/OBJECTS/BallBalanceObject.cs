using UnityEngine;

public class BallBalanceObject : MonoBehaviour
{
    [Header("Script references")]
    public BallBalancingManager manager;

    [Header("Object attributes")]
    public bool completed;
    public ParticleSystem activeParticles;
    public AudioSource sfxSource;
    public AudioClip completionSfx;
    public AudioClip brokenSfx;

    [Header("Distance Settings")]
    public float hearRange = 5f;      // distance at which broken SFX starts
    private Transform player;         // reference to player
    private bool isBrokenSoundPlaying = false;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;

        if (!completed && activeParticles != null)
            activeParticles.Play();

        // DO NOT play the sound here anymore, wait for Update()
        if (sfxSource != null)
        {
            sfxSource.loop = true;
            sfxSource.playOnAwake = false;
        }
    }
    void Update()
    {
        if (completed || sfxSource == null || brokenSfx == null)
            return;

        float distance = Vector3.Distance(player.position, transform.position);

        if (distance <= hearRange)
        {
            // Player is close enough to hear
            if (!isBrokenSoundPlaying)
            {
                sfxSource.clip = brokenSfx;
                sfxSource.Play();
                isBrokenSoundPlaying = true;
            }
        }
        else
        {
            // Player is too far away
            if (isBrokenSoundPlaying)
            {
                sfxSource.Stop();
                isBrokenSoundPlaying = false;
            }
        }
    }
    public void Interact()
    {
        if (completed)
        {
            Debug.Log(name + ": already completed.");
            return;
        }

        manager.OpenForObject(this);

    }

    public void MarkCompleted()
    {
        completed = true;
        Debug.Log(name + ": conduit puzzle completed!");

        if (activeParticles != null)
            activeParticles.Stop();

        // Stop broken sounds immediately
        if (sfxSource != null)
        {
            sfxSource.Stop();
            isBrokenSoundPlaying = false;
        }

        // Play completion SFX once
        if (sfxSource != null && completionSfx != null)
            sfxSource.PlayOneShot(completionSfx);
    }
}
