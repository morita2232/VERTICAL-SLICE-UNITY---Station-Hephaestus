using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Displays and tracks all remaining objectives in the level:
/// - Puzzle objects (wires, ball balance, conduits)
/// - Dirt cleaning
/// - Trash disposal
/// Triggers tutorial completion and level transition when finished.
/// </summary>
public class CheckList : MonoBehaviour
{
    // ================================
    // World Object References (Auto-Filled)
    // ================================

    private WireComputer[] wireComputers;
    private BallBalanceObject[] balanceObjects;
    private ConduitObject[] conduitObjects;
    private GameObject[] dirt;
    private GameObject[] trash;


    // ================================
    // Dialogue Assets
    // ================================

    public TMP_FontAsset sammyFont;
    public Sprite sammyPortrait;


    // ================================
    // Checklist State
    // ================================

    public bool isTutorial = false;   // Whether this checklist belongs to tutorial
    private bool finished = false;    // Prevents multiple completions


    // ================================
    // Counters (Runtime)
    // ================================

    public int allDirt = 0;
    public int remaining = 0;

    public int allTrash = 0;
    public int remainingTrash = 0;


    // ================================
    // UI Layout Settings
    // ================================

    [Header("Attributes")]

    public Vector2 uiOffset = new Vector2(10f, 10f);   // Top-left offset
    public Vector2 uiSize = new Vector2(250f, 200f);   // UI box size


    // ================================
    // Loading Screen
    // ================================

    [Header("Loading Screen")]

    public GameObject loadingScreen;
    public Slider loadingBar;


    void Awake()
    {
        // Detect tutorial scene
        if (SceneManager.GetActiveScene().name == "Tutorial")
        {
            isTutorial = true;
        }

        // Cache all relevant objects in the scene
        wireComputers = FindObjectsByType<WireComputer>(FindObjectsSortMode.None);
        balanceObjects = FindObjectsByType<BallBalanceObject>(FindObjectsSortMode.None);
        conduitObjects = FindObjectsByType<ConduitObject>(FindObjectsSortMode.None);

        dirt = GameObject.FindGameObjectsWithTag("Dirt");
        trash = GameObject.FindGameObjectsWithTag("Trash");
    }

    void OnGUI()
    {
        // IMPORTANT: reset counters every draw
        remaining = 0;
        remainingTrash = 0;
        allTrash = 0;
        allDirt = 0;

        Rect area = new Rect(uiOffset.x, uiOffset.y, uiSize.x, uiSize.y);
        GUILayout.BeginArea(area, GUI.skin.box);

        GUILayout.Label("Checklist");

        // ================================
        // Wire Computers
        // ================================

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

        // ================================
        // Ball Balance Objects
        // ================================

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

        // ================================
        // Conduit Objects
        // ================================

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

        // ================================
        // Dirt
        // ================================

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

        // ================================
        // Trash
        // ================================

        if (trash != null)
        {
            foreach (var obj in trash)
            {
                if (obj == null) continue;

                allTrash++;

                if (obj.activeSelf)
                {
                    remainingTrash++;
                    remaining++;
                }
            }

            if (allTrash > 0)
            {
                GUILayout.Label("• Dispose " + allTrash);
            }
        }

        // ================================
        // Summary
        // ================================

        GUILayout.Space(5);

        if (remaining <= 0)
        {
            GUILayout.Label("All tasks complete!");

            if (!finished)
            {
                finito();
                finished = true;
            }
        }
        else
        {
            GUILayout.Label("Remaining: " + remaining);
        }

        GUILayout.EndArea();
    }

    /// <summary>
    /// Called once when all checklist tasks are completed
    /// </summary>
    void finito()
    {
        if (isTutorial)
        {
            DialogueManager.OnDialogueSequenceFinished += LoadNextLevel;

            DialogueManager.Instance.SayLines(
                "Spammy Sammy",
                new string[]
                {
                    "Now that you know the basics you are ready to work for our great bosses!!!.",
                    "GOOD LUCK OUT THERE!!!"
                },
                sammyFont,
                sammyPortrait
            );
        }
    }

    /// <summary>
    /// Loads the next level after dialogue finishes
    /// </summary>
    void LoadNextLevel()
    {
        // Unsubscribe so it runs only once
        DialogueManager.OnDialogueSequenceFinished -= LoadNextLevel;

        // Start async load with loading screen
        StartCoroutine(LoadSceneAsynchronously("Level_1"));
    }

    /// <summary>
    /// Asynchronously loads a scene while updating a loading bar
    /// </summary>
    IEnumerator LoadSceneAsynchronously(string sceneName)
    {
        loadingScreen.SetActive(true);

        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
        operation.allowSceneActivation = true;

        while (!operation.isDone)
        {
            // Normalize progress (Unity stops at 0.9)
            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            loadingBar.value = progress;

            yield return null;
        }
    }
}




