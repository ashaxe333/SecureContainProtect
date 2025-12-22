using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class InventoryScript : MonoBehaviour
{
    public List<GameObject> slots;
    public List<InventoryItem> items; //tohle asi pryè
    public ItemData usingHeadItem;
    public ItemData usingHandItem;
    private bool show = true;

    public void Add(ItemData collected)
    {
        foreach (GameObject slot in slots)
        {
            if (!slot.GetComponent<InventorySlot>().used) 
            {
                items.Add(new InventoryItem(collected));
                slot.GetComponent<InventorySlot>().sprite = collected.GetComponent<ItemData>().sprite;
                show = false;
                break;
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
        foreach (GameObject slot in slots)
        {
            if (slot.GetComponent<InventorySlot>().InventoryItem == inventoryItem)
            {
                items.Remove(inventoryItem);
                slot.GetComponent<InventorySlot>().RemoveSprite();
                break;
            }
        }
    }

    public bool HasKeyCard(int requiredCardLevel)
    {
        foreach (InventoryItem item in items)
        {
            if(item.itemData is CardData card && card.level >= requiredCardLevel)
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
                item.isUsing = true;
                usingHandItem = item;
            }
            else
            {
                usingHandItem.isUsing = false;
                item.isUsing = true;
                usingHandItem = item;
            }
        }
        else if (item.area == ItemArea.HEAD)
        {
            if (usingHeadItem == null)
            {
                item.isUsing = true;
                usingHeadItem = item;
            }
            else
            {
                usingHeadItem.isUsing = false;
                item.isUsing = true;
                usingHeadItem = item;
            }
        }
    }
}
