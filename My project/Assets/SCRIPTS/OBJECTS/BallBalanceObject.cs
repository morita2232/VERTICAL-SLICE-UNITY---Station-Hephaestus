using UnityEngine;

public class BallBalanceObject : MonoBehaviour
{
    [Header("Script references")]
    public BallBalancingManager manager;

    [Header("Object attributes")]
    public bool completed;

    public void Interact()
    {


        Debug.Log("Se activo el juego guarra");

        manager.OpenForObject(this);

    }
}
