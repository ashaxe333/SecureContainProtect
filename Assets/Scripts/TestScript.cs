using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TestScript : MonoBehaviour
{
    public List<GameManagerScript> gms = new List<GameManagerScript>();
    private void Awake()
    {
        gms = FindObjectsByType<GameManagerScript>(FindObjectsSortMode.None).ToList();
        //Debug.Log(gms.Count);
    }
}
