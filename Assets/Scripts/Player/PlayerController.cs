using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Authentication.ExtendedProtection;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;


public class PlayerController : MonoBehaviour
{
    // Vylepšení:
    // 1) Mít zvuky chùze po rùzných materiálech, Støilet ray na podlahu a materiál podlahy zjistit -> zapnmu zvuk pro danný povrch (Vyžaduje rozsáhlý update prefabù - oddìlit zem, stìny a strop od sebe)

    //s1
    private float moveSpeed = 8.0f;
    private float runSpeed = 14.0f;
    private float crawlSpeed = 4.0f;
	private float gravity = 20.0f;

    [HideInInspector] public PlayerMovementMode movement;

	private CharacterController controller;
	private Vector3 moveDirection = Vector3.zero;
    public Slider sensitivitySlider;
	private float mouseSensitivity;
	private float verticalRotation = 0.0f;

    [HideInInspector] public GameObject clickedObject;

    //s3
    private GameObject player;
    private PlayerStaminaScript playerStaminaScript;
    private bool movementEnabled = true;
    private bool cameraEnabled = true;
    private Vector2 moveInput;

    [Header("Footsteps")]
    [SerializeField] private AudioClip[] footsteps2Sounds;
    [SerializeField] private AudioClip[] footsteps1Sounds;
    [SerializeField] private float walkStepDelay = 0.8f;
    [SerializeField] private float runStepDelay = 0.4f;
    [SerializeField] private float crawlStepDelay = 1f;

    private float stepTimer;


    void Start()
	{
        CursorManagerScript.Instance.HideCursor();

        player = GameObject.FindGameObjectWithTag("Player");
        playerStaminaScript = player.GetComponent<PlayerStaminaScript>();

        mouseSensitivity = PlayerPrefs.GetFloat("MouseSensitivity", 100f);
        sensitivitySlider.SetValueWithoutNotify(mouseSensitivity);

        controller = GetComponent<CharacterController>();
		if(controller is null)
		{
			Debug.LogError("PlayerController: Add CharacterController!!");
		}
	}

	void Update()
	{
        if (movementEnabled)
            Move();

        if (cameraEnabled)
            LookAround();

        HandleFootsteps();
    }

    /// <summary>
    /// Stará se o pohyb a kontroluje, jak se hráè pohybuje (bìhá, jde, ...). Informaci pak posílá to do scriptu SCP939Script
    /// </summary>
    public void Move()
    {
        moveInput = UserInput.Instance.MoveInput;


        float horizontal = moveInput.x;
        float vertical = moveInput.y;
        movement = PlayerMovementMode.STAND;

        bool isMoving = horizontal != 0f || vertical != 0f;

        if (controller.isGrounded)
        {
            moveDirection = new Vector3(horizontal, 0, vertical);
            moveDirection = transform.TransformDirection(moveDirection);

            if (UserInput.Instance.SprintInput && vertical > 0)    // shift + w
            {
                if (playerStaminaScript.canRun) moveDirection *= runSpeed;
                else moveDirection *= moveSpeed;
                movement = PlayerMovementMode.RUN;
                
            }
            else if (UserInput.Instance.CrawlInput && isMoving)     // c + w/s/a/d
            {
                moveDirection *= crawlSpeed;
				movement = PlayerMovementMode.CRAWL;
            }
            else if (isMoving)  // w, s, a, d
            {
                moveDirection *= moveSpeed;
                movement = PlayerMovementMode.WALK;
            }
            else
            {
                moveDirection *= moveSpeed;
                movement = PlayerMovementMode.STAND;
            }
        }

        moveDirection.y -= gravity * Time.deltaTime;
        controller.Move(moveDirection * Time.deltaTime);
    }

    /// <summary>
    /// Stará se o kameru hráèe
    /// </summary>
    public void LookAround()
    {
        // ovládání kamery pomocí myši
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        verticalRotation -= mouseY;
        verticalRotation = Mathf.Clamp(verticalRotation, -90.0f, 90.0f);

        transform.Rotate(Vector3.up * mouseX);
        Camera.main.transform.localRotation = Quaternion.Euler(verticalRotation, 0.0f, 0.0f);
    }

    /// <summary>
    /// Pøehrává zvuk krokù pøi
    /// </summary>
    void HandleFootsteps()
    {
        bool isMoving = controller.velocity.magnitude > 0.1f && controller.isGrounded;

        if (!isMoving)
        {
            stepTimer = 0f;
            return;
        }

        float delay = walkStepDelay;
        float volume = 0.2f;
        AudioClip[] footsteps = footsteps1Sounds;

        if (movement == PlayerMovementMode.RUN)
        {
            delay = runStepDelay;
            volume = 0.2f;
            footsteps = footsteps2Sounds;
        }
        else if (movement == PlayerMovementMode.CRAWL)
        {
            delay = crawlStepDelay;
            volume = 0.1f;
        }

        stepTimer -= Time.deltaTime;

        if (stepTimer <= 0f)
        {
            SoundFXManagerScript.instance.PlaySoundFX(footsteps, transform, volume, false);
            stepTimer = delay;
        }
    }

    /// <summary>
    /// Urèuje, jestli hráè bude moct chodit
    /// </summary>
    /// <param name="enabled"> true/false </param>
    public void SetMovementEnabled(bool enabled)
    {
        movementEnabled = enabled;
    }

    /// <summary>
    /// Urèuje, jestli hráè mùže ovládat kameru
    /// </summary>
    /// <param name="enabled"></param>
    public void SetCameraEnabled(bool enabled)
    {
        cameraEnabled = enabled;
    }

    /// <summary>
    /// Sets Mouse Sensitivity in-game
    /// </summary>
    /// <param name="sens"></param>
    public void SetSensitivity(float sens)
    {
        mouseSensitivity = sens;
        PlayerPrefs.SetFloat("MouseSensitivity", sens);
    }
}