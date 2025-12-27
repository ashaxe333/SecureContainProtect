using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class InventoryScript : MonoBehaviour
{
    public List<GameObject> slots;
    public List<InventorySlot> slotScripts;
    public ItemData usingHeadItem;
    public ItemData usingHandItem;
    private bool show = true;
    public GameObject inventory;

    void Start()
    {
        inventory.SetActive(true);
        inventory.SetActive(false);

        Debug.Log("InventoryScript instance: " + gameObject.name);
        Debug.Log("Slots count: " + slotScripts.Count);

        GetScripts();

    }

    void Update()
    {
        ShowInventory();
    }

    public void GetScripts()
    {
        foreach (GameObject slot in slots)
        {
            slotScripts.Add(slot.GetComponent<InventorySlot>());
        }
    }

    public void ShowInventory()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            bool isActive = !inventory.activeSelf;
            inventory.SetActive(isActive);

            if (isActive)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }
    }

    public void Add(ItemData collected)
    {
        foreach (InventorySlot slot in slotScripts)
        {
            if (!slot.used)
            {
                InventoryItem invItem = new InventoryItem(collected);

                slot.inventoryItem = invItem;
                slot.sprite = collected.sprite;
                slot.ShowSprite();
                show = false;
                return;
            }
        }
        if (show)
        {
            GameManagerScript.gameManagerInstance.SetTextInfo("Inventory is full");
            show = true;
        }
    }

    public void Remove(InventoryItem inventoryItem)
    {
        foreach (InventorySlot slot in slotScripts)
        {
            if (slot.inventoryItem == inventoryItem)
            {
                slot.RemoveSprite();
                return;
            }
        }
    }

    public bool HasKeyCard(int requiredCardLevel)
    {
        foreach (InventorySlot slot in slotScripts)
        {
            if (slot.inventoryItem != null && slot.inventoryItem.itemData is CardData card && card.level >= requiredCardLevel)
            {
                return true;
            }
            /*
             lehèí zápis:

            if (item.itemData is KeyCardData)
            {
                KeyCardData keyCard = (KeyCardData)item.itemData;
                if (keyCard.level >= requiredLevel)
                {
                    return true;
                }
            }
            */
        }
        return false;
    }

    public void Use(ItemData item)
    {
        if (item.area == ItemArea.HAND)
        {
            if (usingHandItem == null)
            {
                item.Use(true);
                usingHandItem = item;
            }
            else
            {
                usingHandItem.Use(false);
                item.Use(true);
                usingHandItem = item;
            }
        }
        else if (item.area == ItemArea.HEAD)
        {
            if (usingHeadItem == null)
            {
                item.Use(true);
                usingHeadItem = item;
            }
            else
            {
                usingHeadItem.Use(false);
                item.Use(true);
                usingHeadItem = item;
            }
        }
    }

    public void Drop(InventoryItem inventoryItem, Vector3 position)
    {
        Instantiate(inventoryItem.itemData.prefab, position, Quaternion.identity);
        Remove(inventoryItem);
    }
}
