using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.Mesh;

public class InventoryScript : MonoBehaviour
{
    // ÚKOLY

    public List<GameObject> slots;
    public List<InventorySlot> slotScripts;

    public EquipmentSlotScript activeHeadSlot;
    public EquipmentSlotScript activeHandSlot;

    // smazat
    //public ItemData usingHeadItem;
    //public ItemData usingHandItem;

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

    /// <summary>
    /// Naplní list se skripty jednotlivých slotù (aby se pøedešlo opakovanému GetComponent v jiných èástech skriptu)
    /// </summary>
    public void GetScripts()
    {
        foreach (GameObject slot in slots)
        {
            slotScripts.Add(slot.GetComponent<InventorySlot>());
        }
    }

    /// <summary>
    /// Zobrazuje/skrývá inventáø
    /// </summary>
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
                foreach(InventorySlot slot in slotScripts)
                {
                    slot.contextMenuScript.Hide();
                }
            }
        }
    }

    /// <summary>
    /// Pøidává item do inventáøe
    /// </summary>
    /// <param name="collected"> sebraný item </param>
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

    /// <summary>
    /// Odstraòuje z inventáøe
    /// </summary>
    /// <param name="inventoryItem"></param>
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

    /// <summary>
    /// Controluje, jestli je aktivní karta
    /// </summary>
    /// <param name="requiredCardLevel"> chtìný level karty </param>
    /// <returns> true/false </returns>
    public bool IsKeyCardActive(int requiredCardLevel)
    {
        if (activeHandSlot.inventoryItem.itemData is CardData card && card.level >= requiredCardLevel) return true;
        return false;
    }

    public bool IsMaskActive()
    {
        if(activeHeadSlot.inventoryItem.itemData is MaskData) return true;
        return false;
    }
    public bool IsNightVisionActive()
    {
        if (activeHeadSlot.inventoryItem.itemData is NightVisionData) return true;
        return false;
    }

    public void SetEquippedItem(InventoryItem inventoryItem)
    {

    }

    public void Equip(InventoryItem inventoryItem)
    {
        if (inventoryItem.itemData.area == ItemArea.HAND)
        {
            if (activeHandSlot.inventoryItem == inventoryItem) return;

            if (!activeHandSlot.used)
            {
                activeHandSlot.used = true;
                activeHandSlot.inventoryItem = inventoryItem;
                inventoryItem.itemData.Equip(true);
                Remove(inventoryItem);
            }
            else
            {
                activeHandSlot.inventoryItem.itemData.Equip(false);
                Add(activeHandSlot.inventoryItem.itemData);
                inventoryItem.itemData.Equip(true);
                activeHandSlot.inventoryItem = inventoryItem;
                Remove(inventoryItem);
            }
        }
        else if (inventoryItem.itemData.area == ItemArea.HEAD)
        {
            if (activeHeadSlot.inventoryItem == inventoryItem) return;

            if (!activeHeadSlot.used)
            {
                activeHeadSlot.used = true;
                activeHeadSlot.inventoryItem = inventoryItem;
                inventoryItem.itemData.Equip(true);
                Remove(inventoryItem);
            }
            else
            {
                activeHeadSlot.inventoryItem.itemData.Equip(false);
                Add(activeHeadSlot.inventoryItem.itemData);
                inventoryItem.itemData.Equip(true);
                activeHeadSlot.inventoryItem = inventoryItem;
                Remove(inventoryItem);
            }
        }
    }

    /// <summary>
    /// Spawnuje vyhozený item zpìt do svìta
    /// </summary>
    /// <param name="inventoryItem"> vyhozený item </param>
    /// <param name="position"> kde se item objeví </param>
    public void Drop(InventoryItem inventoryItem, Vector3 position)
    {
        ItemData item = inventoryItem.itemData;
        if (activeHandSlot.inventoryItem.itemData == item)
        {
            inventoryItem.itemData.Equip(false);
            activeHandSlot.inventoryItem.itemData = null;
        }
        if(activeHeadSlot.inventoryItem.itemData == item)
        {
            inventoryItem.itemData.Equip(false);
            activeHeadSlot.inventoryItem.itemData = null;
        }

        Quaternion rotation = Quaternion.Euler(90f, 0f, 0f);
        Instantiate(inventoryItem.itemData.prefab, position, rotation);
        Remove(inventoryItem);
    }
}
