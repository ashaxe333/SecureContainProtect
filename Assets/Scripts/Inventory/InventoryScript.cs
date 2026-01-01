using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.Mesh;

public class InventoryScript : MonoBehaviour
{
    // ÚKOLY:
    // 1) Pøidat Unequip metodu? Jen zatím nevím k èemu

    public List<GameObject> slots;
    public List<InventorySlot> slotScripts;

    public EquipmentSlotScript activeHeadSlot;
    public EquipmentSlotScript activeHandSlot;

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

                slot.SetItem(invItem);
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
                slot.Clear();
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
        if (activeHandSlot.used && activeHandSlot.inventoryItem.itemData is CardData card && card.level >= requiredCardLevel) return true;
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

    /// <summary>
    /// equipne item do aktivního slotu
    /// </summary>
    /// <param name="inventoryItem"> equipnutý item </param>
    public void Equip(InventoryItem inventoryItem)
    {
        if (inventoryItem.itemData.area == ItemArea.HAND)
        {
            if (activeHandSlot.inventoryItem == inventoryItem) return;

            if (!activeHandSlot.used)
            {
                inventoryItem.itemData.Equip(true);
                activeHandSlot.SetItem(inventoryItem);
                Remove(inventoryItem);
            }
            else
            {
                ItemData unequiped = activeHandSlot.inventoryItem.itemData;
                Add(unequiped);
                unequiped.Equip(false);
                activeHandSlot.Clear();

                activeHandSlot.SetItem(inventoryItem);
                inventoryItem.itemData.Equip(true);
                Remove(inventoryItem);
            }
        }
        else if (inventoryItem.itemData.area == ItemArea.HEAD)
        {
            if (activeHeadSlot.inventoryItem == inventoryItem) return;

            if (!activeHeadSlot.used)
            {
                inventoryItem.itemData.Equip(true);
                activeHeadSlot.SetItem(inventoryItem);
                Remove(inventoryItem);
            }
            else
            {
                ItemData unequiped = activeHeadSlot.inventoryItem.itemData;
                Add(unequiped);
                unequiped.Equip(false);
                activeHeadSlot.Clear();

                activeHeadSlot.SetItem(inventoryItem);
                inventoryItem.itemData.Equip(true);
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
        if (activeHandSlot.used && activeHandSlot.inventoryItem == inventoryItem)
        {
            inventoryItem.itemData.Equip(false);
            activeHandSlot.Clear();
        }
        else if(activeHeadSlot.used && activeHeadSlot.inventoryItem == inventoryItem)
        {
            inventoryItem.itemData.Equip(false);
            activeHeadSlot.Clear();
        }
        else
        {
            Remove(inventoryItem);
        }

        Quaternion rotation = Quaternion.Euler(90f, 0f, 0f);
        Instantiate(inventoryItem.itemData.prefab, position, rotation);
    }
}
