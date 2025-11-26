using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuScript : MonoBehaviour
{
    public GameObject playBTN;
    public GameObject exitBTN;
    public GameObject helpBTN;
    public GameObject tutorialPanel;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        tutorialPanel.SetActive(false);
    }

    /// <summary>
    /// Naète hru
    /// </summary>
    public void LoadGame()
    {
        GameManagerScript.gameManagerInstance.floor = 1; //nefunguje...
        SceneManager.LoadScene(1);
    }

    /// <summary>
    /// Opustí aplikaci
    /// </summary>
    public void QuitApp()
    {
        Debug.Log("quitting");
        Application.Quit();
    }

    /// <summary>
    /// Ukáže panel s tutoriálem
    /// </summary>
    public void ShowTutorial()
    {
        playBTN.SetActive(false);
        exitBTN.SetActive(false);
        helpBTN.SetActive(false);
        tutorialPanel.SetActive(true);
    }

    /// <summary>
    /// Vrací zpìt na menu z tutoriálu
    /// </summary>
    public void Back()
    {
        tutorialPanel.SetActive(false);
        playBTN.SetActive(true);
        exitBTN.SetActive(true);
        helpBTN.SetActive(true);
    }
}
