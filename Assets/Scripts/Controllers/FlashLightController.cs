using System;
using Unity.VisualScripting;
using UnityEngine;

public class FlashLightController : MonoBehaviour
{
    public Transform player;
    public static FlashLightController Instance;
    public InventoryScript inventoryScript;
    public bool isActive;
    public Light spot;
    [SerializeField] private AudioClip[] lightSwitchClicks;

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

        isActive = false;
        spot.enabled = false;
    }

    /// <summary>
    /// Vypíná efekt baterky
    /// </summary>
    /// <param name="active"> bool </param>
    public void SetLightActive(bool active)
    {
        if (inventoryScript.IsFlashLightActive())
        {
            SoundFXManagerScript.instance.PlaySoundFX(lightSwitchClicks, player, 0.08f, false);
            isActive = active;
            spot.enabled = active;
        }
        return;
    }
}
