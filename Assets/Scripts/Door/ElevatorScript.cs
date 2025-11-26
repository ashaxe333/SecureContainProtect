using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.XR;

public class ElevatorSript : MonoBehaviour
{
    public GameObject upButton;
    public GameObject downButton;
    public GameObject doorF0;
    public GameObject doorF1;
    public GameObject doorF2;
    public GameObject elevator;

    private GameObject player;
    private GameObject currentDoor;
    private DoorScript doorScript;
    private GameObject clickedObject;

    private int nextFloor = 1;
    public int currentFloor = 1;
    public bool elevatorIsBroken = false;

    private Coroutine coroutine;
    private float timer = 9;
    private bool port;
    [HideInInspector] public bool called;
    private bool hasPorted = false;
    private float portLength;

    private int direction;
    [HideInInspector] public int destination;

    void Start()
    {
        SetupCurrentDoor();
        SetupCurrentFloor();
        player = GameObject.FindGameObjectWithTag("Player");
        doorScript = currentDoor.GetComponent<DoorScript>();
        currentDoor.GetComponent<NavMeshObstacle>().enabled = true;
        DoorMove(false);
    }

    void Update()
    {
        HandleDoorInteraction();
        PortElevator();
        CallElevator();
    }

    /// <summary>
    /// Kontroluje, jestli, a na které tlaèítko hráè klikl. Podle toho vyhodnotí, jesti pojede nahoru nebo dolu
    /// </summary>
    public void HandleDoorInteraction()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 5.0f, 1 << 9))
        {
            Debug.Log("ElevatorScript: dolu/nahoru");

            if (Input.GetMouseButtonDown(0))
            {
                Debug.Log("ElevatorScript: hitnul jsem button");
                clickedObject = hit.collider.gameObject;

                if (clickedObject == upButton || clickedObject == downButton)
                {
                    if (elevatorIsBroken)
                    {
                        GameManagerScript.gameManagerInstance.GetTextInfo("elevator is broken");
                    }
                    else if ((clickedObject == upButton && currentFloor < 2) || (clickedObject == downButton && currentFloor > 0))
                    {
                        port = true;
                        downButton.GetComponent<BoxCollider>().enabled = false;
                        upButton.GetComponent<BoxCollider>().enabled = false;
                        DoorMove(true);
                    }
                    else
                    {
                        //pøehraje sound, že to nejde
                        Debug.Log("ElevatorScript: Nejde dál");
                    }
                }
            }
        }
    }

    /// <summary>
    /// Kontroluje, jestli hráè mùže dveøe otevøít
    /// </summary>
    /// <param name="closedDoorClickedButton"> 
    /// chrání dveøe zpùsobem, že když by hráè nabìhl do výtahu, a stihl za sebou zavøít dveøe, tak button by zavolal metodu DoorMove(), dveøe by se otevøeli, a výtah by se "rozjel".
    /// Takže toto zpùsobí, že se dveøe, v tomto pøípadì, neotevøou
    /// </param>
    void DoorMove(bool closedDoorClickedButton)
    {
        if (doorScript.isOpen && doorScript.isActive)
        {
            currentDoor.GetComponent<NavMeshObstacle>().enabled = true;
            coroutine = StartCoroutine(doorScript.DoSlidingClose());
        }
        else if (!doorScript.isOpen && doorScript.isActive && !closedDoorClickedButton)
        {
            currentDoor.GetComponent<NavMeshObstacle>().enabled = false;
            coroutine = StartCoroutine(doorScript.DoSlidingOpen());
        }
    }

    /// <summary>
    /// Pøenáší výtah s hráèem po kliknutí na button
    /// </summary>
    void PortElevator()
    {
        if (port) timer -= Time.deltaTime;

        if (timer <= 1 && !hasPorted)
        {
            hasPorted = true;

            if (clickedObject == downButton)
            {
                nextFloor -= 1;
                SetupCurrentFloor();
                Debug.Log("ElevatorScript: vzdálenost = " + portLength);
                elevator.transform.position = new Vector3(elevator.transform.position.x, elevator.transform.position.y - portLength, elevator.transform.position.z);
                player.GetComponent<CharacterController>().enabled = false;
                player.transform.position = new Vector3(player.transform.position.x, player.transform.position.y - portLength, player.transform.position.z);
                player.GetComponent<CharacterController>().enabled = true;
                currentFloor = nextFloor;
            }
            else if (clickedObject == upButton)
            {
                nextFloor += 1;
                SetupCurrentFloor();
                Debug.Log("ElevatorScript: vzdálenost = " + portLength);
                elevator.transform.position = new Vector3(elevator.transform.position.x, elevator.transform.position.y + portLength, elevator.transform.position.z);
                player.GetComponent<CharacterController>().enabled = false;
                player.transform.position = new Vector3(player.transform.position.x, player.transform.position.y + portLength, player.transform.position.z);
                player.GetComponent<CharacterController>().enabled = true;
                currentFloor = nextFloor;
            }
        }

        if (timer <= 0)
        {
            downButton.GetComponent<BoxCollider>().enabled = true;
            upButton.GetComponent<BoxCollider>().enabled = true;
            port = false;
            hasPorted = false;
            timer = 9.0f;
            DoorMove(false);
        }
    }

    /// <summary>
    /// Zavolá výtah na patro, kde jsem
    /// </summary>
    /// <param name="destination">na jaké patro se má výtah dostavit</param>
    /// <returns>vrací, že volám, nebo že už nevolám výtah</returns>
    public void CallElevator()
    {
        if (called) timer -= Time.deltaTime;

        if (timer <= 1 && !hasPorted)
        {
            if (doorScript.isOpen)
            {
                DoorMove(false);
            }

            hasPorted = true;
            nextFloor = destination;
            SetupCurrentFloor();

            if (direction == 0)
            {
                Debug.Log($"ElevatorScript: direction = {direction}, vzdálenost = {portLength}");
                elevator.transform.position = new Vector3(elevator.transform.position.x, elevator.transform.position.y - portLength, elevator.transform.position.z);
                currentFloor = nextFloor;
            }
            else if (direction == 1)
            {
                Debug.Log($"ElevatorScript: direction = {direction}, vzdálenost = {portLength}");
                elevator.transform.position = new Vector3(elevator.transform.position.x, elevator.transform.position.y + portLength, elevator.transform.position.z);
                currentFloor = nextFloor;
            }
        }

        if (timer <= 0)
        {
            called = false;
            hasPorted = false;
            timer = 9.0f;
            DoorMove(false);
            Debug.Log("otevírám");
        }
    }

    /// <summary>
    /// Používá se hlavnì pøi cestování mezi patry. Nastavuje, o kolik se výtah portne, currentDoor a pro volání výtahu i smìr, jakým pojede ke hráèi
    /// </summary>
    public void SetupCurrentFloor()
    {
        switch ((currentFloor, nextFloor))
        {
            case (1, 0):
                Debug.Log("patro0");
                portLength = 96.0f;
                currentDoor = doorF0;
                direction = 0;
                break;

            case (0, 1):
                Debug.Log("patro1");
                portLength = 96.0f;
                currentDoor = doorF1;
                direction = 1;
                break;

            case (1, 2):
                Debug.Log("patro2");
                portLength = 24.0f;
                currentDoor = doorF2;
                direction = 1;
                break;

            case (2, 1):
                Debug.Log("patro1");
                portLength = 24.0f;
                currentDoor = doorF1;
                direction = 0;
                break;

            case (0, 2):
                Debug.Log("patro2");
                portLength = 120.0f;
                currentDoor = doorF2;
                direction = 1;
                break;

            case (2, 0):
                Debug.Log("patro0");
                portLength = 120.0f;
                currentDoor = doorF0;
                direction = 0;
                break;

            default:
                Debug.Log("žádný patro");
                break;
        }

        doorScript = currentDoor.GetComponent<DoorScript>();
    }

    /// <summary>
    /// Na zaèátku nastaví currentDoor
    /// </summary>
    void SetupCurrentDoor()
    {
        if (currentFloor == 0) currentDoor = doorF0;
        else if (currentFloor == 1) currentDoor = doorF1;
        else if (currentFloor == 2) currentDoor = doorF2;
        //else currentDoor = doorF3;
    }
}