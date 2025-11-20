using UnityEngine;

public class WireComputer : MonoBehaviour
{
    public WireMGManager wireManager;   // drag your single manager here in the Inspector
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

        // here you can change material, turn on a light, etc.
    }
}

