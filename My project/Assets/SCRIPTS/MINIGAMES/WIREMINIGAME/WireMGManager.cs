using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages the wire-connection minigame:
/// - Assigns wire IDs and colors
/// - Tracks wire connections
/// - Locks and restores player controls
/// - Notifies the owning computer when solved
/// </summary>
public class WireMGManager : MonoBehaviour
{
    // ================================
    // Display & Visual Settings
    // ================================

    [Header("Display Attributes")]

    public GameObject panelRoot;          // Root UI panel for the minigame
    public List<Color> availableColors;   // Pool of colors for wire pairs
    public float snapBufferPixels = 30f;  // Distance threshold for snapping wires


    // ================================
    // Player & Interaction References
    // ================================

    [Header("Script References")]

    public InteractLocator playerInteractLocator;
    public HorizontalMovement playerHorizontalMovement;
    public Movement playerMovement;
    public RotacionVertical playerVerticalMovement;


    // ================================
    // Wire Configuration
    // ================================

    [Header("Wire Lists")]

    public List<WireStart> wireStart;     // All draggable wire starts
    public List<WireEnd> wireEnd;         // All wire endpoints
    public GameObject crossHair;           // Player crosshair UI


    // ================================
    // Internal State
    // ================================

    private Dictionary<WireStart, WireEnd> pairings =
        new Dictionary<WireStart, WireEnd>(); // Start -> End mappings

    private WireComputer currentOwner;     // Computer currently using the minigame
    private bool isMinigameOpen = false;


    void Start()
    {
        // Hide UI initially
        if (panelRoot != null)
            panelRoot.SetActive(false);

        // Validate configuration
        if (availableColors == null || availableColors.Count == 0)
        {
            Debug.LogError("availableColors is empty or null!");
            return;
        }

        if (wireStart == null || wireEnd == null)
        {
            Debug.LogError("wireStart or wireEnd is not assigned!");
            return;
        }

        // Assign IDs and colors once
        AssignUniqueIdsForList(wireStart);
        AssignUniqueIdsForList(wireEnd);

        // Build start -> end pairings
        BuildPairings();
    }

    /// <summary>
    /// Builds the dictionary that matches wire starts to wire ends by ID
    /// </summary>
    void BuildPairings()
    {
        pairings.Clear();

        foreach (var start in wireStart)
        {
            if (start == null) continue;

            WireEnd match = wireEnd.Find(e => e != null && e.id == start.id);
            if (match != null)
                pairings[start] = match;
        }
    }

    /// <summary>
    /// Opens the wire minigame for a specific computer
    /// </summary>
    public void OpenForComputer(WireComputer owner)
    {
        currentOwner = owner;

        // Reset wire ends
        foreach (var e in wireEnd)
        {
            if (e == null) continue;
            e.connected = false;
        }

        // Reset wire starts
        foreach (var s in wireStart)
        {
            if (s == null) continue;
            s.ResetState();
            s.gameObject.SetActive(true);
        }

        if (panelRoot != null)
            panelRoot.SetActive(true);

        playerInteractLocator.isInminigame = true;
        isMinigameOpen = true;
        crossHair.SetActive(false);

        Debug.Log("Wire minigame opened for " + owner.gameObject.name);
    }

    // =========================================================

    void Update()
    {
        // Skip if not in minigame
        if (!playerInteractLocator.isInminigame) return;

        // Exit minigame
        if (Input.GetKeyDown(KeyCode.Q))
        {
            EndMinigame();
            return;
        }

        // Keep player locked every frame
        playerMovement.canMove = false;
        playerHorizontalMovement.canMove = false;
        playerVerticalMovement.canRotate = false;
        Cursor.lockState = CursorLockMode.None;

        // Check wire connections
        foreach (var kv in pairings)
        {
            WireStart start = kv.Key;
            WireEnd end = kv.Value;

            if (start == null || end == null) continue;
            if (!start.gameObject.activeInHierarchy) continue;
            if (end.connected) continue;

            Vector2 endScreenPos = RectTransformUtility.WorldToScreenPoint(
                end.GetComponentInParent<Canvas>() != null
                    ? end.GetComponentInParent<Canvas>().worldCamera
                    : null,
                end.transform.position
            );

            Vector2 startScreenPos = start.screenPos;
            if (startScreenPos == Vector2.zero)
            {
                startScreenPos = RectTransformUtility.WorldToScreenPoint(
                    start.GetComponentInParent<Canvas>() != null
                        ? start.GetComponentInParent<Canvas>().worldCamera
                        : null,
                    start.transform.position
                );
            }

            float dist = Vector2.Distance(startScreenPos, endScreenPos);
            if (dist <= snapBufferPixels)
            {
                end.connected = true;

                Debug.Log(
                    $"Connected {start.gameObject.name} -> {end.gameObject.name} (dist={dist})"
                );

                // Hide start instead of snapping it
                start.gameObject.SetActive(false);
            }
        }

        // Check completion
        int remaining = 0;
        for (int i = 0; i < wireEnd.Count; i++)
            if (!wireEnd[i].connected) remaining++;

        if (remaining == 0)
        {
            if (currentOwner != null)
                currentOwner.MarkCompleted();

            if (!isMinigameOpen) return;
            EndMinigame();
        }
    }

    /// <summary>
    /// Closes the minigame and restores player control
    /// </summary>
    private void EndMinigame()
    {
        playerInteractLocator.isInminigame = false;
        isMinigameOpen = false;

        playerMovement.canMove = true;
        playerHorizontalMovement.canMove = true;
        playerVerticalMovement.canRotate = true;

        Cursor.lockState = CursorLockMode.Locked;
        crossHair.SetActive(true);

        if (panelRoot != null)
            panelRoot.SetActive(false);

        Debug.Log("Minigame closed.");
    }

    /// <summary>
    /// Assigns unique IDs and colors to wire elements
    /// </summary>
    void AssignUniqueIdsForList<T>(List<T> list) where T : class
    {
        if (list == null || list.Count == 0) return;

        int poolSize = availableColors.Count;
        int needed = Mathf.Min(list.Count, poolSize);

        List<int> idPool = new List<int>(poolSize);
        for (int i = 0; i < poolSize; i++) idPool.Add(i);

        // Shuffle ID pool
        for (int i = 0; i < idPool.Count; i++)
        {
            int j = Random.Range(i, idPool.Count);
            int tmp = idPool[i];
            idPool[i] = idPool[j];
            idPool[j] = tmp;
        }

        for (int i = 0; i < needed; i++)
        {
            var element = list[i];
            if (element == null) continue;

            var imgField = element.GetType().GetField("img");
            var idField = element.GetType().GetField("id");

            if (imgField == null || idField == null)
            {
                Debug.LogError(
                    $"Element type {element.GetType().Name} must have public fields 'img' and 'id'."
                );
                return;
            }

            int id = idPool[i];
            Color c = availableColors[id];

            var imgComponent = imgField.GetValue(element) as UnityEngine.UI.Image;
            if (imgComponent != null)
                imgComponent.color = c;

            idField.SetValue(element, id);
        }
    }
}

