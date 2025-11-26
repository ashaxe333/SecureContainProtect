using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class CorridorManager : MonoBehaviour
{
    public static CorridorManager corridorManagerInstance { get; private set; }    //staticky mùžu pøistupovat ke tøídì CorridorManager + èíst mùžu všude, ale mìnit jen tady

    public List<CorridorData> allCorridorTypes;
    public List<GameObject> corridors = new List<GameObject>();
    public GameObject currentCorridor;
    public GameObject jumpScareWayPoint;
    private GameObject player;

    private void Awake()
    {
        // Singleton (bez toho je corridorManagerInstance jen null)
        if (corridorManagerInstance == null)
        {
            corridorManagerInstance = this;
            //DontDestroyOnLoad(gameObject); // neznièí instanci pøi pøechodu do jiné scény
        }
        else
        {
            Destroy(gameObject); // znièí duplicitní instanci
        }
    }

    void Start()
    {
        LoadCorridors();

        player = GameObject.FindGameObjectWithTag("Player");
        jumpScareWayPoint = GameObject.FindGameObjectWithTag("JumpScareWP");

        if (jumpScareWayPoint == null) 
        {
            Debug.Log("CorridorManager: jumpScare Point!!");
        }
    }

    public GameObject GetRandomNonPlayerRoom()
    {
        //Debug.Log("poèet chodeb: " + corridors.Count);
        corridors.Remove(currentCorridor);
        //Debug.Log("poèet chodeb: " + corridors.Count);
        int index = Random.Range(0, corridors.Count);
        corridors.Add(currentCorridor);
        //Debug.Log("poèet chodeb: " + corridors.Count);
        return corridors[index];
    }

    public GameObject GetClosestNonPlayerRoom()
    {
        float min = float.MaxValue;
        int index = 0;

        corridors.Remove(currentCorridor);
        for (int i = 0; i < corridors.Count; i++)
        {
            if (min > Vector3.Distance(corridors[i].transform.position, player.transform.position))
            {
                min = Vector3.Distance(corridors[i].transform.position, player.transform.position);
                index = i;
            }
        }
        corridors.Add(currentCorridor);

        return corridors[index];
    }

    /// <summary>
    /// Naètì všechny chodby do listu, odkud scp173 bere chodby pro spawn
    /// </summary>
    public void LoadCorridors() 
    {
        GameObject[] objectsInScene = FindObjectsByType<GameObject>(FindObjectsSortMode.None);

        foreach (GameObject obj in objectsInScene)
        {
            if (obj.layer == LayerMask.NameToLayer("Corridors"))
            {
                corridors.Add(obj);
            }
        }

        Debug.Log("corridors count: " + corridors.Count);
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
                Debug.Log("CorridorManager: Mùže se spawnout");
                return true;
            }
            else
            {
                Debug.Log("CorridorManager: Nemùže se spawnout");
                return false;
            }
        }
        else
        {
            Debug.Log("CorridorManager: Nemùže se spawnout");
            return false;
        }
    }
}
