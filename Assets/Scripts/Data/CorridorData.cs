using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Corridor", menuName = "Level/Corridor")]
public class CorridorData : ScriptableObject
{
    public string corridorName;
    public CorridorType type; // enum - rovná, zatáèka, atd.
    public GameObject prefab; // odkaz na prefab místnosti
    public int exits; // kolik má výstupù (napø. 2, 3, 4)
}

public enum CorridorType
{
    Straight,
    Corner,
    T_Junction,
    Crossroad
}
