using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ItemData : ScriptableObject
{
    public string itemName;
    public Sprite sprite;
    public string description;

    public GameObject prefab;
    public ItemArea area;
    public bool isEquiped;

    public virtual void Equip(bool use)
    {
        
    }
}
