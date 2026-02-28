using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class SCP939Script : MonoBehaviour
{
    private GameObject player;
    private PlayerController playerController;
    private PlayerInteractScript playerInteractScript;
    [HideInInspector] public NavMeshAgent scp939;
    private GameObject target;
    [SerializeField] private LayerMask raycastLayerMask;
    public Animator animator;
    private GameObject hitObject;

    private float followDistance;
    private float distanceToPlayer;
    private float distanceToTarget;

    private float returnDistance = 50.0f;
    private float noiseTriggerDistance = 30.0f;
    private float runTriggerDistance = 40.0f;
    private float walkTriggerDistance = 30.0f;
    private float sneakTriggerDistance = 10.0f;
    private float standTriggerDistance = 2.0f;

    private float playerMemory = 0.0f;
    private float playerMemoryDuration = 30.0f;
    private float objectMemory = 0.0f;

    private float runSpeed = 18.0f;
    private float patrolSpeed = 6.0f;

    public Transform[] waypoints;
    private int currentWaypoint = 0;
    private float reroll;

    private bool isTriggered = false;
    private bool isTriggered2 = false;
    private bool isAttacking = false;
    private float attackTime = 4f;
    private float cooldown;

    void Start()
    {
        objectMemory = 1.0f;
        player = GameObject.FindGameObjectWithTag("Player");
        playerController = player.GetComponent<PlayerController>();
        playerInteractScript = player.GetComponent<PlayerInteractScript>();

        scp939 = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        if (player == null)
            Debug.Log("SCP939Script: Assing player!!!");
    }

    void Update()
    {
        distanceToPlayer = Vector3.Distance(scp939.transform.position, player.transform.position);
        //Debug.Log($"isTriggered = {isTriggered}, isTriggered2 = {isTriggered2}, isAttacking = {isAttacking}, isStopped = {scp939.isStopped}");
        objectMemory -= Time.deltaTime;
        playerMemory -= Time.deltaTime;
        cooldown -= Time.deltaTime;

        if (playerMemory <= 0f)
            isTriggered = false;
        //Debug.Log($"SCP939Script: cooldown = {cooldown}");

        MoveTrigger();
        NoiseTrigger();
        Move();
        AttackTimeCounter();
    }

    /// <summary>
    /// Registruje, pokud SCP-939 uslyší pohyb hráèe (Jak daleko od nìj je, a v jakém režimu se pohybuje). Pokud je dost blízko, zaène hráèe sledovat
    /// </summary>
    /// <param name="mode">øíká, v jakém režimu se hráè pohybuje</param>
    public void MoveTrigger()
    {
        if (!isTriggered)
        {
            switch (playerController.movement)
            {
                case 0.0f:
                    followDistance = standTriggerDistance;
                    break;

                case 0.5f:
                    followDistance = sneakTriggerDistance;
                    break;

                case 1.0f:
                    followDistance = walkTriggerDistance;
                    break;

                case 2.0f:
                    followDistance = runTriggerDistance;
                    break;
            }
        }

        
        if (distanceToPlayer <= followDistance)
        {
            if (HasHearingContact())
            {
                isTriggered = true;
                playerMemory = playerMemoryDuration;
            }
        }
    }


    /// <summary>
    /// Zastaví pronásledování hráèe, pokud je hráè za stìnou schovaný více jak 30 vteøin
    /// </summary>
    /// <returns> bool </returns>
    private bool HasHearingContact()
    {
        Vector3 directionToPlayer = (player.transform.position - scp939.transform.position).normalized;

        Ray ray = new Ray(scp939.transform.position, directionToPlayer);
        RaycastHit hit;

        Debug.DrawRay(scp939.transform.position, directionToPlayer * 100.0f, Color.red);

        if (Physics.Raycast(ray, out hit, followDistance, raycastLayerMask))
        {
            //Debug.Log("Trefil jsem: " + hit.collider.gameObject.name);
            hitObject = hit.collider.gameObject;

            if (hitObject == player)
                return true;
            else
                return false;
        }
        else
            return false;
    }

    /// <summary>
    /// Registruje, pokud hráè nìjak interaguje s pøedmìtem
    /// </summary>
    public void NoiseTrigger()
    {
        if (playerInteractScript.clickedObject == null) return;

        target = playerInteractScript.clickedObject;
        distanceToTarget = Vector3.Distance(scp939.transform.position, target.transform.position);
        isTriggered2 = true;
        objectMemory = Random.Range(10, 16);
    }

    /// <summary>
    /// Pokud metoda MoveTriggered nastaví bool isTriggered na true, SCP-939 zaène hráèe sedovat, dokud se mu neztratí a nenastaví isTriggered zpìt na false. 
    /// Pokud isTriggered je false, SCP-939 patroluje
    /// To samé s isTriggered2. Ten nastavuje metoda NoiseTrigger na true, pokud nìco udìlám s objektem, a SCP-939 je v urèité vzdálenosti
    /// </summary>
    void Move()
    {
        if (isTriggered && distanceToPlayer < returnDistance)
            FollowPlayer();
        else if (isTriggered2 && distanceToTarget <= noiseTriggerDistance)
            RunToNoise();
        else
            ResetPatrol();
    }

    /// <summary>
    /// Zaène hráèe sledovat
    /// </summary>
    private void FollowPlayer()
    {
        //Zavíst objectMemory, který, když dojde, pøestane mì sledovat
        Debug.Log("SCP939Script: FollowPlayer");
        if(playerMemory <= 0)
        {
            isTriggered = false;
            return;
        }

        scp939.isStopped = false;
        //Vector3 reachablePosition = GetReachablePosition(player.transform.position);
        //scp939.SetDestination(reachablePosition);
        scp939.SetDestination(player.transform.position);
        scp939.speed = runSpeed;
        animator.SetFloat("Speed", 1f);

        AttackPlayer();
    }

    /*
    private Vector3 GetReachablePosition(Vector3 targetPos)
    {
        NavMeshPath path = new NavMeshPath();

        if (scp939.CalculatePath(targetPos, path))
        {
            if (path.status == NavMeshPathStatus.PathComplete)
            {
                Debug.Log("SCP939Script: GetReachablePosition - PathComplete");
                return targetPos;
            }

            if (path.corners.Length > 0)
            {
                Debug.Log("SCP939Script: GetReachablePosition - PathIncomplete");
                return path.corners[path.corners.Length - 1];
            }
        }

        Debug.Log("SCP939Script: scp939.CalculatePath(targetPos, path) = false");
        return scp939.transform.position;
    }
    */

    /// <summary>
    /// Vyvolá útok na hráèe
    /// </summary>
    private void AttackPlayer()
    {
        if (distanceToPlayer <= 4f && !isAttacking)
        {
            Debug.Log($"SCP939Script: Attack!");
            animator.SetTrigger("Attack");
            isAttacking = true;
            if(cooldown <= 0)
            {
                Debug.Log($"jen jednou!");
                cooldown = 2.0f;
                PlayerDamageManager.instance.isTaking939 = true;
            }
        }
    }

    /// <summary>
    /// Bìží za zvukem, který zaslechl (zpùsobeno hráèem)
    /// </summary>
    private void RunToNoise()
    {
        scp939.isStopped = false;
        scp939.speed = runSpeed;
        distanceToTarget = Vector3.Distance(scp939.transform.position, target.transform.position);
        scp939.SetDestination(target.transform.position);
        animator.SetFloat("Speed", 1f);
        Debug.Log("SCP939Script: RunToNoise...");

        if (distanceToTarget <= 3.0f)
        {
            scp939.isStopped = true;
            animator.SetFloat("Speed", 0f);
            Debug.Log("SCP939Script: RunToNoise if...");

            if (objectMemory <= 0)
            {
                ResetPatrol();
            }
        }
    }

    /// <summary>
    /// Vrátí SCP939 zpìt do Patrol state
    /// </summary>
    private void ResetPatrol()
    {
        scp939.isStopped = false;
        scp939.speed = patrolSpeed;
        isTriggered = false;
        isTriggered2 = false;
        Patrol();
    }

    /// <summary>
    /// SCP-939 chodí mezi body
    /// </summary>
    void Patrol()
    {
        bool atPoint = scp939.remainingDistance < scp939.stoppingDistance + 1;
        //Debug.Log($"SCP939Script: atPoint = {atPoint}");

        if (atPoint && objectMemory < 0.0f)
        {
            //Debug.Log("provadím patrol");
            reroll = Random.Range(12, 20);
            currentWaypoint = Random.Range(0, waypoints.Length);
            objectMemory = reroll;
        }

        if(atPoint && objectMemory > 0.0f)
        {
            //Debug.Log("SCP939Script: Patrol if...");
            scp939.isStopped = true;
            animator.SetFloat("Speed", 0f);
        }  
        else
        {
            //Debug.Log("SCP939Script: Patrol else...");
            scp939.isStopped = false;
            animator.SetFloat("Speed", 0.4f);
        }

        scp939.SetDestination(waypoints[currentWaypoint].position);
    }

    /// <summary>
    /// Odpoèítává èas, jak dlouho scp939 útoèí
    /// </summary>
    private void AttackTimeCounter()
    {
        if (!isAttacking) return;

        attackTime -= Time.deltaTime;
        scp939.isStopped = true;

        if (attackTime <= 0)
        {
            isAttacking = false;
            scp939.isStopped = false;
            attackTime = 2f;
        }
    }
}
