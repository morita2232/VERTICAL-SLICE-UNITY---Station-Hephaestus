//using UnityEngine;

//public class BoundaryTrigger : MonoBehaviour
//{
//    public GameObject fakeSS;
//    private Vector3 pos;

//    private void Start()
//    {
//        Debug.Log("Existo");

//         pos = fakeSS.transform.position;
//    }

//    private void Update()
//    {

//            if (!GetComponent<Collider>().bounds.Contains(fakeSS.transform.position))
//            {
//                Debug.Log("FakeSS started inside the bounds.");
//                fakeSS.transform.position = new Vector3(-1 * pos.x, -1 * pos.y, 0);

//            }


//    }
//}

using UnityEngine;

public class ScreenWrap3D : MonoBehaviour
{
    public Transform target;  // Ship or fakeSS
    private BoxCollider box;  // The area you want to wrap

    private void Awake()
    {
        box = GetComponent<BoxCollider>();
    }

    private void Update()
    {
        WrapTarget();
    }

    private void WrapTarget()
    {
        Vector3 pos = target.position;
        Bounds b = box.bounds;

        // X wrap
        if (pos.x > b.max.x) pos.x = b.min.x;
        else if (pos.x < b.min.x) pos.x = b.max.x;

        // Y wrap
        if (pos.y > b.max.y) pos.y = b.min.y;
        else if (pos.y < b.min.y) pos.y = b.max.y;

        // Z wrap
        if (pos.z > b.max.z) pos.z = b.min.z;
        else if (pos.z < b.min.z) pos.z = b.max.z;

        target.position = pos;
    }
}

