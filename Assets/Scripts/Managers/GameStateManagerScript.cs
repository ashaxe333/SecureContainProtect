using UnityEngine;

public class GameStateManagerScript : MonoBehaviour
{
    // NÁPADY:
    // 1) Když bych mìl více stavù pøes sebe, udìlat Stack<GameState> s Push() a Pop() metodama
    public static GameStateManagerScript Instance { get; private set; }
    public GameState CurrentState { get; private set; }
    public GameState PreviousState { get; private set; }

    private GameObject player;
    private PlayerController playerController;
    private PlayerInteractScript playerInteractScript;
    private InventoryScript inventoryScript;

    public GameObject pauseMenu;
    private PauseMenuScript pauseMenuScript;
    private OptionsMenuScript optionsMenuScript;
    private LockRayScript lockRayScript;
    public bool isPaused = false;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            //DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        pauseMenuScript = gameObject.GetComponent<PauseMenuScript>();
        optionsMenuScript = gameObject.GetComponent<OptionsMenuScript>();
        playerController = player.GetComponent<PlayerController>();
        playerInteractScript = player.GetComponent<PlayerInteractScript>();
        inventoryScript = player.GetComponent<InventoryScript>();
        lockRayScript = GameManagerScript.Instance.GetComponent<LockRayScript>();

        CurrentState = GameState.GAMEPLAY;
    }

    private void Update()
    {
        TogglePauseMenu();
    }

    /// <summary>
    /// Nastavý herní režim
    /// </summary>
    /// <param name="state"></param>
    public void SetState(GameState state)
    {
        CurrentState = state;
        ApplyState();
    }

    /// <summary>
    /// Zaøídí se podle stavu hry
    /// </summary>
    public void ApplyState()
    {
        switch (CurrentState)
        {
            case GameState.GAMEPLAY:
                Time.timeScale = 1;
                EnableInput(true, true, true, true);
                CursorManagerScript.Instance.HideCursor();
                break;

            case GameState.INVENTORY:
                Time.timeScale = 1;
                EnableInput(true, false, false, true);
                CursorManagerScript.Instance.ShowCursor();
                break;

            case GameState.PAUSE:
                Time.timeScale = 0;
                EnableInput(false, false, false, false);
                CursorManagerScript.Instance.ShowCursor();
                break;

            case GameState.CUTSCENE:
                Time.timeScale = 0;
                EnableInput(false, false, false, false);
                CursorManagerScript.Instance.HideCursor();
                break;
        }
    }

    /// <summary>
    /// Urèuje, jestli se hra pøeruší, nebo zapne
    /// </summary>
    public void TogglePauseMenu()
    {
        //funguje, jen editor má v tento moment nepøíjemnou funkci - esc zviditelní kurzor, pravý klik zneviditelní
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
                ResumeGame();
            else
                PauseGame();
        }
    }

    /// <summary>
    /// Pøeruší hru a otevøe pause menu
    /// </summary>
    public void PauseGame()
    {
        isPaused = true;
        pauseMenuScript.pausePanel.SetActive(true);
        pauseMenu.SetActive(true);

        PreviousState = CurrentState; // uloží aktuální stav
        SetState(GameState.PAUSE);
    }

    /// <summary>
    /// Opìt spustí hru
    /// </summary>
    public void ResumeGame()
    {
        isPaused = false;
        pauseMenuScript.pausePanel.SetActive(false);
        pauseMenu.SetActive(false);

        pauseMenuScript.CloseOptions();
        optionsMenuScript.ShowOptionPanels(0);

        SetState(PreviousState);
    }

    /// <summary>
    /// Pøedává povolení na pohyb, ovládání kamery
    /// </summary>
    /// <param name="movementEnable"> pohyb </param>
    /// <param name="cameraEnable"> kamera </param>
    /// <param name="interactEnable"> interakce </param>
    public void EnableInput(bool movementEnable, bool cameraEnable, bool interactEnable, bool keyEnable)
    {
        if (player != null)
        {
            playerController.SetMovementEnabled(movementEnable);
            playerController.SetCameraEnabled(cameraEnable);
            inventoryScript.SetKeyEnabled(keyEnable);
            playerInteractScript.SetInteractEnable(interactEnable);
            lockRayScript.SetInteractEnable(interactEnable);
        }
    }
}
