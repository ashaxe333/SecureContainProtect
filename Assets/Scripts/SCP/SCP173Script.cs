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

    [SerializeField] private AudioClip jumpscareSoundFX;

    private float distanceToPLayer;
    public GameObject[] prohibitedAreas;

    [SerializeField] private LayerMask raycastLayerMask;
    private float timer;
    private float jumpScareTimer = 300f;    // nesmí hráèe jumpscarenout prvních 5 minut hry
    private bool hasSeenPlayer;
    private float spawnDuration = 7.0f;
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
        if (!ProhibitedSpawnAreas()) jumpScareTimer -= Time.deltaTime;
        //Debug.Log(objectMemory);

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
                //Debug.Log("Trefil jsem hráèe");
                hasSeenPlayer = true;
                timer = spawnDuration * 2;
                playerLastPosition = player.transform.position;
            }
        }
        /*
        if(RayHitScript.instance.HitTargertFromTo(scp173.gameObject, player, 75.0f, raycastLayerMask))
        {
            //Debug.Log("Trefil jsem hráèe");
            hasSeenPlayer = true;
            timer = 30f;
            //timer = spawnDuration * 2;
            playerLastPosition = player.transform.position;
        }
        */
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
                scp173.SetDestination(playerLastPosition);
            else
            {
                hasSeenPlayer = false;
                Patrol();
            }
        }
        else
        {
            if (timer < 0)
                Patrol();
        }
    }

    /// <summary>
    /// Každý frame kontroluje, jestli je broadcast v zorném poli kamery, a pokud je trefen 
    /// rayem od hráèe (kdyby byl za sloupem tøeba, tak aby mohl jít, protože ho hráè skuteènì nevidí). 
    /// Pokud je splpnno obojí, broadcast se zastaví
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
            //Time.timeScale = 0; //Debug
            PlayerDamageManager.instance.isTaking173 = true;
        }
    }

    /// <summary>
    /// SCP173 bude náhodnì portolovat do náhodný chodby, do nejbližší chodby k hráèi, nebo vzácnì pøímo pøed hráèe
    /// </summary>
    void Patrol()
    {
        int random = Random.Range(0, 20);

        // Pokud po 15 minutách nenastal ani jeden Jumpscare, tak se stane hned
        if (jumpScareTimer < -300f) random = 0;

        switch (random)
        {
            case 0:
                //Debug.Log("SCP173Script: To player " + random);
                if (AreaManager.Instance.CanSpawn() && jumpScareTimer < 0f)
                {
                    scp173.Warp(AreaManager.Instance.jumpScareWayPoint.transform.position);
                    SoundFXManagerScript.instance.PlaySoundFX(jumpscareSoundFX, player.transform, 0.7f, 1f, 0f, 0f);
                    BlinkScript.Instance.currentTime = BlinkScript.Instance.blinkTimer;
                    jumpScareTimer = 600f;  // mùže udìlat jumpsacre znova až po 10-ti minutách
                }
                else 
                { 
                    //když nevyjde jumpscare, spawne se do nejbližší místnosti
                    scp173.Warp(AreaManager.Instance.GetClosestNonPlayerRoom().GetComponent<AreaInstanceScript>().GetRandomSpawnPoint());
                    Debug.Log("< 5 minut");
                }
                timer = spawnDuration;
                break;

            case int n when (n >= 1 && n <= 5):
                //Debug.Log("SCP173Script: To closest corridor");
                scp173.Warp(AreaManager.Instance.GetClosestNonPlayerRoom().GetComponent<AreaInstanceScript>().GetRandomSpawnPoint());
                timer = spawnDuration;
                break;

            default:
                //Debug.Log("SCP173Script: To random corridor");
                scp173.Warp(AreaManager.Instance.GetRandomNonPlayerRoom().GetComponent<AreaInstanceScript>().GetRandomSpawnPoint());
                timer = spawnDuration;
                break;
        }
    }

    /// <summary>
    /// Kontroluje, jestli se hráè nenachází v místnosti, kam se SCP173 nemùže spawnout na jumpscare
    /// </summary>
    /// <returns></returns>
    public bool ProhibitedSpawnAreas()
    {
        bool result = false;
        for (int i = 0; i < prohibitedAreas.Length; i++)
        {
            if (prohibitedAreas[i] == AreaManager.Instance.currentArea)
            {
                result = true;
                break;
            }
        }
        return result;
    }
}