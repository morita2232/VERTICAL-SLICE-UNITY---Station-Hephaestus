using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class CleanableSurface : MonoBehaviour
{
    [Header("Shader Property")]
    public string propertyName = "Slide"; // Shader Graph reference name
    public int materialIndex = 1;         // Element 1 in your MeshRenderer

    [Header("Cleaning")]
    public float value = 0f;              // 0 = fully dirty, 1 = fully clean
    public float maxValue = 1f;
    public bool isClean = false;

    [Header("Unlock When Clean")]
    public ParticleSystem cleanParticles;


    Renderer rend;
    MaterialPropertyBlock mpb;
    int propID;

    public bool IsClean => isClean;

    void Awake()
    {
        rend = GetComponent<Renderer>();
        mpb = new MaterialPropertyBlock();
        propID = Shader.PropertyToID(propertyName);

        ApplyToMaterial();

    }

    public void Clean(float amount)
    {
        if (isClean) return;

        value = Mathf.Clamp(value + amount, 0f, maxValue);
        ApplyToMaterial();

        if (!isClean && value >= maxValue - 0.0001f)
        {
            isClean = true;

            if (cleanParticles != null)
                StartCoroutine(PlayCleanParticles());
        }
    }

    private IEnumerator PlayCleanParticles()
    {
        cleanParticles.gameObject.SetActive(true);
        cleanParticles.Play();

        yield return new WaitForSeconds(5f);

        cleanParticles.Stop();
        cleanParticles.gameObject.SetActive(false);
    }

    void ApplyToMaterial()
    {
        // Only affect this renderer + this material slot
        rend.GetPropertyBlock(mpb, materialIndex);
        mpb.SetFloat(propID, value);
        rend.SetPropertyBlock(mpb, materialIndex);
    }
}

