using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.Mesh;

public class InventoryScript : MonoBehaviour
{
    public List<GameObject> slots;
    public List<InventorySlot> slotScripts;

    public EquipmentSlotScript activeHeadSlot;
    public EquipmentSlotScript activeHandSlot;
    public GameObject inventory;

    public GameObject maskVision;

    private CanvasGroup canvasGroup;

    void Awake()
    {
        GetScripts();
    }

    void Start()
    {
        canvasGroup = inventory.GetComponent<CanvasGroup>();
        canvasGroup.alpha = 0f;           // invisibile
        canvasGroup.blocksRaycasts = false; // nepøijímá kliky
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
            if(canvasGroup.alpha == 0f)
            {
                canvasGroup.alpha = 1f;
                canvasGroup.blocksRaycasts = true;
                CursorManagerScript.Instance.ShowCursor();
                GameStateManagerScript.Instance.SetState(GameState.INVENTORY);
            }
            else
            {
                canvasGroup.alpha = 0f;
                canvasGroup.blocksRaycasts = false;
                CursorManagerScript.Instance.HideCursor();
                GameStateManagerScript.Instance.SetState(GameState.GAMEPLAY);

                foreach (InventorySlot slot in slotScripts)
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
        bool show = true;

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

        if (show) GameManagerScript.Instance.SetTextInfo("Inventory is full");
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
        if(activeHeadSlot.used && activeHeadSlot.inventoryItem.itemData is MaskData) return true;
        return false;
    }
    public bool IsNightVisionActive()
    {
        if (activeHeadSlot.used && activeHeadSlot.inventoryItem.itemData is NightVisionData) return true;
        return false;
    }

    /// <summary>
    /// equipne item do aktivního slotu
    /// </summary>
    /// <param name="inventoryItem"> equipnutý item </param>
    public void Equip(InventoryItem inventoryItem)
    {
        EquipmentSlotScript equipmentSlot = GetCorrectAreaSlot(inventoryItem);

        if (equipmentSlot == null) return;

        if (!equipmentSlot.used)
        {
            inventoryItem.itemData.Equip(true);
            equipmentSlot.SetItem(inventoryItem);
            Remove(inventoryItem);
        }
        else if(equipmentSlot.inventoryItem == inventoryItem)
        {
            ItemData unequiped = equipmentSlot.inventoryItem.itemData;
            Add(unequiped);
            unequiped.Equip(false);
            equipmentSlot.Clear();
        }
        else
        {
            ItemData unequiped = equipmentSlot.inventoryItem.itemData;
            Add(unequiped);
            unequiped.Equip(false);
            equipmentSlot.Clear();

            equipmentSlot.SetItem(inventoryItem);
            inventoryItem.itemData.Equip(true);
            Remove(inventoryItem);
        }
    }

    /// <summary>
    /// Dostane správný EquipmentSlotScript
    /// </summary>
    /// <param name="inventoryItem"> item </param>
    /// <returns> správný EquipmentSlotScript </returns>
    public EquipmentSlotScript GetCorrectAreaSlot(InventoryItem inventoryItem)
    {
        switch (inventoryItem.itemData.area)
        {
            case ItemArea.HAND:
                return activeHandSlot;

            case ItemArea.HEAD:
                return activeHeadSlot;

            default:
                return null;
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
