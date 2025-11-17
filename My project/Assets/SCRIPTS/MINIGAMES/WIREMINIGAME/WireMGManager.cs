using System.Collections.Generic;
using UnityEngine;

public class WireMGManager : MonoBehaviour
{
    public GameObject panelRoot;
    public List<Color> availableColors;

    public InteractLocator playerInteractLocator;
    public Movement playerMovement;
    public HorizontalMovement playerHorizontalMovement;
    public RotacionVertical playerVerticalMovement;

    public List<WireStart> wireStart;
    public List<WireEnd> wireEnd;

    // pairing: for each start, store its matching end
    private Dictionary<WireStart, WireEnd> pairings = new Dictionary<WireStart, WireEnd>();

    // buffer in pixels for snapping/connection
    public float snapBufferPixels = 30f;

    void Start()
    {
        // validations
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

        // assign ids/colors (your existing method) - keep as is
        AssignUniqueIdsForList(wireStart);
        AssignUniqueIdsForList(wireEnd);

        // build one-to-one pairing: each start matched to the first end with same id
        pairings.Clear();
        foreach (var start in wireStart)
        {
            if (start == null) continue;
            WireEnd match = wireEnd.Find(e => e != null && e.id == start.id);
            if (match != null)
            {
                pairings[start] = match;
                Debug.Log($"{start.gameObject.name} paired to {match.gameObject.name} (id={start.id})");
            }
            else
            {
                Debug.LogWarning($"No matching WireEnd for {start.gameObject.name} (id={start.id})");
            }
        }
    }

    void Update()
    {
        // If not in minigame, skip checks
        if (!playerInteractLocator.isInminigame) return;

        // disable player controls while in minigame
        playerMovement.canMove = false;
        playerHorizontalMovement.canMove = false;
        playerVerticalMovement.canRotate = false;
        Cursor.lockState = CursorLockMode.None;

        // check every pairing for connection
        foreach (var kv in pairings)
        {
            WireStart start = kv.Key;
            WireEnd end = kv.Value;

            if (start == null || end == null) continue;
            if (end.connected) continue; // already done

            // get end screen pos (works with UI canvases)
            Vector2 endScreenPos = RectTransformUtility.WorldToScreenPoint(
                end.GetComponentInParent<Canvas>() != null ? end.GetComponentInParent<Canvas>().worldCamera : null,
                end.transform.position
            );

            // if start.screenPos is zero (not being dragged yet), use its current screen position as fallback
            Vector2 startScreenPos = start.screenPos;
            if (startScreenPos == Vector2.zero)
            {
                startScreenPos = RectTransformUtility.WorldToScreenPoint(
                    start.GetComponentInParent<Canvas>() != null ? start.GetComponentInParent<Canvas>().worldCamera : null,
                    start.transform.position
                );
            }

            // distance compare in pixels
            float dist = Vector2.Distance(startScreenPos, endScreenPos);
            if (dist <= snapBufferPixels)
            {
                // connect!
                end.connected = true;
                Debug.Log($"Connected {start.gameObject.name} -> {end.gameObject.name} (dist={dist})");

                // optional: snap visually then destroy start
                start.transform.position = end.transform.position;
                start.gameObject.SetActive(false);

                // update remaining count or do any completion checks here
            }
        }

        // After checking pairings you can compute remaining ends and handle minigame end:
        int remaining = 0;
        for (int i = 0; i < wireEnd.Count; i++) if (!wireEnd[i].connected) remaining++;

        Debug.Log("Wires left to connect: " + remaining);

        if (remaining == 0)
        {
            // end minigame
            playerInteractLocator.isInminigame = false;
            playerMovement.canMove = true;
            playerHorizontalMovement.canMove = true;
            playerVerticalMovement.canRotate = true;
            Cursor.lockState = CursorLockMode.Locked;
            panelRoot.SetActive(false);
        }
    }

    // --- keep your existing AssignUniqueIdsForList<T> implementation here ---
    void AssignUniqueIdsForList<T>(List<T> list) where T : class
    {
        if (list == null || list.Count == 0) return;

        int poolSize = availableColors.Count;
        int needed = Mathf.Min(list.Count, poolSize);

        // Build id pool
        List<int> idPool = new List<int>(poolSize);
        for (int i = 0; i < poolSize; i++) idPool.Add(i);

        // Shuffle
        for (int i = 0; i < idPool.Count; i++)
        {
            int j = Random.Range(i, idPool.Count);
            int tmp = idPool[i];
            idPool[i] = idPool[j];
            idPool[j] = tmp;
        }

        // Assign to provided list via reflection (keeps compatibility)
        for (int i = 0; i < needed; i++)
        {
            var element = list[i];
            if (element == null) continue;

            var imgField = element.GetType().GetField("img");
            var idField = element.GetType().GetField("id");

            if (imgField == null || idField == null)
            {
                Debug.LogError($"Element type {element.GetType().Name} must have public fields 'img' and 'id'.");
                return;
            }

            int id = idPool[i];
            Color c = availableColors[id];

            var imgComponent = imgField.GetValue(element) as UnityEngine.UI.Image;
            if (imgComponent != null) imgComponent.color = c;

            idField.SetValue(element, id);
            Debug.Log($"{element.GetType().Name}[{i}] assigned id={id}");
        }
    }
}


