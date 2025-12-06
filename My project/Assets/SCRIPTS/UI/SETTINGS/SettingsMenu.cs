using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class SettingsMenu : MonoBehaviour
{
    [Header("UI")]
    public Slider mouseSensitivitySlider;
    public Slider musicSlider;
    public Slider sfxSlider;

    [Header("Audio")]
    public AudioMixer mainMixer;

    const string PREF_MOUSE = "MouseSensitivity";
    const string PREF_MUSIC = "MusicVolume";
    const string PREF_SFX = "SFXVolume";

    const string MIXER_MUSIC = "MusicVolume";
    const string MIXER_SFX = "SFXVolume";

    void Awake()
    {
        // ----- Load Mouse Sensitivity -----
        float mouse = PlayerPrefs.GetFloat(PREF_MOUSE, 5.0f);
        mouseSensitivitySlider.SetValueWithoutNotify(mouse);

        // ----- Load Music -----
        float music = PlayerPrefs.GetFloat(PREF_MUSIC, 0.8f);
        musicSlider.SetValueWithoutNotify(music);
        ApplyMusic(music);

        // ----- Load SFX -----
        float sfx = PlayerPrefs.GetFloat(PREF_SFX, 0.8f);
        sfxSlider.SetValueWithoutNotify(sfx);
        ApplySFX(sfx);
    }

    // ====== CALLBACKS ======

    public void OnMouseSensitivityChanged(float value)
    {
        PlayerPrefs.SetFloat(PREF_MOUSE, value);
        PlayerPrefs.Save();
    }

    public void OnMusicVolumeChanged(float value)
    {
        ApplyMusic(value);
        PlayerPrefs.SetFloat(PREF_MUSIC, value);
        PlayerPrefs.Save();
    }

    public void OnSFXVolumeChanged(float value)
    {
        ApplySFX(value);
        PlayerPrefs.SetFloat(PREF_SFX, value);
        PlayerPrefs.Save();
    }

    // ====== APPLY TO MIXER ======

    private void ApplyMusic(float value)
    {
        float dB = Mathf.Log10(Mathf.Clamp(value, 0.0001f, 1f)) * 20f;
        mainMixer.SetFloat(MIXER_MUSIC, dB);
    }

    private void ApplySFX(float value)
    {
        float dB = Mathf.Log10(Mathf.Clamp(value, 0.0001f, 1f)) * 20f;
        mainMixer.SetFloat(MIXER_SFX, dB);
    }
}





