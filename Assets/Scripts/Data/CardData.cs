using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Item/Card")]
public class CardData : ItemData
{
    public int level;

    public override void Equip(bool equip)
    {
        isEquiped = equip;
        //Debug.Log($"using {itemName}: {isEquiped}");
    }
}
