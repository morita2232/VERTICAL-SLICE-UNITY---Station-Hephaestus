using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class ConduitTube : MonoBehaviour
{
    [Header("Charge Settings")]
    public float chargeSpeed = 0.5f;       // how fast the fill rises
    public Vector2 targetRange = new Vector2(0.3f, 0.7f); // random range bounds

    [Header("Debug (read-only)")]
    [Range(0f, 1f)] public float targetMin;
    [Range(0f, 1f)] public float targetMax;
    [Range(0f, 1f)] public float currentFill;

    public bool isLocked { get; private set; }
    public bool isCharging { get; private set; }

    Renderer rend;
    Material matInstance;

    public bool InTarget =>
        currentFill >= targetMin && currentFill <= targetMax;

    void Awake()
    {
        rend = GetComponent<Renderer>();
        matInstance = rend.material;

        // pick a random target window within targetRange
        float center = Random.Range(targetRange.x, targetRange.y);
        float halfSize = 0.05f; // thickness of red zone
        targetMin = Mathf.Clamp01(center - halfSize);
        targetMax = Mathf.Clamp01(center + halfSize);

        // push to material
        matInstance.SetFloat("_TargetMin", targetMin);
        matInstance.SetFloat("_TargetMax", targetMax);

        currentFill = 0f;
        matInstance.SetFloat("_Fill", currentFill);
    }

    void Update()
    {
        if (!isCharging || isLocked)
            return;

        // grow the fill upward
        currentFill += chargeSpeed * Time.deltaTime;
        currentFill = Mathf.Clamp01(currentFill);

        matInstance.SetFloat("_Fill", currentFill);

        // success: we enter the red zone
        if (InTarget)
        {
            LockSuccess();
        }
        // failure: we reached the top and never passed through the red zone
        else if (currentFill >= 1f)
        {
            FailAndReset();
        }
    }

    public void StartCharge()
    {
        if (isLocked) return; // already solved

        isCharging = true;
        currentFill = 0f;
        matInstance.SetFloat("_Fill", currentFill);

        // make fill blue/green at start
        matInstance.SetColor("_FillColor", Color.cyan);
    }

    void LockSuccess()
    {
        isLocked = true;
        isCharging = false;

        // set fill to the middle of the red zone
        currentFill = (targetMin + targetMax) * 0.5f;
        matInstance.SetFloat("_Fill", currentFill);

        // turn fill green to show success
        matInstance.SetColor("_FillColor", Color.green);
    }

    void FailAndReset()
    {
        isCharging = false;
        currentFill = 0f;
        matInstance.SetFloat("_Fill", currentFill);

        // optional: flash red, play error sound, etc.
        // matInstance.SetColor("_FillColor", Color.red);
    }

    public void ResetTube()
    {
        isLocked = false;
        isCharging = false;
        currentFill = 0f;
        matInstance.SetFloat("_Fill", currentFill);
    }
}
