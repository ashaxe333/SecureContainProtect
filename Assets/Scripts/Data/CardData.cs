using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Item/Card")]
public class CardData : ItemData
{
    public int level;

    public override void Use(GameObject player)
    {
        Debug.Log("Using key card level " + level);
    }
}
