using UnityEngine;
using UnityEngine.SceneManagement;

public class Pause : MonoBehaviour
{
    public GameObject pauseMenuUI;
    public GameObject settingsMenuUI;
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
                Resume();
            }
            else
            {
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

    public void ExitGame()
    {
        #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;   // stop play mode in editor
        #else
                    Application.Quit();                                // close the game in a build
        #endif
    }

}
