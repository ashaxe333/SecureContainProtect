using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AreaInstanceScript : MonoBehaviour
{
    public AreaData sourceData;
    public List<Transform> spawnPoints = new List<Transform>();
    [HideInInspector] public int floor;
    //public GameObject saveSpawnPoint;
    public GameObject scp173;

    void Awake()
    {
        //saveSpawnPoint = GameObject.FindGameObjectWithTag("ssp");
        scp173 = GameObject.FindGameObjectWithTag("173");

        if (scp173 == null) Debug.Log("Není scp173!!");
    }

    public Vector3 GetRandomSpawnPoint()
    {
        if (spawnPoints.Count == 0)
        {
            Debug.Log($"AreaInstanceScript: No spawn points assigned in {gameObject.name}!");
            //return saveSpawnPoint.transform.position;
            return scp173.transform.position;
        }

        floor = (int)sourceData.floor;

        int index = Random.Range(0, spawnPoints.Count);
        float offsetY = Random.Range(-4f, 4f);
        float offsetX = Random.Range(-4f, 4f);
        Transform spawn = spawnPoints[index];

        return new Vector3(spawn.position.x + offsetX, spawn.position.y + offsetY, spawn.position.z);
    }

    public void PlayerEntered()
    {
        //Debug.Log("Player entered" + sourceData.name);
        //AreaManager.Instance.currentArea = this.gameObject;  // this.GameObject(); - Z nìjakého dùvodu není platný

        AreaManager.Instance.currentArea = this.gameObject;  // this.GameObject(); - Z nìjakého dùvodu není platný
    }
}