using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.AI;

public class LockScript : MonoBehaviour
{
    // UPRAVIT!!!
    // 1) ? místo door sem dát parentWall, odkud získám nìjak (tøeba pøes skript) door, PØÍPADNÌ doorLeft a doorRight ?
    // 2) v budoucnu nedìlat pohyb dveøí pøes coroutine, ale pøes ANIMACI.

    // Base
    private GameObject player;
    public GameObject door;
    private DoorScript doorScript;
    private InventoryScript inventoryScript;
    private Coroutine coroutine;

    // Elevator
    public int floor;
    public GameObject elevator;
    private ElevatorSript elevatorScript;

    //public AudioSource audioSource;
    //public AudioClip doorOpeningAudio;
    //public AudioClip doorClosingAudio;

    void Start()
    {
        doorScript = door.GetComponent<DoorScript>();
        SetupForElevator();
        player = GameObject.FindGameObjectWithTag("Player");
        inventoryScript = player.GetComponent<InventoryScript>();
        door.GetComponent<NavMeshObstacle>().enabled = true;

        //audioSource = GetComponent<AudioSource>();
        //doorOpeningAudio = Resources.Load<AudioClip>("Sounds/");
    }

    /// <summary>
    /// Kontroluje, jestli jsem klikl na zámek od dveøí. Pokud jo, a pokud je možné dveøe otevøít, zavolá metodu DoorMove()
    /// </summary>
    /// <param name="clickedObject">kliknutý zámek</param>
    public void HandleDoorInteraction()
    {
        if (doorScript.isBroken)
        {
            GameManagerScript.gameManagerInstance.GetTextInfo("Door seems to be broken");
        }
        else if (doorScript.lowestKeyCardLevel > 0)
        {
            Debug.Log("DoorScript: Má hráè keycard? " + inventoryScript.HasKeyCard(doorScript.lowestKeyCardLevel));
            if (inventoryScript.HasKeyCard(doorScript.lowestKeyCardLevel))
            {
                DoorCheck();
            }
            else
            {
                GameManagerScript.gameManagerInstance.GetTextInfo("A better key card is required");
            }
        }
        else
        {
            DoorCheck();
        }
    }

    void DoorCheck()
    {
        if (doorScript.doorType == DoorType.ELEVATOR && elevatorScript.elevatorIsBroken)
        {
            GameManagerScript.gameManagerInstance.GetTextInfo("Elevator is broken");
            Debug.Log("LockScript: Elevator is broken");
        }
        else if (doorScript.doorType == DoorType.ELEVATOR && floor != elevatorScript.currentFloor)
        {
            elevatorScript.destination = floor;
            elevatorScript.called = true;
            Debug.Log("LockScript: Elevator was called");
            GameManagerScript.gameManagerInstance.GetTextInfo("Elevator was called");
        }
        else
        {
            DoorMove();
            Debug.Log("LockScript: open");
        }
    }

    /// <summary>
    /// Kontroluje, jestli hráè mùže dveøe otevøít, a volá metodu pro samotné otevírání dveøí
    /// </summary>
    void DoorMove()
    {
        if (doorScript.isOpen && doorScript.isActive)
        {
            door.GetComponent<NavMeshObstacle>().enabled = true;
            coroutine = StartCoroutine(doorScript.DoSlidingClose());
        }
        else if (!doorScript.isOpen && doorScript.isActive)
        {
            door.GetComponent<NavMeshObstacle>().enabled = false;
            coroutine = StartCoroutine(doorScript.DoSlidingOpen());
        }
        else
        {
            // Zvuk pøi spamování
        }
    }

    void SetupForElevator()
    {
        if (doorScript.doorType == DoorType.ELEVATOR)
        {
            elevatorScript = elevator.GetComponent<ElevatorSript>();
        }
    }
}
