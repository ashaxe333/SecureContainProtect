using UnityEngine;

public class OptionsMenuScript : MonoBehaviour
{
    public GameObject pauseMenu;
    public GameObject optionMenu;
    public GameObject generalMenu;
    public GameObject controlsMenu;
    public GameObject graphicsMenu;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ShowOptionPanels(0);
    }

    /// <summary>
    /// Vrací zpìt na Game Paused menu
    /// </summary>
    public void Back()
    {
        pauseMenu.SetActive(true);
        ShowOptionPanels(0);
    }

    /// <summary>
    /// Zobrazí urèitý panel, který hráè chtìl
    /// </summary>
    /// <param name="panel"> èíslo panelu, který se má zobrazit </param>
    public void ShowOptionPanels(int panel)
    {
        switch (panel)
        {
            case 0:
                optionMenu.SetActive(false);
                generalMenu.SetActive(false);
                controlsMenu.SetActive(false);
                graphicsMenu.SetActive(false);
                break;

            case 1:
                generalMenu.SetActive(true);
                controlsMenu.SetActive(false);
                graphicsMenu.SetActive(false);
                break;

            case 2:
                generalMenu.SetActive(false);
                controlsMenu.SetActive(true);
                graphicsMenu.SetActive(false);
                break;

            case 3:
                generalMenu.SetActive(false);
                controlsMenu.SetActive(false);
                graphicsMenu.SetActive(true);
                break;
        }
    }

}
