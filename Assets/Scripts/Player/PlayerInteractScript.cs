using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class PlayerInteractScript : MonoBehaviour
{
    // ÚKOLY
    // 1) Oddìlit zámky od interactive objektù, a dát jim jinı image, ne ruèièku pokud mám aktivní kartu
    // 2) Všem collectiblùm zobrazt pøi sbírání ještì název
    // 3) Opìt SCP939 by si informace mìlo získávat samo

    private GameObject player;
    private InventoryScript inventoryScript;

    public GameObject scp939_1;
    public GameObject scp939_2;
    public GameObject scp939_3;

    //public List<GameObject> allInteractiveObjects = new List<GameObject>();
    public GameObject hand;
    private bool fkinHand;
    private bool interactEnable = true;
    [SerializeField] public GameObject clickedObject;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        inventoryScript = player.GetComponent<InventoryScript>();

        scp939_1 = GameObject.FindGameObjectWithTag("939_1");
        scp939_2 = GameObject.FindGameObjectWithTag("939_2");
        scp939_3 = GameObject.FindGameObjectWithTag("939_3");

        hand.SetActive(false);
    }

    void Update()
    {
        if (interactEnable)
            Interact();

        Assign939();
    }

    /// <summary>
    /// Hlídá, jestli hráè neintereaguje s objekty, se kterıma to je monı
    /// </summary>
    public void Interact()
    {
        clickedObject = null;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 5.0f, 1 << 7))
        {
            //Debug.Log("je to interactive");
            //Dát sem image karty?
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
                inventoryScript.Add(clickedObject.GetComponent<ItemHolderScript>().sourceData);
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
    /// Najde všechny vìci, kterı se dají sbírat, aktivovat, pouít,...
    /// Mùu sem dát i zámky, a nemusí bıt v LockRayScriptu. 
    /// 
    /// Ale chci mít míøenı ray u collectiblù/interaktivù?
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
    /// Nastaví, jestli hráè mùe intereagovat
    /// </summary>
    /// <param name="enabled"></param>
    public void SetInteractEnable(bool enabled)
    {
        interactEnable = enabled;
    }

    /// <summary>
    /// inicializuje všechny scp-939 po pøechodu do patra F0
    /// </summary>
    private void Assign939()    //vyøešit tak, abych nemusel assignvat tyto scp. Kadı zvláš by si mìl vytáhnout z hráèe informaci o tom, jak chodí
    {
        if (GameManagerScript.Instance.currentFloor == 0)
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
