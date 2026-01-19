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
    //s1
    private float moveSpeed = 8.0f;
    private float runSpeed = 14.0f;
    private float sneakSpeed = 2.0f;
	private float gravity = 20.0f;

    [SerializeField] public float movement;

	private CharacterController controller;
	private Vector3 moveDirection = Vector3.zero;
    public Slider sensitivitySlider;
	private float mouseSensitivity;   // do nastavení
	private float verticalRotation = 0.0f;
    private GameObject gameManager;

    [SerializeField] public GameObject clickedObject;

    //s3
    private GameObject player;
    private PlayerStaminaScript playerStaminaScript;
    private bool movementEnabled = true;
    private bool cameraEnabled = true;

    void Start()
	{
        CursorManagerScript.Instance.HideCursor();

        gameManager = GameObject.FindGameObjectWithTag("GameManager");

        player = GameObject.FindGameObjectWithTag("Player");
        playerStaminaScript = player.GetComponent<PlayerStaminaScript>();

        mouseSensitivity = PlayerPrefs.GetFloat("MouseSensitivity", 100f);
        sensitivitySlider.SetValueWithoutNotify(mouseSensitivity);

        controller = GetComponent<CharacterController>();
		if(controller is null)
		{
			Debug.LogError("CharacterController");
		}
	}

	void Update()
	{
        if (movementEnabled)
            Move();

        if (cameraEnabled)
            LookAround();
    }

    /// <summary>
    /// Stará se o pohyb a kontroluje, jak se hráè pohybuje (bìhá, jde, ...). Informaci pak posílá to do scriptu SCP939Script
    /// </summary>
    public void Move()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        movement = 0.0f;

        if (controller.isGrounded)
        {
            moveDirection = new Vector3(horizontal, 0, vertical);
            moveDirection = transform.TransformDirection(moveDirection);

            if (Input.GetKey(KeyCode.LeftShift) && vertical > 0)    // w
            {
                if (playerStaminaScript.canRun) moveDirection *= runSpeed;
                else moveDirection *= moveSpeed;
                movement = 2.0f;
                
            }
            else if (Input.GetKey(KeyCode.X) && (horizontal != 0 || vertical != 0))     // w, s, a, d
            {
                moveDirection *= sneakSpeed;
				movement = 0.5f;
            }
            else if (horizontal != 0 || vertical != 0)  // w, s, a, d
            {
                moveDirection *= moveSpeed;
                movement = 1.0f;
            }
            else
            {
                moveDirection *=  moveSpeed;
                movement = 0.0f;
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