using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

/// <summary>
/// Handles game settings UI:
/// - Mouse sensitivity
/// - Music volume
/// - SFX volume
/// Persists values using PlayerPrefs and applies audio
/// changes through an AudioMixer.
/// </summary>
public class SettingsMenu : MonoBehaviour
{
    // ================================
    // UI References
    // ================================

    [Header("UI")]

    public Slider mouseSensitivitySlider;   // Controls mouse sensitivity
    public Slider musicSlider;              // Controls music volume
    public Slider sfxSlider;                // Controls SFX volume


    // ================================
    // Audio
    // ================================

    [Header("Audio")]

    public AudioMixer mainMixer;             // Main audio mixer


    // ================================
    // PlayerPrefs Keys
    // ================================

    const string PREF_MOUSE = "MouseSensitivity";
    const string PREF_MUSIC = "MusicVolume";
    const string PREF_SFX = "SFXVolume";


    // ================================
    // AudioMixer Parameters
    // ================================

    const string MIXER_MUSIC = "MusicVolume";
    const string MIXER_SFX = "SFXVolume";


    void Awake()
    {
        // ================================
        // Load Mouse Sensitivity
        // ================================

        float mouse = PlayerPrefs.GetFloat(PREF_MOUSE, 5.0f);
        mouseSensitivitySlider.SetValueWithoutNotify(mouse);

        // ================================
        // Load Music Volume
        // ================================

        float music = PlayerPrefs.GetFloat(PREF_MUSIC, 0.8f);
        musicSlider.SetValueWithoutNotify(music);
        ApplyMusic(music);

        // ================================
        // Load SFX Volume
        // ================================

        float sfx = PlayerPrefs.GetFloat(PREF_SFX, 0.8f);
        sfxSlider.SetValueWithoutNotify(sfx);
        ApplySFX(sfx);
    }

    // ================================
    // UI Callbacks
    // ================================

    /// <summary>
    /// Called when mouse sensitivity slider changes
    /// </summary>
    public void OnMouseSensitivityChanged(float value)
    {
        PlayerPrefs.SetFloat(PREF_MOUSE, value);
        PlayerPrefs.Save();
    }

    /// <summary>
    /// Called when music volume slider changes
    /// </summary>
    public void OnMusicVolumeChanged(float value)
    {
        ApplyMusic(value);
        PlayerPrefs.SetFloat(PREF_MUSIC, value);
        PlayerPrefs.Save();
    }

    /// <summary>
    /// Called when SFX volume slider changes
    /// </summary>
    public void OnSFXVolumeChanged(float value)
    {
        ApplySFX(value);
        PlayerPrefs.SetFloat(PREF_SFX, value);
        PlayerPrefs.Save();
    }

    // ================================
    // AudioMixer Application
    // ================================

    void ApplyMusic(float value)
    {
        // Convert linear slider value to decibels
        float dB = Mathf.Log10(Mathf.Clamp(value, 0.0001f, 1f)) * 20f;
        mainMixer.SetFloat(MIXER_MUSIC, dB);
    }

    void ApplySFX(float value)
    {
        // Convert linear slider value to decibels
        float dB = Mathf.Log10(Mathf.Clamp(value, 0.0001f, 1f)) * 20f;
        mainMixer.SetFloat(MIXER_SFX, dB);
    }
}






