using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class LevelChanger : MonoBehaviour
{
    public Animator animator;
    private int levelToLoad;
    public GameObject endGameCanvas;
    //public Text findText;
    public TextMeshProUGUI changeText;//this will need to be changed if Arik doesn't use TMP
    string winText = "Good Job! You saved the day!";
    string dedText = "Aww, too bad. Good luck next time.";

    private void Start()
    {
        endGameCanvas.SetActive(false);
    }

    public void PlayGame()
    {
        FadeToLevel(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void QuitGame()
    {
        Debug.Log("Quit");
        Application.Quit();
    }

    public void FadeToLevel(int levelIndex)
    {
        levelToLoad = levelIndex;
        animator.SetTrigger("FadeOut");
    }

    public void OnFadeComplete()
    {
        SceneManager.LoadScene(levelToLoad);
    }
    /// <summary>
    /// these are mine, and we can progress if Arik gets this far
    /// </summary>
    public void winClick()
    {
        endGameCanvas.SetActive(true);
        //findText.text = winText;
        changeText.text = winText;
        StartCoroutine(Waiter());
    }

    public void onDead()
    {
        endGameCanvas.SetActive(true);
        //findText.text = dedText;
        changeText.text = dedText;
        StartCoroutine(Waiter());
    }


    private IEnumerator Waiter()
    {
        yield return new WaitForSeconds(3);
        FadeToLevel(2);
    }
    
}
