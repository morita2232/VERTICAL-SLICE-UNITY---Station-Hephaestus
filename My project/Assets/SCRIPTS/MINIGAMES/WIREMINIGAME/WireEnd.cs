using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class WireEnd : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header ("Attributes")]
    public int id;

    public Image img;


    [Header ("Is it connected to a wire start")]
    public bool connected = false;

 
    void Awake()
    {
        img = GetComponent<Image>();

    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("Mouse entered " + gameObject.name);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("Mouse exited " + gameObject.name);
    }
}
