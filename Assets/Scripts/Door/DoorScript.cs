using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class DoorScript : MonoBehaviour
{
    [HideInInspector] public bool isOpen = false;       // kontroluje, jestli jsou dveøe otevøené nebo zavøené
    [HideInInspector] public bool isActive = true;      // chrání pøed spamklikem
    public bool isBroken = false;                       // pokud jsou dveøe zamèené, nejdou otevøít
    public int lowestKeyCardLevel;                      // nejnižší potøebný level karty
    [SerializeField] private Animator animator;

    public DoorType doorType;
    [HideInInspector] public Vector3 startPosition;
    public GameObject parentWall;
    [HideInInspector] public Vector3 slideDirection;

    [SerializeField] private NavMeshObstacle door_NMO;
    [SerializeField] private Transform SoundFX_Source;

    public AudioClip doorMovementSFX;

    private float slideAmount;                          // poèítá se podle typu dveøí
    private float speed;                                // poèítá se podle typu dveøí
    private float delay;

    private void Awake()
    {
        startPosition = transform.position;
        DoorRotation();
        SetDoorMoveParameters();
    }

    void Start()
    {
        if (parentWall == null) Debug.Log($"DoorScript: dveøe {name} nemají parentwall!");
        //animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            animator.SetTrigger("OpenDoor");
            Debug.Log("Trigger manuálnì nastaven");
        }
    }

    /// <summary>
    /// Podle doorType nastaví smìry posunu dveøí
    /// </summary>
    void DoorRotation()
    {
        switch (doorType)
        {
            case DoorType.SINGLE or DoorType.ELEVATOR or DoorType.RIGHT:
                SetDoorDirection(Vector3.back, Vector3.right);
                break;

            case DoorType.LEFT:
                SetDoorDirection(Vector3.forward, Vector3.left);
                break;

            case DoorType.GATE:
                SetDoorDirection(Vector3.up, Vector3.zero);
                break;
        }
    }

    /// <summary>
    /// Nastaví smìry posunu dveøí
    /// </summary>
    /// <param name="direction1"> smìr otevírání </param>
    /// <param name="direction2"> smìr zavírání </param>
    public void SetDoorDirection(Vector3 direction1, Vector3 direction2)
    {
        if (direction2 != Vector3.zero)
            switch (Mathf.Round(parentWall.transform.eulerAngles.y))
            {
                case 0 or 180:
                    slideDirection = direction1;
                    break;

                case 90 or 270:
                    slideDirection = direction2;
                    break;

                default:
                    Debug.Log($"DoorScript: Unexpected wall rotation on {parentWall.name}");
                    break;

            }
        else
            slideDirection = direction1;
    }

    /// <summary>
    /// podle typu deøí vypoèítá, o kolik se dveøe posunou a jak dlouho se budou posouvat
    /// </summary>
    void SetDoorMoveParameters()
    {
        switch (doorType)
        {
            case DoorType.SINGLE or DoorType.ELEVATOR:
                slideAmount = 4.0f;
                speed = 2.0f;
                delay = 0.2f;
                break;

            case DoorType.LEFT or DoorType.RIGHT:
                slideAmount = 3.0f;
                speed = 2.0f;
                delay = 0.2f;
                break;

            case DoorType.GATE:
                slideAmount = 10.0f;
                speed = 1.0f;
                delay = 0.4f;
                break;
        }
    }

    /// <summary>
    /// Tuto metodu postupnì volá 'StartCoroutine(DoSlidingClose())' a frame po framu otevírá dveøe.
    /// 1) vypoèítá si, kam se mají dveøe posunout (endPosition)
    /// 2) zapamatuje aktuální pozici dveøí jako startovní (newStartPosition)
    /// 3) Každý frame posouvá dveøe ze startovní pozice do cílové pomocí Vector3.Lerp()
    /// 4) Po každém framu zvyšuje promìnnou time pomocí Time.DeltaTime().
    /// Když time dosáhne 1, dveøe se zastaví, nastaví se jako otevøené a dá se zase kliknout na zámek
    /// </summary>
    /// <returns>pouze posouvá dveøe</returns>
    public IEnumerator DoSlidingOpen() //IEnumerator - metoda se spouští po èástech
    {
        /*
        door_NMO.enabled = false;
        isActive = false;
        Vector3 endPosition = startPosition + slideAmount * slideDirection;
        Vector3 newStartPosition = transform.position;
        float time = 0.0f;
        SoundFXManagerScript.instance.PlaySoundFX(doorMovementSFX, SoundFX_Source, 0.17f, 1.5f, 0f, 0f);

        yield return new WaitForSeconds(delay);
        //Debug.Log($"DoorScript: slideDirection = {slideDirection}, start = {startPosition}, end = {endPosition}");

        while (time < 1)
        {
            transform.position = Vector3.Lerp(newStartPosition, endPosition, time);
            yield return null;
            time += Time.deltaTime * speed;
        }

        isOpen = true;
        isActive = true;
        */

        door_NMO.enabled = false;
        isActive = false;
        SoundFXManagerScript.instance.PlaySoundFX(doorMovementSFX, SoundFX_Source, 0.17f, 1.5f, 0f, 0f);

        yield return new WaitForSeconds(delay);
        Debug.Log($"DoorScript: OPEN!");
        yield return null;
        Debug.Log($"Animator: {animator.name}, Controller: {animator.runtimeAnimatorController}");
        Debug.Log($"Trigger OpenDoor nastaven na: {animator.gameObject.name}");
        animator.SetTrigger("OpenDoor");
        yield return new WaitForSeconds(1);

        isOpen = true;
        isActive = true;
    }

    /// <summary>
    /// To stejné, jako u otevírání, akorát se dveøe zavírají
    /// </summary>
    /// <returns>pouze posouvá dveøe</returns>
    public IEnumerator DoSlidingClose()
    {
        /*
        door_NMO.enabled = true;
        isActive = false;
        Vector3 endPosition = startPosition;
        Vector3 newStartPosition = transform.position;
        float time = 0.0f;
        SoundFXManagerScript.instance.PlaySoundFX(doorMovementSFX, SoundFX_Source, 0.17f, 1.5f, 0f, 0f); //0.05

        yield return new WaitForSeconds(delay);

        while (time < 1)
        {
            transform.position = Vector3.Lerp(newStartPosition, endPosition, time);
            yield return null;
            time += Time.deltaTime * speed;
        }

        isOpen = false;
        isActive = true;
        */

        door_NMO.enabled = true;
        isActive = false;
        SoundFXManagerScript.instance.PlaySoundFX(doorMovementSFX, SoundFX_Source, 0.17f, 1.5f, 0f, 0f); //0.05

        yield return new WaitForSeconds(delay);
        Debug.Log($"DoorScript: CLOSE!");
        yield return null;
        animator.SetTrigger("CloseDoor");
        yield return new WaitForSeconds(1);

        isOpen = false;
        isActive = true;
    }
}