using UnityEngine;

public class BallBalanceObject : MonoBehaviour
{
    [Header("Script references")]
    public BallBalancingManager manager;

    [Header("Object attributes")]
    public bool completed;

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

    }
}
