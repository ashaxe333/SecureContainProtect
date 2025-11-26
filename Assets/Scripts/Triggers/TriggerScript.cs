using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerScript : MonoBehaviour
{
    public GameObject floorForActivate;
    private GameObject floorForDeactivate;
    private GameObject scp173;

    void Start()
    {
        scp173 = GameObject.FindGameObjectWithTag("173");

        if(floorForActivate == null)
        {
            Debug.Log("TriggerScript: Add floor for activate!");
        }
    }

    /*
    /// <summary>
    /// Hlídá, pokud se hráè nedotknul triggeru. Pokud jo, Deaktivuje se aktuálnì aktivní patro, a aktivuje se patro nastavené na danném trigger
    /// </summary>
    /// <param name="other"> Objekt dotýkající se triggeru (hráè) </param>
    private void OnTriggerEnter(Collider other)
    {
        floorForDeactivate = GameManagerScript.gameManagerInstance.GetComponent<GameManagerScript>().GetActiveFloor();
        Debug.Log("deactivating:" + floorForDeactivate.name);

        if (other.gameObject.CompareTag("Player"))
        {
            floorForDeactivate.SetActive(false);
            floorForActivate.SetActive(true);
        }

        //scp173.GetComponent<SCP173Script>().CorrectFloorWP(floorForActivate);
    }
    */
}
