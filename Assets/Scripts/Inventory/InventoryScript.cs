using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryScript : MonoBehaviour
{
    public List<InventoryItem> items;

    public void Add(ItemData item)
    {
        items.Add(new InventoryItem(item));
    }

    public void Remove(int index)
    {
        items.RemoveAt(index);
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
}
