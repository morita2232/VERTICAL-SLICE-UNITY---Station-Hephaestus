using UnityEngine;

public class GroundDetection : MonoBehaviour
{

    public Vector3 offset;
    public float radius;
    public float distance;
    public bool grounded;
    public LayerMask mask;

    private void Update()
    {
        RaycastHit hit;

        if (Physics.SphereCast(transform.position + offset,
            radius, Vector3.down, out hit, distance, mask))
        {
            grounded = true;
        }
        else
        {
            grounded = false;
        }
    }


    private void OnDrawGizmos()
    {
        //Inicial
        Gizmos.DrawWireSphere(transform.position + offset, radius);

        //Final
        Gizmos.DrawWireSphere((transform.position + offset)
            + Vector3.down * distance, radius);
    }

}
