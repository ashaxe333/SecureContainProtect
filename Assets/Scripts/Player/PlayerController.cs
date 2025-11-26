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

	private CharacterController controller;
	private Vector3 moveDirection = Vector3.zero;
	public float mouseSensitivity = 100.0f;
	private float verticalRotation = 0.0f;
    private GameObject gameManager;

    //s2
    public GameObject scp939_1;
    public GameObject scp939_2;
    public GameObject scp939_3;
    public GameObject clickedObject;

    void Start()
	{
		Cursor.lockState = CursorLockMode.Locked; // skryje kurzor myöi
        gameManager = GameObject.FindGameObjectWithTag("GameManager");

        controller = GetComponent<CharacterController>();
		if(controller is null)
		{
			Debug.LogError("CharacterController");
		}
	}

	void Update()
	{
        Move();
        Assign939();
    }

    /// <summary>
    /// Star· se o pohyb a kontroluje, jak se hr·Ë pohybuje (bÏh·, jde, ...). Informaci pak posÌl· to do scriptu SCP939Script
    /// </summary>
    public void Move()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        // predelat na int
        float result = 0.0f;

        if (controller.isGrounded)
        {
            moveDirection = new Vector3(horizontal, 0, vertical);
            moveDirection = transform.TransformDirection(moveDirection);

            if (Input.GetKey(KeyCode.LeftShift) && vertical > 0)    // w
            {
                moveDirection *= runSpeed;
                result = 2.0f;
            }
            else if (Input.GetKey(KeyCode.X) && (horizontal != 0 || vertical != 0))     // w, s, a, d
            {
                moveDirection *= sneakSpeed;
				result = 0.5f;
            }
            else if (horizontal != 0 || vertical != 0)  // w, s, a, d
            {
                moveDirection *= moveSpeed;
                result = 1.0f;
            }
            else
            {
                moveDirection *=  moveSpeed;
                result = 0.0f;
            }

            if (scp939_1 != null && scp939_2 != null && scp939_3 != null)
            {
                scp939_1.GetComponent<SCP939Script>().MoveTrigger(result);
                scp939_2.GetComponent<SCP939Script>().MoveTrigger(result);
                scp939_3.GetComponent<SCP939Script>().MoveTrigger(result);
            }
        }

        moveDirection.y -= gravity * Time.deltaTime;
        controller.Move(moveDirection * Time.deltaTime);

        // ovl·d·nÌ kamery pomocÌ myöi
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        verticalRotation -= mouseY;
        verticalRotation = Mathf.Clamp(verticalRotation, -90.0f, 90.0f);

        transform.Rotate(Vector3.up * mouseX);
        Camera.main.transform.localRotation = Quaternion.Euler(verticalRotation, 0.0f, 0.0f);
    }

    /// <summary>
    /// inicializuje vöechny scp-939 po p¯echodu do patra F0
    /// </summary>
    private void Assign939()    //vy¯eöit tak, abych nemusel assignvat tyto scp. Kaûd˝ zvl·öù by si mÏl vyt·hnout z hr·Ëe informaci o tom, jak chodÌ
    {
        if (GameManagerScript.gameManagerInstance.floor == 0)
        {
            scp939_1 = GameObject.FindGameObjectWithTag("939_1");
            scp939_2 = GameObject.FindGameObjectWithTag("939_2");
            scp939_3 = GameObject.FindGameObjectWithTag("939_3");
        }
        else
        {
            scp939_1 = null;
            scp939_2 = null;
            scp939_3 = null;
        }
    }
}