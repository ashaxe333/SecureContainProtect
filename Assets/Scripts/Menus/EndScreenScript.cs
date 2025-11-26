using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndScreenScript : MonoBehaviour
{
    private void Start()
    {
        Cursor.lockState = CursorLockMode.None;
    }

    /// <summary>
    /// Vratí hráèe do menu
    /// </summary>
    public void BackToMenu()
    {
        SceneManager.LoadScene(0);
    }
}
