using UnityEngine;

public class CheckList : MonoBehaviour
{
    private WireComputer[] wireComputers;

    // position & size of the checklist box on screen
    public Vector2 uiOffset = new Vector2(10f, 10f);
    public Vector2 uiSize = new Vector2(250f, 200f);

    void Awake()
    {
        wireComputers = FindObjectsByType<WireComputer>(FindObjectsSortMode.None);
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


