using UnityEngine;

public class ConduitObject : MonoBehaviour
{
    [Header("Script references")]
    public ConduitManager manager;

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
            Debug.Log(name + ": conduit puzzle already completed.");
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

        if (sfxSource != null && completionSfx != null)
            sfxSource.PlayOneShot(completionSfx);
    }

}
