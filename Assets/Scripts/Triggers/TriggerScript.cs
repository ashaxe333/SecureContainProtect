using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerScript : MonoBehaviour
{
    // ÚKOLY:
    // 1) Zprovoznit optimalizaci GameObjektù (vše zaøádkované)

    //public GameObject floorForActivate;
    //private GameObject floorForDeactivate;
    //private GameObject scp173;

    public int newCurrentFloor;

    void Start()
    {
        /*
        scp173 = GameObject.FindGameObjectWithTag("173");

        if(floorForActivate == null)
        {
            Debug.Log("TriggerScript: Add currentFloor for activate!");
        }
        */
    }

    /// <summary>
    /// Hlídá, pokud se hráè nedotknul triggeru. Pokud jo, nastaví nový aktuální patro
    /// </summary>
    /// <param name="other"> Objekt dotýkající se triggeru (hráè) </param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && newCurrentFloor != GameManagerScript.Instance.currentFloor)
        {
            GameManagerScript.Instance.currentFloor = newCurrentFloor;
            Debug.Log($"TriggerScript: new current floor = {GameManagerScript.Instance.currentFloor}");
        }
    }

    /*
    /// <summary>
    /// Hlídá, pokud se hráè nedotknul triggeru. Pokud jo, Deaktivuje se aktuálnì aktivní patro, a aktivuje se patro nastavené na danném trigger
    /// </summary>
    /// <param name="other"> Objekt dotýkající se triggeru (hráè) </param>
    private void OnTriggerEnter(Collider other)
    {
        floorForDeactivate = GameManagerScript.Instance.GetActiveFloor();

        if (other.gameObject.CompareTag("Player") && floorForActivate != floorForDeactivate)
        {
            Debug.Log("deactivating:" + floorForDeactivate.name);
            floorForDeactivate.SetLightActive(false); 
            Debug.Log("activating:" + floorForActivate.name);
            floorForActivate.SetLightActive(true);
        }

        //scp173.GetComponent<SCP173Script>().CorrectFloorWP(floorForActivate);
    }
    */
}
