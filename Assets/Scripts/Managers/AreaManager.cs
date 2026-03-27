using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class AreaManager : MonoBehaviour
{
    // MOZNA:
    // pøidat currentAreas, což bude pøedstavovat list s aktualními chodbami. Odtud bude broadcast loadovat spawny, a ne z areas. Kvùli patrùm - jsem na F1, scp tahá z F1

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
            Destroy(gameObject); // znièí duplicitní instanci

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

    /// <summary>
    /// Získá náhodnou areu
    /// </summary>
    /// <returns> area </returns>
    public GameObject GetRandomNonPlayerRoom()
    {
        areas.Remove(currentArea);
        int index = Random.Range(0, areas.Count);

        // DOÈASNÝ - Hlídá, aby se SCP173 spawnoval do stejného patra jako je hráè
        while (areas[index].GetComponent<AreaInstanceScript>().floor != GameManagerScript.Instance.currentFloor)
            index = Random.Range(0, areas.Count);

        areas.Add(currentArea);
        return areas[index];
    }

    /// <summary>
    /// Získá nejbližší areu k hráèi    (VZDUŠNOU ÈAROU ALE -> v budoucnu pøidat každé aree list tìch navazujících - to budou ty nejbližší)
    /// </summary>
    /// <returns> area </returns>
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
    /// Naètì všechny chodby do listu, odkud broadcast bere chodby pro spawn
    /// </summary>
    public void LoadAreas() 
    {
        //areas = FindObjectsByType<AreaInstanceScript>(FindObjectsSortMode.None).ToList();
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

    // Pro broadcast, aby pøi optimalizaci vybíralo zprávný spawny
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
