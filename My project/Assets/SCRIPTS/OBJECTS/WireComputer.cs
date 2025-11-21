using UnityEngine;

public class WireComputer : MonoBehaviour
{
    [Header("Script references")]
    public WireMGManager wireManager; 
        
    [Header("Object attributes")]
    public bool completed;

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

    }
}

