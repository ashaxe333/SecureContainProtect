using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuScript : MonoBehaviour
{
    bool isPaused = false;
    public GameObject pausePanel;

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1;
        pausePanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    /// <summary>
    /// Pøeruší hru a otevøe pause menu
    /// </summary>
    public void PauseGame()
    {
        isPaused = true;
        Time.timeScale = 0;
        Cursor.lockState = CursorLockMode.None;
        pausePanel.SetActive(true);
    }

    /// <summary>
    /// Opìt spustí hru
    /// </summary>
    public void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.Locked;
        pausePanel.SetActive(false);
    }

    /// <summary>
    /// Vratí hráèe do menu
    /// </summary>
    public void BackToMenu()
    {
        SceneManager.LoadScene(0);
    }
}
