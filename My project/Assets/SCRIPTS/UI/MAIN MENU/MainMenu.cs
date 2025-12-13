using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    public GameObject settingsPanel;
    public GameObject crreditsPanel;

    public void StartGame()
    {
        SceneManager.LoadScene("Tutorial");
    }

    public void Setting()
    {
        settingsPanel.SetActive(true);
    }

    public void Close()
    {
        settingsPanel.SetActive(false);
    }

    public void Credits()
    {
        crreditsPanel.SetActive(true);
    }
    public void CloseCredits()
    {
        crreditsPanel.SetActive(false);
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
