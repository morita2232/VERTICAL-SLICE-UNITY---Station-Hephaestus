using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Handles pause menu behavior:
/// - Toggling pause state
/// - Locking / unlocking player controls
/// - Managing pause, settings, and UI visibility
/// </summary>
public class Pause : MonoBehaviour
{
    // ================================
    // UI References
    // ================================

    public GameObject pauseMenuUI;     // Main pause menu panel
    public GameObject settingsMenuUI;  // Settings sub-menu
    public GameObject textUI;           // Gameplay UI text
    public GameObject crosshair;        // Crosshair shown during gameplay


    // ================================
    // Player Control References
    // ================================

    public Movement playerMovement;
    public HorizontalMovement horizontalLook;
    public RotacionVertical verticalLook;


    // ================================
    // Pause State
    // ================================

    public static bool isPaused = false;


    void Awake()
    {
        // Ensure pause menu reflects current pause state
        pauseMenuUI.SetActive(isPaused);
    }

    void Update()
    {
        // Toggle pause with ESC
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                // Resume game
                settingsMenuUI.SetActive(false);
                crosshair.SetActive(true);
                Resume();
            }
            else
            {
                // Pause game
                textUI.SetActive(false);
                crosshair.SetActive(false);
                PauseGame();
            }
        }
    }

    // ================================
    // Pause / Resume Logic
    // ================================

    /// <summary>
    /// Resumes gameplay and restores player control
    /// </summary>
    public void Resume()
    {
        Cursor.lockState = CursorLockMode.Locked;

        playerMovement.canMove = true;
        horizontalLook.canMove = true;
        verticalLook.canRotate = true;

        pauseMenuUI.SetActive(false);
        settingsMenuUI.SetActive(false);

        Time.timeScale = 1f;
        isPaused = false;
    }

    /// <summary>
    /// Pauses gameplay and disables player control
    /// </summary>
    void PauseGame()
    {
        Cursor.lockState = CursorLockMode.None;

        playerMovement.canMove = false;
        horizontalLook.canMove = false;
        verticalLook.canRotate = false;

        pauseMenuUI.SetActive(true);
        textUI.SetActive(false);

        Time.timeScale = 0f;
        isPaused = true;
    }


    // ================================
    // Menu Buttons
    // ================================

    /// <summary>
    /// Loads the main menu scene
    /// </summary>
    public void MainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    /// <summary>
    /// Opens the settings panel
    /// </summary>
    public void OpenSettingsPannel()
    {
        settingsMenuUI.SetActive(true);
    }

    /// <summary>
    /// Closes the settings panel
    /// </summary>
    public void ClosePannel()
    {
        settingsMenuUI.SetActive(false);
    }

    /// <summary>
    /// Restarts the current level
    /// </summary>
    public void RestartLevel()
    {
        isPaused = false;
        Time.timeScale = 1f;
        pauseMenuUI.SetActive(isPaused);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    /// <summary>
    /// Exits the game (editor or build)
    /// </summary>
    public void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // Stop play mode in editor
#else
        Application.Quit(); // Quit game in build
#endif
    }
}

