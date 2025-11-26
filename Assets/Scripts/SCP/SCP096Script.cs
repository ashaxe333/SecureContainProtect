using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class SCP096Script : MonoBehaviour
{
    private GameObject player;
    public NavMeshAgent scp096;
    public GameObject face;
    private bool isSeen;
    private bool triggered;

    public Transform[] waypoints;
    private int currentWayPoint;
    private int newWayPoint;

    private float timer;
    private float duration = 15.0f;
    private float followSpeed = 20.0f;
    private float patrolSpeed = 4.0f;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        scp096.speed = patrolSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        //FaceIsSeen();
        FollowPlayer();
    }

    /// <summary>
    /// Hlídá, jestli se hráè kouká smìrem na oblièej SCP-096, a pokud ho zároveò netrefuje paprsek. Pokud podmínka projde, spustí se metoda pro zabití hráèe
    /// </summary>
    void FollowPlayer()
    {
        isSeen = face.GetComponent<SCP096FaceScript>().FaceIsSeen();
        Vector3 directionToFace = (face.transform.position - player.transform.position).normalized;

        Ray ray = new Ray(player.transform.position, directionToFace);
        RaycastHit hit;
        //Debug.DrawRay(player.transform.position, directionToFace * 50.0f, Color.red);

        if (Physics.Raycast(ray, out hit, 50.0f))
        {
            Debug.Log("Trefil: " + hit.collider.gameObject.name);

            //FaceIsSeen();

            if (hit.collider.gameObject == face && isSeen)
            {
                Debug.Log("vidí!");
                triggered = true;
            }
            else
            {
                if (timer <= 0 && !triggered)
                {
                    Patrol();
                }
            }

            if (triggered)
            {
                GoToPlayer();
            }
        }
        else
        {
            if (triggered)
            {
                GoToPlayer();
            }
            else
            {
                if (timer <= 0 && !triggered)
                {
                    Patrol();
                }
            }
        }
    }

    /// <summary>
    /// Zpùsobí, že se scp096 rozbìhne na hráèe
    /// </summary>
    void GoToPlayer()
    {
        duration -= Time.deltaTime;

        if (duration <= 0)
        {
            scp096.SetDestination(player.transform.position);
            scp096.speed = followSpeed;
            KillPlayer();
        }
    }

    /// <summary>
    /// Zabije hráèe
    /// </summary>
    void KillPlayer()
    {
        if (Vector3.Distance(scp096.transform.position, player.transform.position) < 2.0f)
        {
            DeathInfoScript.msg = "You were killed by SCP-096";
            SceneManager.LoadScene(2);
        }
    }

    /*
    /// <summary>
    /// kontroluje, jestli je oblièej scp096 v poli kamery
    /// </summary>
    public void FaceIsSeen() //možná OnBecameVisible? https://docs.unity3d.com/ScriptReference/Renderer.OnBecameVisible.html
    {
        if (face.GetComponent<Renderer>().isVisible)
        {
            Debug.Log("bacha");
            isSeen = true;
        }
        else
        {
            isSeen = false;
        }
    }
    */

    /// <summary>
    /// Náhodná chùze SCP-096
    /// </summary>
    void Patrol()
    {
        if (scp096.remainingDistance < scp096.stoppingDistance + 1 && timer < 0.0f)
        {
            Debug.Log("provadím patrol");
            float reroll = Random.Range(30, 60);
            currentWayPoint = Random.Range(0, waypoints.Length);
            timer = reroll;
        }
        scp096.SetDestination(waypoints[currentWayPoint].position);
    }
}
