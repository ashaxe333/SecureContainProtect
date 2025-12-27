using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class ItemContextMenuScript : MonoBehaviour
{
    private InventorySlot currentSlotScript;
    private InventoryScript inventoryScript;
    private GameObject player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        inventoryScript = FindAnyObjectByType<InventoryScript>();
        gameObject.SetActive(false);
    }

    public void Hide()
    {
        currentSlotScript = null;
        gameObject.SetActive(false);
    }

    public void Show(InventorySlot inventorySlot, Vector2 position)
    {
        gameObject.SetActive(true);
        gameObject.transform.position = position;
        currentSlotScript = inventorySlot;
    }

    public void OnDrop()
    {
        if (currentSlotScript == null) return;

        inventoryScript.Drop(currentSlotScript.inventoryItem, player.transform.position + player.transform.forward);
        Hide();
    }

    public void OnUse()
    {
        if (currentSlotScript == null) return;

        inventoryScript.Use(currentSlotScript.inventoryItem.itemData);
        Hide();
    }
}
