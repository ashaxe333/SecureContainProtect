using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class AreaManager : MonoBehaviour
{
    // MOZNA:
    // pøidat currentAreas, což bude pøedstavovat list s aktualními chodbami. Odtud bude scp173 loadovat spawny, a ne z areas. Kvùli patrùm - jsem na F1, scp tahá z F1
    public static AreaManager Instance { get; private set; }    //staticky mùžu pøistupovat ke tøídì AreaManager + èíst mùžu všude, ale mìnit jen tady

    public List<AreaData> allAreaTypes;
    public List<GameObject> areas = new List<GameObject>();
    //public List<GameObject> currentAreas = new List<GameObject>();    // list s aktuálnì aktivníma místnostma
    public GameObject currentArea;
    public GameObject jumpScareWayPoint;
    private GameObject player;

    private void Awake()
    {
        // Singleton (bez toho je Instance jen null)
        if (Instance == null)
        {
            Instance = this;
            //DontDestroyOnLoad(gameObject); // neznièí instanci pøi pøechodu do jiné scény
        }
        else
        {
            Destroy(gameObject); // znièí duplicitní instanci
        }

        LoadAreas();
    }

    void Start()
    {
        currentArea = areas[Random.Range(0, areas.Count)];
        player = GameObject.FindGameObjectWithTag("Player");
        jumpScareWayPoint = GameObject.FindGameObjectWithTag("JumpScareWP");

        if (jumpScareWayPoint == null) 
        {
            Debug.Log("AreaManager: jumpScare Point!!");
        }
    }

    public GameObject GetRandomNonPlayerRoom()
    {
        //Debug.Log("poèet chodeb: " + areas.Count);
        areas.Remove(currentArea);
        //Debug.Log("poèet chodeb: " + areas.Count);
        int index = Random.Range(0, areas.Count);
        areas.Add(currentArea);
        //Debug.Log("poèet chodeb: " + areas.Count);
        return areas[index];
    }

    public GameObject GetClosestNonPlayerRoom()
    {
        float min = float.MaxValue;
        int index = 0;

        areas.Remove(currentArea);
        for (int i = 0; i < areas.Count; i++)
        {
            if (min > Vector3.Distance(areas[i].transform.position, player.transform.position))
            {
                min = Vector3.Distance(areas[i].transform.position, player.transform.position);
                index = i;
            }
        }
        areas.Add(currentArea);

        return areas[index];
    }

    /// <summary>
    /// Naètì všechny chodby do listu, odkud scp173 bere chodby pro spawn
    /// </summary>
    public void LoadAreas() 
    {
        GameObject[] objectsInScene = FindObjectsByType<GameObject>(FindObjectsSortMode.None);

        foreach (GameObject obj in objectsInScene)
        {
            if (obj.layer == LayerMask.NameToLayer("Areas"))
            {
                areas.Add(obj);
            }
        }

        Debug.Log("areas count: " + areas.Count);
    }

    /// <summary>
    /// Kontroluje, jestli se mùže scp 173 spawnout pøímo pøed hráèe
    /// </summary>
    /// <returns> vrací mùže/nemùže </returns>
    public bool CanSpawn()
    {
        Vector3 directionToPlayer = (jumpScareWayPoint.transform.position - player.transform.position).normalized;     // získám smìr a normalized ho zkrátí na napø [0, 1]

        Ray ray = new Ray(player.transform.position, directionToPlayer);    // odkud, jakým smìrem
        RaycastHit hit;

        Debug.DrawRay(player.transform.position, directionToPlayer * 10.0f, Color.green);

        if (Physics.Raycast(ray, out hit, 10.0f))
        {
            GameObject hitObject = hit.collider.gameObject;
            if (hitObject == jumpScareWayPoint)
            {
                Debug.Log("AreaManager: Mùže se spawnout");
                return true;
            }
            else
            {
                Debug.Log("AreaManager: Nemùže se spawnout");
                return false;
            }
        }
        else
        {
            Debug.Log("AreaManager: Nemùže se spawnout");
            return false;
        }
    }

    // Pro scp173, aby pøi optimalizaci vybíralo zprávný spawny
    /*
    public void LoadCurrentAreas()
    {
        switch (GameManagerScript.Instance.currentFloor)
        {
            case 0:
                foreach(GameObject corridor in areas)
                {
                    if (corridor.GetComponent<AreaInstanceScript>().floor == GameManagerScript.Instance.currentFloor)
                    {
                        currentAreas.Add(corridor);
                    }
                }
                break;

        }
    }
    */
}
