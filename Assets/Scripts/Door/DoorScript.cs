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
    private float delay;

    private void Awake()
    {
        startPosition = transform.position;
    }

    void Start()
    {
        if (parentWall == null) Debug.Log($"DoorScript: dveøe {name} nemají parentwall!");
        //Debug.Log($"Animator: {animator.name}, Controller: {animator.runtimeAnimatorController}");
        //Debug.Log($"Trigger OpenDoor nastaven na: {animator.gameObject.name}");
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
        door_NMO.enabled = false;
        isActive = false;
        SoundFXManagerScript.instance.PlaySoundFX(doorMovementSFX, SoundFX_Source, 0.17f, 1.5f, 0f, 0f);

        yield return new WaitForSeconds(delay);
        //Debug.Log($"DoorScript: OPEN!");
        yield return null;
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
        door_NMO.enabled = true;
        isActive = false;
        SoundFXManagerScript.instance.PlaySoundFX(doorMovementSFX, SoundFX_Source, 0.17f, 1.5f, 0f, 0f); //0.05

        yield return new WaitForSeconds(delay);
        //Debug.Log($"DoorScript: CLOSE!");
        yield return null;
        animator.SetTrigger("CloseDoor");
        yield return new WaitForSeconds(1);

        isOpen = false;
        isActive = true;
    }
}