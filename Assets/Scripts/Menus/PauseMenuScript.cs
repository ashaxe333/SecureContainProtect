using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuScript : MonoBehaviour
{
    public GameObject pauseMenu;
    public GameObject optionMenu;
    public GameObject generalsMenu;
    public GameObject pausePanel;

    // Start is called before the first frame update
    void Start()
    {
        pauseMenu.SetActive(false);
        pausePanel.SetActive(false);
    }

    /// <summary>
    /// Otevøe options menu
    /// </summary>
    public void OpenOptions()
    {
        optionMenu.SetActive(true);
        generalsMenu.SetActive(true);
        pauseMenu.SetActive(false);
    }

    /// <summary>
    /// Vratí hráèe do menu
    /// </summary>
    public void BackToMenu()
    {
        SceneManager.LoadScene(0);
    }
}