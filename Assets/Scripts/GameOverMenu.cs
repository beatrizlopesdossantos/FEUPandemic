using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOverMenu : MonoBehaviour
{
    [SerializeField] private Slider slider;

    public void RestartGame()
    {
        StartCoroutine(LoadScene());
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    IEnumerator LoadScene()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex - 1);

        slider.gameObject.SetActive(true);
        GameObject.Find("RestartButton").SetActive(false);
        GameObject.Find("MainButton").SetActive(false);
        GameObject.Find("ExitButton").SetActive(false);

        while (!asyncLoad.isDone)
        {
            slider.value = asyncLoad.progress * 100;
            yield return null;
        }
    }
}
