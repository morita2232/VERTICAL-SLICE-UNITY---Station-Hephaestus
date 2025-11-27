using UnityEngine;

public class CheckList : MonoBehaviour
{
    private WireComputer[] wireComputers;
    private BallBalanceObject[] balanceObjects;

    [Header("Attributes")]
    public Vector2 uiOffset = new Vector2(10f, 10f);
    public Vector2 uiSize = new Vector2(250f, 200f);

    void Awake()
    {
        wireComputers = FindObjectsByType<WireComputer>(FindObjectsSortMode.None);
        balanceObjects = FindObjectsByType<BallBalanceObject>(FindObjectsSortMode.None);
    }


    void OnGUI()
    {
        // draw a simple box
        Rect area = new Rect(uiOffset.x, uiOffset.y, uiSize.x, uiSize.y);
        GUILayout.BeginArea(area, GUI.skin.box);

        GUILayout.Label("Checklist");

        int remaining = 0;

        if (wireComputers != null)
        {
            foreach (var comp in wireComputers)
            {
                if (comp == null) continue;

                if (!comp.completed)
                {
                    remaining++;
                    GUILayout.Label("• Fix " + comp.gameObject.name);
                }
            }
        }

        if (balanceObjects != null)
        {
            foreach (var obj in balanceObjects)
            {
                if (obj == null) continue;

                if (!obj.completed)
                {
                    remaining++;
                    GUILayout.Label("• Fix " + obj.gameObject.name);
                }
            }
        }

        if (remaining == 0)
        {
            GUILayout.Space(5);
            GUILayout.Label("All tasks complete!");
        }
        else
        {
            GUILayout.Space(5);
            GUILayout.Label("Remaining: " + remaining);
        }

        GUILayout.EndArea();
    }
}


