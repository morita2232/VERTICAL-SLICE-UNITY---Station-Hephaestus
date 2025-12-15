using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Handles main menu navigation:
/// - Starting the game
/// - Opening / closing settings
/// - Opening / closing credits
/// - Exiting the application
/// </summary>
public class MainMenu : MonoBehaviour
{
    // ================================
    // UI Panels
    // ================================

    public GameObject settingsPanel;   // Settings menu panel
    public GameObject crreditsPanel;   // Credits menu panel


    // ================================
    // Menu Actions
    // ================================

    /// <summary>
    /// Starts the game by loading the Tutorial scene
    /// </summary>
    public void StartGame()
    {
        SceneManager.LoadScene("Tutorial");
    }

    /// <summary>
    /// Opens the settings panel
    /// </summary>
    public void Setting()
    {
        settingsPanel.SetActive(true);
    }

    /// <summary>
    /// Closes the settings panel
    /// </summary>
    public void Close()
    {
        settingsPanel.SetActive(false);
    }

    /// <summary>
    /// Opens the credits panel
    /// </summary>
    public void Credits()
    {
        crreditsPanel.SetActive(true);
    }

    /// <summary>
    /// Closes the credits panel
    /// </summary>
    public void CloseCredits()
    {
        crreditsPanel.SetActive(false);
    }

    /// <summary>
    /// Exits the game (Editor or Build)
    /// </summary>
    public void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;   // Stop play mode in editor
#else
        Application.Quit();                                // Close the game in a build
#endif
    }
}

