using UnityEngine;
using UnityEngine.SceneManagement;

public class Pause : MonoBehaviour
{
    public GameObject pauseMenuUI;
    public GameObject settingsMenuUI;
    public GameObject textUI;
    public static bool isPaused = false;

    public Movement playerMovement;
    public HorizontalMovement horizontalLook;
    public RotacionVertical verticalLook;
    void Awake()
    {
        pauseMenuUI.SetActive(isPaused);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    
        

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                settingsMenuUI.SetActive(false);
                Resume();
            }
            else
            {
                textUI.SetActive(false);
                PauseGame();
            }
        }


    }

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

    public void MainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    public void OpenSettingsPannel()
    {

        settingsMenuUI.SetActive(true);

    }

    public void ClosePannel()
    {

        settingsMenuUI.SetActive(false);

    }

    public void RestartLevel()
    {
        isPaused = false;
        Time.timeScale = 1f;
        pauseMenuUI.SetActive(isPaused);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void ExitGame()
    {
        #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;   // stop play mode in editor
        #else
                    Application.Quit();                                // close the game in a build
        #endif
    }

}
