using System.Collections;
using System.Collections.Generic;
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

    public Animator animator;

    private float followDistance;
    private float distanceToPlayer;
    private float distanceToTarget;

    private float returnDistance = 50f;
    private float noiseTriggerDistance = 30.0f;
    private float runTriggerDistance = 40.0f;
    private float walkTriggerDistance = 30.0f;
    private float sneakTriggerDistance = 10.0f;
    private float standTriggerDistance = 2.0f;

    private float runSpeed = 18.0f;
    private float patrolSpeed = 6.0f;

    public Transform[] waypoints;
    private int currentWaypoint = 0;
    private float timer;
    private float reroll;

    private bool isTriggered = false;
    private bool isTriggered2 = false;
    private bool isAttacking = false;
    private float attackTime = 4f;

    void Start()
    {
        timer = 1.0f;
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
        Debug.Log($"isTriggered = {isTriggered}, isTriggered2 = {isTriggered2}, isAttacking = {isAttacking}, isStopped = {scp939.isStopped}");
        timer -= Time.deltaTime;

        MoveTrigger();
        NoiseTrigger();
        Move();
        Counter();
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
            isTriggered = true;
        }
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
        timer = 15.0f;
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

    private void FollowPlayer()
    {
        Debug.Log("SCP939Script: FollowPlayer");

        scp939.isStopped = false;
        scp939.SetDestination(player.transform.position);
        scp939.speed = runSpeed;
        animator.SetFloat("Speed", 1f);

        AttackPlayer();
    }
    private void AttackPlayer()
    {
        if (distanceToPlayer <= 4f && !isAttacking)
        {
            Debug.Log($"SCP939Script: Attack!");
            animator.SetTrigger("Attack");
            //scp939.isStopped = false;
            isAttacking = true;
            //PlayerDamageManager.instance.isTaking939 = true;
            DeathInfoScript.msg = "You were killed by SCP-939";
        }
    }

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

            if (timer <= 0)
            {
                ResetPatrol();
            }
        }
    }

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
        Debug.Log($"SCP939Script: atPoint = {atPoint}");

        if (atPoint && timer < 0.0f)
        {
            Debug.Log("provadím patrol");
            reroll = Random.Range(12, 20);
            currentWaypoint = Random.Range(0, waypoints.Length);
            timer = reroll;
        }

        if(atPoint && timer > 0.0f)
        {
            Debug.Log("SCP939Script: Patrol if...");
            scp939.isStopped = true;
            animator.SetFloat("Speed", 0f);
        }  
        else
        {
            Debug.Log("SCP939Script: Patrol else...");
            scp939.isStopped = false;
            animator.SetFloat("Speed", 0.4f);
        }

        scp939.SetDestination(waypoints[currentWaypoint].position);
    }

    private void Counter()
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
