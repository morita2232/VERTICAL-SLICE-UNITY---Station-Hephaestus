using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Handles UI behavior for ending screens.
/// Unlocks the cursor and provides navigation back to the main menu
/// or exiting the game.
/// </summary>
public class EndingsUI : MonoBehaviour
{
    void Start()
    {
        // Ensure cursor is visible and free on ending screens
        Cursor.lockState = CursorLockMode.None;
    }

    /// <summary>
    /// Returns to the main menu scene
    /// </summary>
    public void MainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
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
