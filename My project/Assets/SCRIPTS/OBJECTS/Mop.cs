using UnityEngine;

public class Mop : MonoBehaviour
{
    public Transform holdPos; // assign MopHoldPos in inspector
    public bool holding;
    private void Update()
    {
        if (holding)
        {
            if (Input.GetKeyDown(KeyCode.G))
            {
                Drop();
            }
        }
    }
    public void PickUp()
    {
        holding = true;

        // Make mop follow the pivot
        transform.SetParent(holdPos);

        // Position at pivot
        transform.localPosition = Vector3.zero;

        // Rotate relative to pivot so it’s sideways in hand
        // Tweak these values until it looks right in game view
        transform.localRotation = Quaternion.Euler(0f, 90f, 0f);

    }

    public void Drop()
    {
        holding = false;

        transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        
        transform.position = new Vector3(
            transform.position.x,
            0.3000245f,
            transform.position.z
        );


        transform.SetParent(null);
    }

}





