using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InventoryItem
{
    public ItemData itemData;

    public InventoryItem(ItemData data)
    {
        this.itemData = data;
    }
}