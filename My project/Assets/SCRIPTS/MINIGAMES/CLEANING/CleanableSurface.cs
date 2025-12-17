using System.Collections;
using UnityEngine;

/// <summary>
/// Controls a shader-based cleanable surface.
/// Increases a shader property over time until the surface is fully clean,
/// optionally playing particles when cleaning is complete.
/// </summary>
[RequireComponent(typeof(Renderer))]
public class CleanableSurface : MonoBehaviour
{
    // ================================
    // Shader Property Settings
    // ================================

    [Header("Shader Property")]

    public string propertyName = "Slide"; // Shader Graph float property name
    public int materialIndex = 1;         // Material slot on the renderer


    // ================================
    // Cleaning State
    // ================================

    [Header("Cleaning")]

    public float value = 0f;              // 0 = fully dirty, 1 = fully clean
    public float maxValue = 1f;            // Maximum clean value
    public bool isClean = false;           // Whether the surface is fully clean


    // ================================
    // Completion Effects
    // ================================

    [Header("Unlock When Clean")]

    public ParticleSystem cleanParticles;  // Particles played when cleaning finishes


    // ================================
    // Internal References
    // ================================

    Renderer rend;                         // Cached renderer
    MaterialPropertyBlock mpb;             // Property block for per-instance control
    int propID;                            // Shader property ID


    // Public read-only access to clean state
    public bool IsClean => isClean;


    void Awake()
    {
        // Cache renderer and property block
        rend = GetComponent<Renderer>();
        mpb = new MaterialPropertyBlock();
        propID = Shader.PropertyToID(propertyName);

        // Apply initial value to material
        ApplyToMaterial();
    }

    /// <summary>
    /// Increases the clean value by a given amount
    /// </summary>
    public void Clean(float amount)
    {
        if (isClean) return;

        value = Mathf.Clamp(value + amount, 0f, maxValue);
        ApplyToMaterial();

        if (!isClean && value >= maxValue - 0.1f)
        {
            isClean = true;

            if (cleanParticles != null)
                StartCoroutine(PlayCleanParticles());
        }
    }

    /// <summary>
    /// Plays particles briefly when the surface becomes clean
    /// </summary>
    private IEnumerator PlayCleanParticles()
    {
        cleanParticles.gameObject.SetActive(true);
        cleanParticles.Play();

        yield return new WaitForSeconds(5f);

        cleanParticles.Stop();
        cleanParticles.gameObject.SetActive(false);
    }

    /// <summary>
    /// Applies the current clean value to the shader material
    /// </summary>
    void ApplyToMaterial()
    {
        // Only affect this renderer and material slot
        rend.GetPropertyBlock(mpb, materialIndex);
        mpb.SetFloat(propID, value);
        rend.SetPropertyBlock(mpb, materialIndex);
    }
}


