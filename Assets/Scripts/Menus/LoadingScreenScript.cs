using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingScreenScript : MonoBehaviour
{
    public GameObject LoadingScreen;
    public Slider LoadingBar;

    public void LoadScene(int levelToLoad)
    {
        LoadingBar.value = 0;
        LoadingScreen.SetActive(true);
        StartCoroutine(LoadAsyncScreen(levelToLoad));
    }

    IEnumerator LoadAsyncScreen(int levelToLoad)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(levelToLoad);
        operation.allowSceneActivation = false;
        float progressValue = 0;

        while (!operation.isDone)
        {
            progressValue = Mathf.MoveTowards(progressValue, operation.progress, Time.deltaTime);
            LoadingBar.value = progressValue;
            Debug.Log(progressValue);

            if (progressValue >= 0.9f)
            {
                progressValue = 1;
                operation.allowSceneActivation = true;
            }
            yield return null;
        }
    }

    /*
     public void LoadScene(int levelToLoad)
    {
        LoadingScreen.SetActive(true);
        StartCoroutine(LoadAsyncScreen(levelToLoad));
    }

    IEnumerator LoadAsyncScreen(int levelToLoad)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(levelToLoad);
        
        while (!operation.isDone)
        {
            float progressValue = Mathf.Clamp01(operation.progress / 0.9f);
            Debug.Log(progressValue);
            LoadingBar.value = progressValue;
            yield return null;
        }
    }
     */
}
