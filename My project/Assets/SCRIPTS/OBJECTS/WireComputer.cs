using UnityEngine;

public class WireComputer : MonoBehaviour
{
    [Header("Script references")]
    public WireMGManager wireManager; 
        
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


    // called when player presses E on this computer
    public void Interact()
    {
        if (completed)
        {
            Debug.Log(name + ": already completed.");
            return;
        }

        wireManager.OpenForComputer(this);
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

