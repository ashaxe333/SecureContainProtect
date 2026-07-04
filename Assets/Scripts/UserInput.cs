using UnityEngine;
using UnityEngine.InputSystem;

public class UserInput : MonoBehaviour
{
    public static UserInput Instance {  get; private set; }

    public Vector2 MoveInput { get; private set; }
    public bool JumpInput { get; private set; }
    public bool CrawlInput { get; private set; }
    public bool SprintInput { get; private set; }
    public bool BlinkInput { get; private set; }
    public bool CollectInput { get; private set; }
    public bool InteractInput { get; private set; }
    public bool MenuOpenCloseInput { get; private set; }
    public bool InventoryOpenCloseInput { get; private set; }

    private PlayerInput playerInput;

    private InputAction moveAction;
    private InputAction jumpAction;
    private InputAction crawlAction;
    private InputAction sprintAction;
    private InputAction blinkAction;
    private InputAction collectAction;
    private InputAction interactAction;
    private InputAction menuOpenCloseAction;
    private InputAction inventoryOpenCloseAction;
    
    void Awake()
    {
        if (Instance == null)
            Instance = this;

        playerInput = GetComponent<PlayerInput>();
        SetupInputActions();
    }

    void Update()
    {
        UpdateInputs();
    }

    private void SetupInputActions()
    {
        moveAction = playerInput.actions["Move"];
        jumpAction = playerInput.actions["Jump"];
        crawlAction = playerInput.actions["Crawl"];
        sprintAction = playerInput.actions["Sprint"];
        blinkAction = playerInput.actions["Blink"];
        collectAction = playerInput.actions["Collect"];
        interactAction = playerInput.actions["Interact"];
        menuOpenCloseAction = playerInput.actions["MenuOpenCloseAction"];
        inventoryOpenCloseAction = playerInput.actions["InventoryOpenCloseAction"];
    }

    private void UpdateInputs()
    {
        MoveInput = moveAction.ReadValue<Vector2>();
        JumpInput = jumpAction.WasPressedThisFrame();
        CrawlInput = crawlAction.WasPressedThisFrame();
        SprintInput = sprintAction.WasPressedThisFrame();
        BlinkInput = blinkAction.WasPressedThisFrame();
        CollectInput = collectAction.WasPressedThisFrame();
        InteractInput = interactAction.WasPressedThisFrame();
        MenuOpenCloseInput = menuOpenCloseAction.WasPressedThisFrame();
        InventoryOpenCloseInput = inventoryOpenCloseAction.WasPressedThisFrame();
    }
}
