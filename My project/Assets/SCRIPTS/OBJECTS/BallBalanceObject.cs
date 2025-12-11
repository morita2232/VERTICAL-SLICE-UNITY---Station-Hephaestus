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

    void Start()
    {
        if (!completed && activeParticles != null)
            activeParticles.Play();
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
        Debug.Log(name + ": puzzle completed!");
        if (activeParticles != null)
            activeParticles.Stop();

        if (sfxSource != null && completionSfx != null)
            sfxSource.PlayOneShot(completionSfx);
    }
}
