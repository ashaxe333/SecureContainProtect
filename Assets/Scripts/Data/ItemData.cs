using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item")]
public abstract class ItemData : ScriptableObject
{
    public string itemName;
    public Sprite icon;
    public string description;
    public GameObject prefab;

    public virtual void Use(GameObject player)
    {
        Debug.Log("Používáš: " + itemName);
    }
}

