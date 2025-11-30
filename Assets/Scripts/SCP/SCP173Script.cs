using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using static UnityEngine.GraphicsBuffer;

public class SCP173Script : MonoBehaviour
{
    private GameObject player;
    public Camera playerCamera;
    public NavMeshAgent scp173;
    public GameObject child;
    private Renderer scp173Renderer;
    private Transform scp173Transform;

    private float distanceToPLayer;
    public GameObject startingSpawn;

    [SerializeField] private LayerMask raycastLayerMask;
    private float timer;
    private bool hasSeenPlayer;
    private float spawnDuration = 15.0f;
    Vector3 playerLastPosition;

    private GameObject hitObject;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        //playerCamera = Camera.main;
        scp173Renderer = child.GetComponent<Renderer>();
        scp173Transform = child.GetComponent<Transform>();
        timer = spawnDuration;

        scp173.Warp(startingSpawn.transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        //Debug.Log(timer);

        FollowPlayer();
        IsKilled();
        IsSeen();
    }

    /// <summary>
    /// Hlídá, jestli se hráè nevyskytuje v dosahu. Pokud jo, bude hráèe sledovat na poslední místo, kde byl vidìt
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

            if (hit.collider.gameObject == player)
            {
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
        if (scp173Renderer.isVisible && !player.GetComponent<BlinkScript>().isBlinking)
        {
            scp173.isStopped = true;
            scp173.ResetPath();
            scp173.velocity = Vector3.zero;
            scp173.nextPosition = scp173.transform.position;
        }
        else
        {
            scp173.isStopped = false;
        }
    }
    
    /*
    public void IsSeen()
    {
        Vector3 viewportPos = playerCamera.WorldToViewportPoint(scp173Transform.position);

        bool inFront = viewportPos.z > 0;
        bool insideViewport = viewportPos.x > 0 && viewportPos.x < 1 &&
                              viewportPos.y > 0 && viewportPos.y < 1;

        bool isInCameraView = inFront && insideViewport;

        // Raycast od hráèe k SCP-173
        Vector3 dirToSCP = (scp173Transform.position - player.transform.position).normalized;
        float distToSCP = Vector3.Distance(player.transform.position, scp173Transform.position);
        bool lineOfSight = !Physics.Raycast(player.transform.position, dirToSCP, distToSCP, raycastLayerMask);

        bool isVisible = isInCameraView && lineOfSight && !player.GetComponent<BlinkScript>().isBlinking;

        scp173.isStopped = isVisible;

        if (isVisible)
            Debug.Log("Kamera vidí 173!");
        else
            Debug.Log("Kamera nevidí 173.");
    }

    */

    /// <summary>
    /// Pokud se scp-173 pøiblíží dost blízko a zároveò ho hráè nevidí, zabije hráèe
    /// </summary>
    public void IsKilled()
    {
        Debug.Log($"{distanceToPLayer} : {player.GetComponent<BlinkScript>().isBlinking}");
        if ((!scp173Renderer.isVisible || player.GetComponent<BlinkScript>().isBlinking) && distanceToPLayer <= 3.0f)
        {
            DeathInfoScript.msg = "You were killed by SCP-173";
            SceneManager.LoadScene(2);
        }
    }

    /// <summary>
    /// SCP173 se bude náhodnì portovat do buïto náhodný chodby, do nejbližší chodby k hráèi, nebo vzácnì pøímo pøed hráèe
    /// </summary>
    void Patrol()
    {
        int random = Random.Range(0, 20);
        //Debug.Log("SCP173Script: " + random + " (random number)");

        switch (random)
        {
            case 0:
                Debug.Log("CorridorManager: To player");
                if (CorridorManager.corridorManagerInstance.CanSpawn()) scp173.Warp(CorridorManager.corridorManagerInstance.jumpScareWayPoint.transform.position);
                else scp173.Warp(CorridorManager.corridorManagerInstance.GetClosestNonPlayerRoom().GetComponent<CorridorInstanceScript>().GetRandomSpawnPoint()); //když nevyjde jumpscare, spawne se do nejbližší místnosti
                timer = spawnDuration;
                break;

            case int n when (n >= 1 && n <= 5):
                Debug.Log("CorridorManager: To closest corridor");
                scp173.Warp(CorridorManager.corridorManagerInstance.GetClosestNonPlayerRoom().GetComponent<CorridorInstanceScript>().GetRandomSpawnPoint());
                timer = spawnDuration;
                break;

            default:
                Debug.Log("CorridorManager: To random corridor");
                scp173.Warp(CorridorManager.corridorManagerInstance.GetRandomNonPlayerRoom().GetComponent<CorridorInstanceScript>().GetRandomSpawnPoint());
                timer = spawnDuration;
                break;
        }
    }
}