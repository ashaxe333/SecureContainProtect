using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaTriggerScript : MonoBehaviour
{
    public GameObject parentArea;

    private void Start()
    {
        if (parentArea == null)
            Debug.Log("parent area!!!" + this.gameObject.name);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            //Debug.Log("current corridor changed to: " + AreaManager.Instance.currentArea.name);
            parentArea.GetComponent<AreaInstanceScript>().PlayerEntered();
            //parentArea.PlayerEntered();
        }
    }
}
