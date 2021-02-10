using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    const float DEFAULT_LOAD_SCENE_DELAY = 0f;

    //[SerializeField] private int splashScreenScene = 0;
    //private float splashScreenDelay = 4f;
    [SerializeField] private int startSceneIndex = 0;
    private int currentSceneIndex;


    // Start is called before the first frame update
    void Start()
    {
        currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        //if (currentSceneIndex == splashScreenScene)
        //    LoadNextScene(splashScreenDelay);
    }

    public void LoadMainMenu()
    {
        //StartCoroutine(LoadScene("StartScene")); 
        StartCoroutine(LoadScene(startSceneIndex));
    }

    public void LoadOptionsScene()
    {
        //StartCoroutine(LoadScene("OptionsScene"));
    }

    public void LoadNextScene(float delay = DEFAULT_LOAD_SCENE_DELAY)
    {
        //check for for scene out of boundaries
        if (currentSceneIndex == SceneManager.sceneCountInBuildSettings - 1)
            LoadMainMenu();
        else
            StartCoroutine(LoadScene(currentSceneIndex + 1, delay));
    }

    public void RestartScene(float delay = DEFAULT_LOAD_SCENE_DELAY)
    {
        StartCoroutine(LoadScene(currentSceneIndex, delay));
    }

    public void LoadGameOverScene(float delay = DEFAULT_LOAD_SCENE_DELAY)
    {
        //StartCoroutine(LoadScene("GameOverScene", delay));
    }


    private IEnumerator LoadScene(int sceneIndex, float delay = DEFAULT_LOAD_SCENE_DELAY)
    {
        Time.timeScale = 1;
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(sceneIndex);
    }

    private IEnumerator LoadScene(string sceneName, float delay = DEFAULT_LOAD_SCENE_DELAY)
    {
        Time.timeScale = 1;
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(sceneName);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
