using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Area", menuName = "Item/Area")]
public class AreaData : ScriptableObject
{
    public string corridorName;
    public AreaType type;
    public FloorType floor;
    public GameObject prefab;
    public int exits;
}
