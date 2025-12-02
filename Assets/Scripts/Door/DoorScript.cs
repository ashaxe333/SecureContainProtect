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

    public DoorType doorType;
    [HideInInspector] public Vector3 startPosition;
    public GameObject parentWall;
    [HideInInspector] public Vector3 slideDirection;

    private float slideAmount;                          // poèítá se podle typu dveøí
    private float speed;                                // poèítá se podle typu dveøí

    private NavMeshObstacle nmo;    // pøidat velikost

    private void Awake()
    {
        startPosition = transform.position;
        nmo = GetComponent<NavMeshObstacle>();
        DoorRotation();
        SetDoorMoveParameters();
    }

    void Start()
    {
        if(parentWall == null) Debug.Log($"DoorScript: dveøe {name} nemají parentwall!");
    }

    /// <summary>
    /// Podle hodnoty se nastaví, jakým smìrem se dveøe budou otevírat
    /// </summary>
    void DoorRotation()
    {
        switch (parentWall.transform.eulerAngles.y)
        {
            case 0 or 180:
                slideDirection = Vector3.back;
                break;

            case 90 or 270:
                slideDirection = Vector3.right;
                break;
        }
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
                break;

            case DoorType.LEFT or DoorType.RIGHT:
                slideAmount = 3.0f;
                speed = 2.0f;
                break;

            case DoorType.GATE:
                slideAmount = 7.0f;
                speed = 4.0f;
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
        //audioSource.PlayOneShot(doorOpeningAudio);
        isActive = false;
        Vector3 endPosition = startPosition + slideAmount * slideDirection;
        Vector3 newStartPosition = transform.position;
        float time = 0.0f;
        //Debug.Log($"DoorScript: slideDirection = {slideDirection}, start = {startPosition}, end = {endPosition}");

        while (time < 1)
        {
            transform.position = Vector3.Lerp(newStartPosition, endPosition, time);
            yield return null;
            time += Time.deltaTime * speed;
        }

        isOpen = true;
        isActive = true;
    }

    /// <summary>
    /// To stejné, jako u otevírání, akorát se dveøe zavírají
    /// </summary>
    /// <returns>pouze posouvá dveøe</returns>
    public IEnumerator DoSlidingClose()
    {
        isActive = false;
        Vector3 endPosition = startPosition;
        Vector3 newStartPosition = transform.position;
        float time = 0.0f;

        while (time < 1)
        {
            transform.position = Vector3.Lerp(newStartPosition, endPosition, time);
            yield return null;
            time += Time.deltaTime * speed;
        }

        isOpen = false;
        isActive = true;
    }
}

/// <summary>
/// tøída, která definuje typ dveøí, na základì kterého se poèítá jejích pohyb a rychlost
/// </summary>
public enum DoorType
{
    SINGLE, LEFT, RIGHT, ELEVATOR, GATE
}
