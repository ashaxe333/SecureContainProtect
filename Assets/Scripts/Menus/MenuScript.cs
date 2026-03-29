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
    public GameObject optionMenu;
    public GameObject generalsMenu;
    public GameObject menuPanel;

    private void Start()
    {
        CursorManagerScript.Instance.ShowCursor();
        optionMenu.SetActive(false);
    }

    /// <summary>
    /// Naète hru
    /// </summary>
    public void LoadGame()
    {
        //GameManagerScript.Instance.currentFloor = 1; //nefunguje...
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
    /// Otevøe options menu
    /// </summary>
    public void OpenOptions()
    {
        optionMenu.SetActive(true);
        generalsMenu.SetActive(true);
        menuPanel.SetActive(false);
    }

    /// <summary>
    /// Zavøe options menu
    /// </summary>
    public void CloseOptions()
    {
        optionMenu.SetActive(false);
    }
}
