using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.AI;

public class LockScript : MonoBehaviour
{
    // UPRAVIT!!!
    // 1) ? místo soundSourceTransform sem dát parentWall, odkud získám nìjak (tøeba pøes skript) soundSourceTransform, PØÍPADNÌ doorLeft a doorRight ?
    // 2) v budoucnu nedìlat pohyb dveøí pøes coroutine, ale pøes ANIMACI.

    // Base
    private GameObject player;
    public List<DoorScript> doors = new List<DoorScript>();
    //public GameObject door;
    private DoorScript MainDoor => doors[0];    //property: '=> doors[0]' je to samé, jako '{ get { return doors[0]; } }'
    private DoorScript doorScript;
    private InventoryScript inventoryScript;
    private Coroutine coroutine;

    [SerializeField] private AudioClip lockClickSFX;
    [SerializeField] private AudioClip lockErrorSFX;

    // Elevator
    public int floor;
    public GameObject elevator;
    private ElevatorSript elevatorScript;

    //public AudioSource audioSource;
    //public AudioClip doorOpeningAudio;
    //public AudioClip doorClosingAudio;

    void Start()
    {
        if (doors.Count == 0)
        {
            //doors.Add(door.GetComponent<DoorScript>());
            Debug.LogError($"LockScript na {name} nemá pøiøazené žádné dveøe!");

            return;
        }
        SetupForElevator();
        player = GameObject.FindGameObjectWithTag("Player");
        inventoryScript = player.GetComponent<InventoryScript>();

        //door.GetComponent<NavMeshObstacle>().enabled = true;

        //audioSource = GetComponent<AudioSource>();
        //doorOpeningAudio = Resources.Load<AudioClip>("Sounds/");
    }

    /// <summary>
    /// Kontroluje, jestli jsem klikl na zámek od dveøí. Pokud jo, a pokud je možné dveøe otevøít, zavolá metodu DoorMove()
    /// </summary>
    /// <param name="clickedObject">kliknutý zámek</param>
    public void HandleDoorInteraction()
    {
        SoundFXManagerScript.instance.PlaySoundFX(lockClickSFX, gameObject.transform, 0.1f, 1f, 0f, 0f); //0.02

        if (MainDoor.isBroken)
        {
            GameManagerScript.Instance.SetTextInfo("Door seems to be broken or inactive");
            SoundFXManagerScript.instance.PlaySoundFX(lockErrorSFX, gameObject.transform, 0.3f, 1f, 0f, 0f); //0.05
        }
        else if (MainDoor.lowestKeyCardLevel > 0)
        {
            //Debug.Log("DoorScript: Má hráè keycard? " + inventoryScript.IsKeyCardActive(MainDoor.lowestKeyCardLevel));
            if (inventoryScript.IsKeyCardActive(MainDoor.lowestKeyCardLevel))
            {
                DoorCheck();
            }
            else
            {
                GameManagerScript.Instance.SetTextInfo("A better key card is required");
                SoundFXManagerScript.instance.PlaySoundFX(lockErrorSFX, gameObject.transform, 0.3f, 1f, 0f, 0f); //0.05
            }
        }
        else
        {
            DoorCheck();
        }
    }

    void DoorCheck()
    {
        if (MainDoor.doorType == DoorType.ELEVATOR && elevatorScript.elevatorIsBroken)
        {
            GameManagerScript.Instance.SetTextInfo("Elevator is broken");
            SoundFXManagerScript.instance.PlaySoundFX(lockErrorSFX, gameObject.transform, 0.3f, 1f, 0f, 0f); //0.05
            //Debug.Log("LockScript: Elevator is isBroken");
        }
        else if (MainDoor.doorType == DoorType.ELEVATOR && floor != elevatorScript.currentFloor)
        {
            elevatorScript.destination = floor;
            elevatorScript.called = true;
            //Debug.Log("LockScript: Elevator was called");
            GameManagerScript.Instance.SetTextInfo("Elevator was called");
        }
        else
        {
            DoorMove();
            //Debug.Log("LockScript: open");
        }
    }

    /// <summary>
    /// Kontroluje, jestli hráè mùže dveøe otevøít, a volá metodu pro samotné otevírání dveøí
    /// </summary>
    void DoorMove()
    {
        if (MainDoor.isOpen && MainDoor.isActive)
        {
            foreach (DoorScript doorScript in doors)
            {
                //doorScript.gameObject.GetComponent<NavMeshObstacle>().enabled = true;
                coroutine = StartCoroutine(doorScript.DoSlidingClose());
                //doorScript.DoSlidingClose();  // Až budou animace

            }
        }
        else if (!MainDoor.isOpen && MainDoor.isActive)
        {
            foreach (DoorScript doorScript in doors)
            {
                //doorScript.gameObject.GetComponent<NavMeshObstacle>().enabled = false;
                coroutine = StartCoroutine(doorScript.DoSlidingOpen());
                //doorScript.DoSlidingOpen();   // Až budou animace
            }
        }
        //else //spam
    }

    void SetupForElevator()
    {
        if (MainDoor.doorType == DoorType.ELEVATOR)
        {
            elevatorScript = elevator.GetComponent<ElevatorSript>();
        }
    }
}
