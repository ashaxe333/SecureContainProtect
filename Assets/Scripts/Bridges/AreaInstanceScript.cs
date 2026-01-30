using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AreaInstanceScript : MonoBehaviour
{
    public AreaData sourceData;
    public List<Transform> spawnPoints = new List<Transform>();
    [HideInInspector] public int floor;

    public Vector3 GetRandomSpawnPoint()
    {
        if (spawnPoints.Count == 0)
        {
            Debug.Log("AreaInstanceScript: No spawn points assigned!");
            return transform.position;
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