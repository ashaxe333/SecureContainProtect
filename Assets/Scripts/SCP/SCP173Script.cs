using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using static UnityEngine.GraphicsBuffer;

public class SCP173Script : MonoBehaviour
{
    // UKOLY:
    // 1) Na jumpscare zavolat Light Manager, a na každý zapnutý svìtlo zavolat metodu, kde probliknou, a spustí se flickering.
    //    S tím zavolat sound pro jumpscare.

    private GameObject player;
    public Camera playerCamera;
    private BlinkScript blinkScript;

    public NavMeshAgent scp173;
    public GameObject child;
    private Renderer scp173Renderer;
    private Transform scp173Transform;

    private float distanceToPLayer;
    //public GameObject startingSpawn;

    [SerializeField] private LayerMask raycastLayerMask;
    private float timer;
    private bool hasSeenPlayer;
    private float spawnDuration = 15.0f;
    private float killDistance = 3.0f;      // dobrý tøeba pro nastavení obtížnosti. 4f už nic neodpustí, 3f je ještì "milý"
    Vector3 playerLastPosition;

    private GameObject hitObject;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        blinkScript = player.GetComponent<BlinkScript>();
        //playerCamera = Camera.main;

        scp173Renderer = child.GetComponent<Renderer>();
        scp173Transform = child.GetComponent<Transform>();
        timer = spawnDuration;

        scp173.Warp(AreaManager.Instance.GetRandomNonPlayerRoom().GetComponent<AreaInstanceScript>().GetRandomSpawnPoint());
    }

    void Update()
    {
        timer -= Time.deltaTime;
        //Debug.Log(timer);

        FollowPlayer();
        IsKilled();
        IsSeen();
    }

    /// <summary>
    /// Hlídá, jestli se hráè nevyskytuje v dosahu. Pokud jo, bude hráèe sledovat na poslední místo, kde byl vidìn
    /// </summary>
    void FollowPlayer()
    {
        distanceToPLayer = Vector3.Distance(scp173.transform.position, player.transform.position);
        Vector3 directionToPlayer = (player.transform.position - scp173.transform.position).normalized;

        Ray ray = new Ray(scp173.transform.position, directionToPlayer);
        RaycastHit hit;

        Debug.DrawRay(scp173.transform.position, directionToPlayer * 100.0f, Color.red);

        if (Physics.Raycast(ray, out hit, 75.0f, raycastLayerMask/*Physics.DefaultRaycastLayers, QueryTriggerInteraction.Ignore*/))
        {
            //Debug.Log("Trefil jsem: " + hit.collider.gameObject.name);
            hitObject = hit.collider.gameObject;

            if (hitObject == player)
            {
                //Debug.Log("Trefil jsem hráèe!!! ZABÍT!!!");
                hasSeenPlayer = true;
                timer = spawnDuration * 2;
                playerLastPosition = player.transform.position;
            }
        }

        Hit();
    }

    /// <summary>
    /// Pokud byl hráè vidìt, sleduje ho na jeho poslední pozici, kde byl zahlídnut. Pokud nebyl, patroluje
    /// </summary>
    void Hit()
    {
        if (hasSeenPlayer)
        {
            if (timer > 0)
            {
                //timer -= Time.deltaTime;
                scp173.SetDestination(playerLastPosition);
            }
            else
            {
                hasSeenPlayer = false;
                Patrol();
            }
        }
        else
        {
            if (timer < 0)
            {
                Patrol();
            }
        }
    }

    /// <summary>
    /// Každý frame kontroluje, jestli je scp173 v zorném poli kamery, a pokud je trefen 
    /// rayem od hráèe (kdyby byl za sloupem tøeba, tak aby mohl jít, protože ho hráè skuteènì nevidí). 
    /// Pokud je splpnno obojí, scp173 se zastaví
    /// </summary>
    
    public void IsSeen()
    {
        if (scp173Renderer.isVisible && !blinkScript.isBlinking)
        {
            //Debug.Log($"SCP173: renderer = {scp173Renderer.isVisible}");
            //Debug.Log($"SCP173: Is Seen");
            scp173.isStopped = true;
            scp173.ResetPath();
            scp173.velocity = Vector3.zero;
            scp173.nextPosition = scp173.transform.position;
        }
        else
        {
            //Debug.Log($"SCP173: renderer = {scp173Renderer.isVisible}");
            //Debug.Log($"SCP173: Isn't Seen");
            scp173.isStopped = false;
        }
    }

    /// <summary>
    /// Pokud se scp-173 pøiblíží dost blízko a zároveò ho hráè nevidí, zabije hráèe
    /// </summary>
    public void IsKilled()
    {
        //Debug.Log($"{distanceToPLayer} : renderer.isVisible - {scp173Renderer.isVisible}");
        //Debug.Log($"SCP173: hitObject == player - {hitObject == player}");

        if ((!scp173Renderer.isVisible || blinkScript.isBlinking) && distanceToPLayer <= killDistance && hitObject == player)
        {
            DeathInfoScript.msg = "You were killed by SCP-173";
            //Time.timeScale = 0; Debug
            PlayerDamageManager.instance.isTaking173 = true;
        }
    }

    /// <summary>
    /// SCP173 se bude náhodnì portovat do buïto náhodný chodby, do nejbližší chodby k hráèi, nebo vzácnì pøímo pøed hráèe
    /// </summary>
    void Patrol()
    {
        int random = Random.Range(0, 20);

        switch (random)
        {
            case 0:
                //Debug.Log("SCP173Script: To player " + random);
                if (AreaManager.Instance.CanSpawn()) 
                    scp173.Warp(AreaManager.Instance.jumpScareWayPoint.transform.position);
                else 
                    scp173.Warp(AreaManager.Instance.GetClosestNonPlayerRoom().GetComponent<AreaInstanceScript>().GetRandomSpawnPoint()); //když nevyjde jumpscare, spawne se do nejbližší místnosti
                timer = spawnDuration;
                break;

            case int n when (n >= 1 && n <= 5):
                //Debug.Log("SCP173Script: To closest corridor");
                scp173.Warp(AreaManager.Instance.GetClosestNonPlayerRoom().GetComponent<AreaInstanceScript>().GetRandomSpawnPoint());
                timer = spawnDuration;
                break;

            default:
                //Debug.Log("SCP173Script: To random corridor");    GetComponent<AreaInstanceScript>().
                scp173.Warp(AreaManager.Instance.GetRandomNonPlayerRoom().GetComponent<AreaInstanceScript>().GetRandomSpawnPoint());
                timer = spawnDuration;
                break;
        }
    }
}