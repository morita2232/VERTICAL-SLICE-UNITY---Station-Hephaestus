using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class NoSleep : MonoBehaviour
{
    void Start()
    {
        var rb = GetComponent<Rigidbody>();
        rb.sleepThreshold = 0f;   // disables sleeping for this rigidbody
    }
}

