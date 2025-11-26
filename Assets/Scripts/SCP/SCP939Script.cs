using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class SCP939Script : MonoBehaviour
{
    private GameObject player;
    public NavMeshAgent scp939;

    private GameObject target;

    private float followDistance;
    private float distanceToPlayer;
    private float distanceToTarget;
    private float returnDistance = 20f;
    private float followSpeed = 13.0f;
    private float patrolSpeed = 6.0f;

    public Transform[] waypoints;
    private int currentWaypoint = 0;
    private float timer;
    private float reroll;

    private bool isTriggered = false;
    private bool isTriggered2 = false;

    private GameObject gameManager;

    // Start is called before the first frame update
    void Start()
    {
        timer = 1.0f;
        player = GameObject.FindGameObjectWithTag("Player");
        gameManager = GameObject.FindGameObjectWithTag("GameManager");

        if (player == null)
        {
            Debug.Log("hráè!");
        }
    }

    // Update is called once per frame
    void Update()
    {
        distanceToPlayer = Vector3.Distance(scp939.transform.position, player.transform.position);
        timer -= Time.deltaTime;
        Move();
    }

    /// <summary>
    /// Registruje, pokud SCP-939 uslyší pohyb hráèe (Jak daleko od nìj je, a v jakém režimu se pohybuje). Pokud je dost blízko, zaène hráèe sledovat
    /// </summary>
    /// <param name="mode">øíká, v jakém režimu se hráè pohybuje</param>
    public void MoveTrigger(float mode)
    {
        if (!isTriggered)
        {
            switch (mode)
            {
                case 0.0f:
                    followDistance = 2.0f;
                    break;

                case 0.5f:
                    followDistance = 6.0f;
                    break;

                case 1.0f:
                    followDistance = 12.0f;
                    break;

                case 2.0f:
                    followDistance = 25.0f;
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
    public void NoiseTrigger(GameObject clickedObject)
    {
        target = clickedObject;
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
        {
            scp939.speed = followSpeed;
            scp939.SetDestination(player.transform.position);
            if(distanceToPlayer <= 2.0f)
            {
                DeathInfoScript.msg = "You were killed by SCP-939";
                SceneManager.LoadScene(2);
            }
        }
        else if (isTriggered2 && distanceToTarget <= 30.0f)
        {
            scp939.speed = followSpeed;
            distanceToTarget = Vector3.Distance(scp939.transform.position, target.transform.position);
            scp939.SetDestination(target.transform.position);

            if(distanceToTarget <= 3.0f)
            {
                scp939.isStopped = true;
                if(timer <= 0)
                {
                    scp939.isStopped = false;
                    scp939.speed = patrolSpeed;
                    isTriggered2 = false;
                    Patrol();
                }
            }
        }
        else
        { 
            scp939.speed = patrolSpeed;
            isTriggered = false;
            Patrol();
        }
    }

    /// <summary>
    /// SCP-939 chodí mezi body
    /// </summary>
    void Patrol()
    {
        /*
        // do koleèka
        if (scp939.remainingDistance < scp939.stoppingDistance + 1)
        {
            currentWaypoint = (currentWaypoint + 1) % F1waypoints.Length;     //restartuje currenWaypoint na 0
        }
        scp939.SetDestination(F1waypoints[currentWaypoint].position);

        // tam zpátky
        if (scp939.remainingDistance < scp939.stoppingDistance + 1)
        {
            if (F1waypoints[currentWaypoint].CompareTag("MainWayPoint"))
            {

            }
            currentWaypoint = currentWaypoint + 1;
        }
        scp939.SetDestination(F1waypoints[currentWaypoint].position);
        */

        // random
        if (scp939.remainingDistance < scp939.stoppingDistance + 1 && timer < 0.0f)
        {
            Debug.Log("provadím patrol");
            reroll = Random.Range(10, 16);
            currentWaypoint = Random.Range(0, waypoints.Length);
            timer = reroll;
        }
        scp939.SetDestination(waypoints[currentWaypoint].position);
    }
}
