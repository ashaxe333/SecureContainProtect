using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CorridorInstanceScript : MonoBehaviour
{
    public CorridorData sourceData; // ruènì nebo automaticky
    public List<Transform> spawnPoints = new List<Transform>();

    private bool playerIsInside = false;

    public Vector3 GetRandomSpawnPoint()
    {
        if (spawnPoints.Count == 0)
        {
            Debug.Log("CorridorInstanceScript: No spawn points assigned!");
            return transform.position;
        }

        int index = Random.Range(0, spawnPoints.Count);
        float offsetY = Random.Range(-4f, 4f);
        float offsetX = Random.Range(-4f, 4f);
        Transform spawn = spawnPoints[index];

        return new Vector3(spawn.position.x + offsetX, spawn.position.y + offsetY, spawn.position.z);
    }

    public void PlayerEntered()
    {
        Debug.Log("Player entered" + sourceData.name);
        CorridorManager.corridorManagerInstance.currentCorridor = this.gameObject;  // this.GameObject(); - Z nìjakého dùvodu není platný
    }
}