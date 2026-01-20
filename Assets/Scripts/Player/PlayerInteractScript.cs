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
    // 1) Oddìlit zámky od interactive objektù, a dát jim jiný image, než ruèièku pokud mám aktivní kartu
    // 2) Všem collectiblùm zobrazit pøi sbírání ještì název
    // 3) 

    private GameObject player;
    private InventoryScript inventoryScript;

    //public List<GameObject> allInteractiveObjects = new List<GameObject>();
    public GameObject hand;
    private bool interactEnable = true;
    [SerializeField] public GameObject clickedObject;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        inventoryScript = player.GetComponent<InventoryScript>();

        hand.SetActive(false);
    }

    void Update()
    {
        if (interactEnable)
            Interact();
    }

    /// <summary>
    /// Hlídá, jestli hráè neintereaguje s objekty, se kterýma to je možný
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
        else if (Physics.Raycast(ray, out hit, 5.0f, 1 << 13))
        {
            Debug.Log("je to LightSwitch");
            hand.SetActive(true);

            if (Input.GetMouseButtonDown(0))
            {
                clickedObject = hit.collider.gameObject;
                clickedObject.GetComponent<LightTriggerScript>().LightSwitch();
            }
        }
        else hand.SetActive(false);
    }

    /*
    /// <summary>
    /// Najde všechny vìci, který se dají sbírat, aktivovat, použít,...
    /// Mùžu sem dát i zámky, a nemusí být v LockRayScriptu. 
    /// 
    /// Ale chci mít míøený ray u collectiblù/interaktivù?
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
    /// Nastaví, jestli hráè mùže intereagovat
    /// </summary>
    /// <param name="enabled"></param>
    public void SetInteractEnable(bool enabled)
    {
        interactEnable = enabled;
    }
}
