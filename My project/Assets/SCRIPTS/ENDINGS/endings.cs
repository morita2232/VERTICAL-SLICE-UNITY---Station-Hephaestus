using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;


public class endings : MonoBehaviour
{
    public InteractLocator interactLocator;
    public GameObject player;
    public Transform returnPosition;
    public TMP_FontAsset sammyFont;
    public Sprite sammyPortrait;
    public TMP_FontAsset translationFont;    // normal readable font
    public Sprite alienPortrait;
    public GameObject alien;
    public GameObject exes;

    public void EndingOne()
    {
        player.transform.position = returnPosition.position;
        interactLocator.isInSpaceShip = false;

        DialogueManager.Instance.SayLines(
                   "Spammy Sammy",
                   new string[]
                   {
                        "Uhm,buddy?, why is the ship leaving?",
                        "What did you do?",
                        "I thought we were friends...",
                        "The company is not going to be happy about this..."
                   }, sammyFont, sammyPortrait);
        StartCoroutine(LoadNextSceneAfterDelay(5f, true));

    }

   public void EndingTwo()
    {
        DialogueManager.OnDialogueSequenceFinished += LayDown;

        DialogueManager.Instance.SayLines(
                   "Alien",
                   new string[]
                   {
                        "I understand...",
                        "You had to make a choice.",
                        "I hope you find peace with it.",
                        "Farewell, human."
                   }, translationFont, alienPortrait);
    }

    void LayDown()
    {
        DialogueManager.OnDialogueSequenceFinished -= LayDown;
        alien.transform.rotation = Quaternion.Euler(-90f, 0f, 0f);
        alien.transform.position = new Vector3(85.25f, 0.402f, 22.33f);
        exes.SetActive(true);
        StartCoroutine(LoadNextSceneAfterDelay(5f, false));

    }

    IEnumerator LoadNextSceneAfterDelay(float delay, bool ending)
    {
        yield return new WaitForSeconds(delay);
        
        if(ending)
            SceneManager.LoadScene("GoodEnding");
        else
            SceneManager.LoadScene("BadEnding");
    }

}
