using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AreaInstanceScript : MonoBehaviour
{
    public AreaData sourceData;
    public List<Transform> spawnPoints = new List<Transform>();
    public int floor;
    public List<AreaInstanceScript> nextAreas = new List<AreaInstanceScript>();
    //public GameObject saveSpawnPoint;
    private GameObject scp173;
    public List<Light> lights = new List<Light>();

    void Awake()
    {
        //saveSpawnPoint = GameObject.FindGameObjectWithTag("ssp");
        scp173 = GameObject.FindGameObjectWithTag("173");

        if (scp173 == null) Debug.Log("Není broadcast!!");
    }

    public Vector3 GetRandomSpawnPoint()
    {
        if (spawnPoints.Count == 0)
        {
            //Debug.Log($"AreaInstanceScript: No spawn points assigned in {gameObject.name}!");
            //return saveSpawnPoint.transform.position;
            return scp173.transform.position;
        }

        int index = Random.Range(0, spawnPoints.Count);
        float offsetY = Random.Range(-4f, 4f);
        float offsetX = Random.Range(-4f, 4f);
        Transform spawn = spawnPoints[index];

        return new Vector3(spawn.position.x + offsetX, spawn.position.y + offsetY, spawn.position.z);
    }

    /// <summary>
    /// Nastavuje novou current areu
    /// </summary>
    public void PlayerEntered()
    {
        Debug.Log($"AreaInstanceScript: Player entered {gameObject.name} ({sourceData.name})");
        AreaManager.Instance.previousArea = AreaManager.Instance.currentArea;
        AreaManager.Instance.currentArea = this;
        AreaManager.Instance.nextAreas = nextAreas;
    }
}