using UnityEngine;

public class CheckList : MonoBehaviour
{
    private WireComputer[] wireComputers;
    private BallBalanceObject[] balanceObjects;
    private ConduitObject[] conduitObjects;
    private GameObject[] dirt;

    public int allDirt = 0;
    public int remaining = 0;

    [Header("Attributes")]
    public Vector2 uiOffset = new Vector2(10f, 10f);
    public Vector2 uiSize = new Vector2(250f, 200f);

    void Awake()
    {
        wireComputers = FindObjectsByType<WireComputer>(FindObjectsSortMode.None);
        balanceObjects = FindObjectsByType<BallBalanceObject>(FindObjectsSortMode.None);
        conduitObjects = FindObjectsByType<ConduitObject>(FindObjectsSortMode.None);
        dirt = GameObject.FindGameObjectsWithTag("Dirt");
    }

    void OnGUI()
    {
        // IMPORTANT: reset counters each draw
        remaining = 0;
        allDirt = 0;

        Rect area = new Rect(uiOffset.x, uiOffset.y, uiSize.x, uiSize.y);
        GUILayout.BeginArea(area, GUI.skin.box);

        GUILayout.Label("Checklist");

        // Wire computers
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

        // Ball balance objects
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

        // Conduit objects
        if (conduitObjects != null)
        {
            foreach (var obj in conduitObjects)
            {
                if (obj == null) continue;

                if (!obj.completed)
                {
                    remaining++;
                    GUILayout.Label("• Fix " + obj.gameObject.name);
                }
            }
        }

        // Dirt
        if (dirt != null)
        {
            foreach (var obj in dirt)
            {
                if (obj == null) continue;

                if (obj.activeSelf)
                {
                    allDirt++;
                    remaining++;
                }
            }
            if (allDirt > 0)
            {
                 GUILayout.Label("• Clean " + allDirt);

            }
        }

        // Summary
        GUILayout.Space(5);

        if (remaining == 0)
            GUILayout.Label("All tasks complete!");
        else
            GUILayout.Label("Remaining: " + remaining);

        GUILayout.EndArea();
    }
}



