using UnityEngine;

public class ScreenWrap3D : MonoBehaviour
{
    public Transform target;  // the ship
    private BoxCollider box;

    void Awake()
    {
        box = GetComponent<BoxCollider>();
    }

    void Update()
    {
        WrapTarget();
    }

    void WrapTarget()
    {
        Vector3 pos = target.position;
        Bounds b = box.bounds;

        // X wrap
        if (pos.x > b.max.x) pos.x = b.min.x;
        else if (pos.x < b.min.x) pos.x = b.max.x;

        // Y wrap
        if (pos.y > b.max.y) pos.y = b.min.y;
        else if (pos.y < b.min.y) pos.y = b.max.y;

        // Z wrap (if you're using 2D, you can omit this)
        if (pos.z > b.max.z) pos.z = b.min.z;
        else if (pos.z < b.min.z) pos.z = b.max.z;

        target.position = pos;
    }
}

