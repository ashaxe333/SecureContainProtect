using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CorridorTriggerScript : MonoBehaviour
{
    public GameObject parentCorridor;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("current corridor changed to: " + CorridorManager.corridorManagerInstance.currentCorridor.name);
            parentCorridor.GetComponent<CorridorInstanceScript>().PlayerEntered();
        }
    }
}
