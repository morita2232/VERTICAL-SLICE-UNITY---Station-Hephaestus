using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class ConduitTube : MonoBehaviour
{
    [Header("Charge Settings")]
    public float chargeSpeed = 0.5f;   // how fast it fills when active
    public float drainSpeed = 0.2f;    // how fast it drains when inactive
    public Vector2 targetRange = new Vector2(0.3f, 0.7f); // where red zone can spawn

    [Header("Debug (read-only)")]
    [Range(0f, 1f)] public float targetMin;
    [Range(0f, 1f)] public float targetMax;
    [Range(0f, 1f)] public float currentFill;

    public bool isActive { get; private set; }

    Renderer rend;
    Material matInstance;

    public bool InTarget =>
        currentFill >= targetMin && currentFill <= targetMax;

    void Awake()
    {
        rend = GetComponent<Renderer>();
        matInstance = rend.material;

        // random red zone within the allowed range
        float center = Random.Range(targetRange.x, targetRange.y);
        float halfSize = 0.05f;
        targetMin = Mathf.Clamp01(center - halfSize);
        targetMax = Mathf.Clamp01(center + halfSize);

        matInstance.SetFloat("_TargetMin", targetMin);
        matInstance.SetFloat("_TargetMax", targetMax);

        currentFill = 0f;
        matInstance.SetFloat("_Fill", currentFill);

        // default fill color
        matInstance.SetColor("_FillColor", Color.cyan);
    }

    void Update()
    {
        float dt = Time.deltaTime;

        if (isActive)
        {
            // grow up
            currentFill += chargeSpeed * dt;
            currentFill = Mathf.Clamp01(currentFill);
        }
        else
        {
            // drain down
            if (currentFill > 0f)
            {
                currentFill -= drainSpeed * dt;
                if (currentFill < 0f) currentFill = 0f;
            }
        }

        matInstance.SetFloat("_Fill", currentFill);

        if (InTarget)
            matInstance.SetColor("_FillColor", Color.green);
        else
            matInstance.SetColor("_FillColor", Color.cyan);
    }

    public void SetActive(bool value)
    {
        isActive = value;
    }

    public void ResetTube()
    {
        isActive = false;
        currentFill = 0f;
        matInstance.SetFloat("_Fill", currentFill);
        matInstance.SetColor("_FillColor", Color.cyan);
    }
}

