using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaTriggerScript : MonoBehaviour
{
    public AreaInstanceScript parentArea;

    private void Start()
    {
        if (parentArea == null)
            Debug.Log($"AreaTriggerScript: {gameObject.name} Is missing parent area");

        //Debug.Log($"AreaTriggerScript: {parentArea.gameObject.name}");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
            parentArea.PlayerEntered();
    }
}
