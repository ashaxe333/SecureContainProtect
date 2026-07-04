using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR;

public class LockRayScript : MonoBehaviour
{
    // UPRAVIT!
    // 1) List locks pøedìlat z GameObjectu na LockScript

    private GameObject player;
    private GameObject scp173;

    private List<GameObject> locks = new List<GameObject>();
    private GameObject clickedObject;

    private bool interactEnable = true;


    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        scp173 = GameObject.FindGameObjectWithTag("173");

        LoadLocks();
    }

    // Update is called once per frame
    void Update()
    {
        Interact();
    }

    /// <summary>
    /// Hlídá, jestli hráè interaguje se zámky. Pokud ano, spustí se metoda otevírání/zavírání dveøí kliknutého zámku
    /// </summary>
    public void Interact()
    {
        int targetIndex = ClosestLock();
        Vector3 directionToLock = (locks[targetIndex].transform.position - player.transform.position).normalized;     // získám smìr a normalized ho zkrátí na napø [0, 1]

        Ray ray = new Ray(player.transform.position, directionToLock);    // odkud, jakým smìrem
        RaycastHit hit;
        Debug.DrawRay(player.transform.position, directionToLock * 4.0f, Color.red);

        if (Physics.Raycast(ray, out hit, 4.0f, (1 << 8) | (1 << 10) | (1 << 11)))
        {
            if (hit.collider.gameObject.layer == 8)
            {
                //Debug.Log("muzu otevrit/zavrit");
                InteractionUIManager.Instance.SetLockInteractState(true);

                if (Input.GetMouseButtonDown(0) && interactEnable)
                {
                    //Debug.Log("oteviram/zaviram");
                    clickedObject = hit.collider.gameObject;
                    clickedObject.GetComponent<LockScript>().HandleDoorInteraction();
                }
            }
            else InteractionUIManager.Instance.SetLockInteractState(false);
        }
        else InteractionUIManager.Instance.SetLockInteractState(false);
    }

    /// <summary>
    /// Najde nejbližší zámek k hráèi
    /// </summary>
    /// <returns> index v poli nejbližšího zámku </returns>
    int ClosestLock()
    {
        float min = float.MaxValue;
        int index = 0;

        for (int i = 0; i < locks.Count; i++)
        {
            if (locks[i].activeInHierarchy && min > Vector3.Distance(locks[i].transform.position, player.transform.position))
            {
                min = Vector3.Distance(locks[i].transform.position, player.transform.position);
                index = i;
            }
        }

        return index;
    }

    /// <summary>
    /// Nastaví, jestli hráè mùže intereagovat
    /// </summary>
    /// <param name="enabled"></param>
    public void SetInteractEnable(bool enabled)
    {
        interactEnable = enabled;
    }

    /// <summary>
    /// Naète všechny zámky do pole
    /// </summary>
    void LoadLocks()
    {
        GameObject[] objectsInScene = FindObjectsByType<GameObject>();

        foreach (GameObject obj in objectsInScene)
        {
            if (obj.layer == 8)
            {
                locks.Add(obj);
            }
        }

        Debug.Log("pocet zamku: " + locks.Count);
    }
}
