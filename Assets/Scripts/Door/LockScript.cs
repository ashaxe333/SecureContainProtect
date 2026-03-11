using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.AI;

public class LockScript : MonoBehaviour
{
    // UPRAVIT!!!
    // 1) ? mÌsto soundSourceTransform sem d·t parentWall, odkud zÌsk·m nÏjak (t¯eba p¯es skript) soundSourceTransform, PÿÕPADNÃ doorLeft a doorRight ?
    // 2) v budoucnu nedÏlat pohyb dve¯Ì p¯es coroutine, ale p¯es ANIMACI.

    // Base
    private GameObject player;
    public List<DoorScript> doors = new List<DoorScript>();
    //public GameObject door;
    private DoorScript MainDoor => doors[0];    //property: '=> doors[0]' je to samÈ, jako '{ get { return doors[0]; } }'
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
            Debug.LogError($"LockScript na {name} nem· p¯i¯azenÈ û·dnÈ dve¯e!");

            return;
        }
        SetupForElevator();
        player = GameObject.FindGameObjectWithTag("Player");
        inventoryScript = player.GetComponent<InventoryScript>();

        //door.GetComponent<NavMeshObstacle>().isEnabled = true;

        //audioSource = GetComponent<AudioSource>();
        //doorOpeningAudio = Resources.Load<AudioClip>("Sounds/");
    }

    /// <summary>
    /// Kontroluje, jestli jsem klikl na z·mek od dve¯Ì. Pokud jo, a pokud je moûnÈ dve¯e otev¯Ìt, zavol· metodu DoorMove()
    /// </summary>
    /// <param name="clickedObject">kliknut˝ z·mek</param>
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
            //Debug.Log("DoorScript: M· hr·Ë keycard? " + inventoryScript.IsKeyCardActive(MainDoor.lowestKeyCardLevel));
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

    /// <summary>
    /// Kontroluje, jestli je v˝tah v provozu, a p¯ÌpadnÏ otev¯e dve¯e
    /// </summary>
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
    /// Kontroluje, jestli hr·Ë m˘ûe dve¯e otev¯Ìt, a vol· metodu pro samotnÈ otevÌr·nÌ dve¯Ì
    /// </summary>
    void DoorMove()
    {
        if (MainDoor.isOpen && MainDoor.isActive)
        {
            foreach (DoorScript doorScript in doors)
            {
                //doorScript.gameObject.GetComponent<NavMeshObstacle>().isEnabled = true;
                coroutine = StartCoroutine(doorScript.DoSlidingClose());
                //doorScript.DoSlidingClose();  // Aû budou animace
                if (OpensGate()) TriggerEndScript.instance.SetEnabled(false);
            }
        }
        else if (!MainDoor.isOpen && MainDoor.isActive)
        {
            foreach (DoorScript doorScript in doors)
            {
                //doorScript.gameObject.GetComponent<NavMeshObstacle>().isEnabled = false;
                coroutine = StartCoroutine(doorScript.DoSlidingOpen());
                //doorScript.DoSlidingOpen();   // Aû budou animace
                if (OpensGate()) TriggerEndScript.instance.SetEnabled(true);
            }
        }
        //else //spam
    }

    /// <summary>
    /// NastavÌ elevatorScript, pokud jsou dve¯e typu ELEVATOR
    /// </summary>
    void SetupForElevator()
    {
        if (MainDoor.doorType == DoorType.ELEVATOR)
        {
            elevatorScript = elevator.GetComponent<ElevatorSript>();
        }
    }

    /// <summary>
    /// Zjiöùuje, jestli z·mÏk otevÌr· ˙nikovÈ dve¯e
    /// </summary>
    /// <returns> bool </returns>
    bool OpensGate()
    {
        if(gameObject.CompareTag("GateBOpen"))
            return true;
        return false;
    }
}
