using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class PlayerInteractScript : MonoBehaviour
{
    private GameObject player;

    public GameObject scp939_1;
    public GameObject scp939_2;
    public GameObject scp939_3;

    //public List<GameObject> allInteractiveObjects = new List<GameObject>();
    public GameObject hand;
    private bool fkinHand;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        scp939_1 = GameObject.FindGameObjectWithTag("939_1");
        scp939_2 = GameObject.FindGameObjectWithTag("939_2");
        scp939_3 = GameObject.FindGameObjectWithTag("939_3");

        hand.SetActive(false);
    }

    void Update()
    {
        Interact();
        Assign939();
    }

    /// <summary>
    /// HlÌd·, jestli hr·Ë neintereaguje s objekty, se kter˝ma to je moûn˝
    /// </summary>
    public void Interact()
    {
        GameObject clickedObject = null;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 5.0f, 1 << 7))
        {
            //Debug.Log("je to interactive");
            hand.SetActive(true);

            if (Input.GetMouseButtonDown(0))
            {
                clickedObject = hit.collider.gameObject;
                clickedObject.GetComponent<LeverDoorScript>().DoorMove(clickedObject);

                if (scp939_1 != null && scp939_2 != null && scp939_3 != null)
                {
                    Debug.Log("neni null");
                    scp939_1.GetComponent<SCP939Script>().NoiseTrigger(clickedObject);
                    scp939_2.GetComponent<SCP939Script>().NoiseTrigger(clickedObject);
                    scp939_3.GetComponent<SCP939Script>().NoiseTrigger(clickedObject);
                }
            }
        }
        else if (Physics.Raycast(ray, out hit, 5.0f, 1 << 6))
        {
            //Debug.Log("je to collectible");
            hand.SetActive(true);

            if (Input.GetMouseButtonDown(0))
            {
                clickedObject = hit.collider.gameObject;
                player.GetComponent<InventoryScript>().Add(clickedObject.GetComponent<ItemHolderScript>().sourceData);
                clickedObject.SetActive(false);
            }
        }
        else if (Physics.Raycast(ray, out hit, 5.0f, 1 << 9))
        {
            Debug.Log("je to button");
            hand.SetActive(true);
        }
        else hand.SetActive(false);
    }

    /*
    /// <summary>
    /// Najde vöechny vÏci, kter˝ se dajÌ sbÌrat, aktivovat, pouûÌt,...
    /// M˘ûu sem d·t i z·mky, a nemusÌ b˝t v LockRayScriptu. 
    /// 
    /// Ale chci mÌt mÌ¯en˝ ray u collectibl˘/interaktiv˘?
    /// 
    /// </summary>
    public void LoadInteractiveObjects()
    {
        GameObject[] objectInScene = FindObjectsOfType<GameObject>();

        foreach (GameObject go in objectInScene)
        {
            allInteractiveObjects.Add(go);
        }
    }
    */

    /// <summary>
    /// inicializuje vöechny scp-939 po p¯echodu do patra F0
    /// </summary>
    private void Assign939()    //vy¯eöit tak, abych nemusel assignvat tyto scp. Kaûd˝ zvl·öù by si mÏl vyt·hnout z hr·Ëe informaci o tom, jak chodÌ
    {
        if (GameManagerScript.gameManagerInstance.floor == 0)
        {
            scp939_1 = GameObject.FindGameObjectWithTag("939_1");
            scp939_2 = GameObject.FindGameObjectWithTag("939_2");
            scp939_3 = GameObject.FindGameObjectWithTag("939_3");
        }
        else
        {
            scp939_1 = null;
            scp939_2 = null;
            scp939_3 = null;
        }
    }
}
