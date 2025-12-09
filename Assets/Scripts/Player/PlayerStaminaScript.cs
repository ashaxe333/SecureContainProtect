using System;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStaminaScript : MonoBehaviour
{
    private GameObject player;
    private float staminaValue;
    private PlayerController playerController;

    public Slider staminaSlider;
    [SerializeField] public bool canRun = true;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerController = GetComponent<PlayerController>();
        staminaSlider.value = staminaValue;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(canRun);
        UseStamina();
    }

    /// <summary>
    /// Method 
    /// </summary>
    public void UseStamina()
    {
        if (playerController.movement == 2.0f)
        {
            if (staminaValue > 0)
            {
                canRun = true;
                staminaValue -= Time.deltaTime * 5;
            }
            else
            {
                canRun = false;
                staminaValue = 0;
            }
        }
        else
        {
            canRun = true;
            if (staminaValue < 100) staminaValue += Time.deltaTime * 4;
            else staminaValue = 100;
        }

        staminaSlider.value = staminaValue;
    }

}
