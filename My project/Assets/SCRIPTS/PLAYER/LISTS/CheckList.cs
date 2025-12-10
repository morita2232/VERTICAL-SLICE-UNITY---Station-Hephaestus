using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CheckList : MonoBehaviour
{
    private WireComputer[] wireComputers;
    private BallBalanceObject[] balanceObjects;
    private ConduitObject[] conduitObjects;
    private GameObject[] dirt;
    private GameObject[] trash;
    public TMP_FontAsset sammyFont;
    public Sprite sammyPortrait;
    public bool isTutorial = false;
    private bool finished = false;

    public int allDirt = 0;
    public int remaining = 0;

    public int allTrash = 0;
    public int remainingTrash = 0;

    [Header("Attributes")]
    public Vector2 uiOffset = new Vector2(10f, 10f);
    public Vector2 uiSize = new Vector2(250f, 200f);

    [Header("Loading Screen")]
    public GameObject loadingScreen;
    public Slider loadingBar;

    void Awake()
    {
        if(SceneManager.GetActiveScene().name == "Tutorial") {
            isTutorial = true;
        }
        wireComputers = FindObjectsByType<WireComputer>(FindObjectsSortMode.None);
        balanceObjects = FindObjectsByType<BallBalanceObject>(FindObjectsSortMode.None);
        conduitObjects = FindObjectsByType<ConduitObject>(FindObjectsSortMode.None);
        dirt = GameObject.FindGameObjectsWithTag("Dirt");
        trash = GameObject.FindGameObjectsWithTag("Trash");
    }

    void OnGUI()
    {
        //// IMPORTANT: reset counters each draw
        remaining = 0;
        remainingTrash = 0;
        allTrash = 0;
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

            // Summary
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
                }, sammyFont, sammyPortrait
            );
        }
    }

    void LoadNextLevel()
    {
        // Desuscribirse para que solo se llame una vez
        DialogueManager.OnDialogueSequenceFinished -= LoadNextLevel;

        // Empezar la carga asíncrona con pantalla de carga
        StartCoroutine(LoadSceneAsynchronously("Level_1"));
    }



    IEnumerator LoadSceneAsynchronously(string sceneName)
    {
        loadingScreen.SetActive(true);

        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
        operation.allowSceneActivation = true;   // opcional, por si luego quieres hacer cosas fancy

        while (!operation.isDone)
        {
            // Opcional: normalizar a 0–1 porque progress llega solo hasta 0.9
            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            loadingBar.value = progress;

            // Debug para ver cuánto va cargando
            // Debug.Log(progress);

            yield return null;
        }
    }

}



