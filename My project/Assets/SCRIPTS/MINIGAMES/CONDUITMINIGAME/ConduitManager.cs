using UnityEngine;

public class ConduitManager : MonoBehaviour
{
    public ConduitTube[] tubes;
    public KeyCode lockKey = KeyCode.E;  // "charge" key
    public bool solved { get; private set; }

    void Update()
    {
        if (solved) return;

        // press E: all tubes start charging from zero
        if (Input.GetKeyDown(lockKey))
        {
            foreach (var t in tubes)
            {
                if (t == null) continue;
                t.StartCharge();
            }
        }

        CheckSolved();
    }

    void CheckSolved()
    {
        foreach (var t in tubes)
        {
            if (t == null) return;
            if (!t.isLocked) return;   // all must have succeeded
        }

        solved = true;
        OnSolved();
    }

    void OnSolved()
    {
        Debug.Log("Conduit puzzle solved!");
        // open door, enable power, etc.
    }
}
