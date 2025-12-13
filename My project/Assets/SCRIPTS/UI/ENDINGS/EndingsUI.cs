using UnityEngine;
using UnityEngine.SceneManagement;

public class EndingsUI : MonoBehaviour
{

    public void Start()
    {
        Cursor.lockState = CursorLockMode.None;
    }
    public void MainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
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
