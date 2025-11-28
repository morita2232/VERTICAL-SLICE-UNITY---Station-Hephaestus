using UnityEngine;

public class Mop : MonoBehaviour
{
    public GameObject holdPos;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PickUp()
    {
        transform.position = holdPos.transform.position;
       // transform.rotation = new Vector3(90f, 0f, 0f);
    }
}
