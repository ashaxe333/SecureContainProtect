using UnityEngine;

public class CursorManagerScript : MonoBehaviour
{
    public static CursorManagerScript Instance { get; private set; }

    /// <summary>
    /// Singleton
    /// </summary>
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Skrývá kurzor
    /// </summary>
    public void HideCursor()
    {
        Debug.Log("HIDE CURSOR CALLED", this);
        Cursor.lockState = CursorLockMode.None;    // prevence pøed bugem
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    /// <summary>
    /// Odkrývá kurzor
    /// </summary>
    public void ShowCursor()
    {
        Debug.Log("SHOW CURSOR CALLED", this);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
