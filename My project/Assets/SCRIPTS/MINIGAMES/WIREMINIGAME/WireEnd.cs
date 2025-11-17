using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class WireEnd : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public int id;

    public Image img;
    public bool connected = false;
    void Awake()
    {
        img = GetComponent<Image>();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
