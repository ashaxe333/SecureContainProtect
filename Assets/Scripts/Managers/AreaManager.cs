using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class AreaManager : MonoBehaviour
{
    public static AreaManager Instance { get; private set; }    //staticky mùžu pøistupovat ke tøídì AreaManager + èíst mùžu všude, ale mìnit jen tady

    public List<AreaData> allAreaTypes;
    public List<AreaInstanceScript> areas = new List<AreaInstanceScript>();

    [HideInInspector] public AreaInstanceScript currentArea;    // chodba, ve které se hráè momentálnì nachází
    [HideInInspector] public List<AreaInstanceScript> nextAreas = new List<AreaInstanceScript>();    // list s navazujícíma areama na currentArea
    [HideInInspector] public AreaInstanceScript previousArea;    // pøedešlá currentArea

    public GameObject jumpScareWayPoint;
    private GameObject player;

    private void Awake()
    {
        Singleton();
        LoadAreas();
    }

    void Start()
    {
        currentArea = GameObject.FindGameObjectWithTag("StartingArea").GetComponent<AreaInstanceScript>();
        nextAreas = currentArea.nextAreas;

        player = GameObject.FindGameObjectWithTag("Player");
        jumpScareWayPoint = GameObject.FindGameObjectWithTag("JumpScareWP");

        if (jumpScareWayPoint == null) 
            Debug.Log("AreaManager: jumpScare Point!!");
    }

    /// <summary>
    /// Získá náhodnou areu
    /// </summary>
    /// <returns> area </returns>
    public AreaInstanceScript GetRandomNonPlayerRoom()
    {
        areas.Remove(currentArea);
        int index = Random.Range(0, areas.Count);

        // DOÈASNÝ - Hlídá, aby se SCP173 spawnoval do stejného patra jako je hráè
        while (areas[index].floor != GameManagerScript.Instance.currentFloor || areas[index].spawnPoints.Count == 0)
            index = Random.Range(0, areas.Count);

        areas.Add(currentArea);
        return areas[index];
    }

    /// <summary>
    /// Získá nejbližší areu k hráèi
    /// </summary>
    /// <returns> area </returns>
    public AreaInstanceScript GetClosestNonPlayerRoom()
    {
        if (nextAreas.Count == 0)
            return GetRandomNonPlayerRoom();
        else if (nextAreas.Count == 1) 
        { 
            if (nextAreas[0].spawnPoints.Count == 0)
                return GetRandomNonPlayerRoom();
            else
                return nextAreas[0];
        }
        else
        {
            float min = float.MaxValue;
            int index = 0;

            nextAreas.Remove(previousArea);
            for (int i = 0; i < nextAreas.Count; i++)
            {
                if (nextAreas[i].spawnPoints == null || nextAreas[i].spawnPoints.Count == 0)
                    continue;
                if (min > Vector3.Distance(nextAreas[i].transform.position, player.transform.position))
                {
                    min = Vector3.Distance(nextAreas[i].transform.position, player.transform.position);
                    index = i;
                }
            }
            nextAreas.Add(previousArea);

            if (min != float.MaxValue)
                return nextAreas[index];
            else
                return GetRandomNonPlayerRoom();
        }
    }

    /// <summary>
    /// Naète všechny chodby do listu (odkud broadcast bere chodby pro spawn - NEIMPLEMENTOVÁNO)
    /// </summary>
    public void LoadAreas() 
    {
        areas = FindObjectsByType<AreaInstanceScript>().ToList();
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

    /// <summary>
    /// Singleton (bez toho je Instance jen null)
    /// </summary>
    public void Singleton()
    {
        if (Instance == null)
        {
            Instance = this;
            //DontDestroyOnLoad(gameObject); // neznièí instanci pøi pøechodu do jiné scény
        }
        else
            Destroy(gameObject); // znièí duplicitní instanci
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
