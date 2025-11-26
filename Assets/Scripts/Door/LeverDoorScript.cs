using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class LeverDoorScript : MonoBehaviour
{
    public GameObject lever;
    public GameObject rightSide;
    public GameObject leftSide;
    private GameObject player;

    private Vector3 slideDirectionRight;
    private Vector3 slideDirectionLeft;
    private Vector3 startPositionRight;
    private Vector3 startPositionLeft;
    private float slideAmount = 4.0f;
    private float speed = 2.0f;

    private Coroutine coroutine;
    private bool isOpen = false;    // kontroluje, jestli jsou dveøe otevøené nebo zavøené
    private bool isActive = true;
    public float angle = 0;

    void Start()
    {
        if (lever.CompareTag("GateBOpen")) slideAmount = 6.0f;

        player = GameObject.FindGameObjectWithTag("Player");
        startPositionRight = rightSide.transform.position;
        startPositionLeft = leftSide.transform.position;

        if (rightSide == null || leftSide == null) throw new SystemException("dveøe!");

        switch (angle)
        {
            case 0:
                slideDirectionRight = Vector3.forward;
                slideDirectionLeft = Vector3.back;
                break;

            case 90:
                slideDirectionRight = Vector3.right;
                slideDirectionLeft = Vector3.left;
                break;
        }
    }

    /// <summary>
    /// Po interakci s pákou otevøe dveøe
    /// </summary>
    /// <param name="clickedObject"></param>
    public void DoorMove(GameObject clickedObject)
    {
        if (clickedObject == lever)
        {
            if (isOpen && isActive)
            {
                Debug.Log("zavøít");
                coroutine = StartCoroutine(DoSlidingClose());  //zavøít
            }
            else if (!isOpen && isActive)
            {
                Debug.Log("otevøít");
                coroutine = StartCoroutine(DoSlidingOpen());  //otevøít
            }
        }
    }

    /// <summary>
    /// Tuto metodu postupnì volá 'StartCoroutine(DoSlidingClose())' a frame po framu otevírá dveøe.
    /// 1) vypoèítá si, kam se mají èástí dveøí posunout (endPositionRight, endPositionLeft)
    /// 2) zapamatuje aktuální pozici dveøí jako startovní (newStartPositionRight, newStartPositionLeft)
    /// 3) Každý frame posouvá dveøe ze startovní pozice do cílové pomocí Vector3.Lerp()
    /// 4) Po každém framu zvyšuje promìnnou time pomocí Time.DeltaTime().
    /// Když time dosáhne 1, dveøe se zastaví, nastaví se jako otevøené a dá se zase kliknout na zámek
    /// </summary>
    /// <returns>pouze posouvá dveøe</returns>
    public IEnumerator DoSlidingOpen() //IEnumerator - metoda se spouští po èástech
    {
        isActive = false;
        Vector3 endPositionRight = startPositionRight + slideAmount * slideDirectionRight;
        Vector3 endPositionLeft = startPositionLeft + slideAmount * slideDirectionLeft;
        Vector3 newStartPositionRight = rightSide.transform.position;
        Vector3 newStartPositionLeft = leftSide.transform.position;
        float time = 0.0f;
        while (time < 1)
        {
            rightSide.transform.position = Vector3.Lerp(newStartPositionRight, endPositionRight, time);
            leftSide.transform.position = Vector3.Lerp(newStartPositionLeft, endPositionLeft, time);
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
        Vector3 endPositionRight = startPositionRight;
        Vector3 endPositionLeft = startPositionLeft;
        Vector3 newStartPositionRight = rightSide.transform.position;
        Vector3 newStartPositionLeft = leftSide.transform.position;
        float time = 0.0f;
        while (time < 1)
        {
            rightSide.transform.position = Vector3.Lerp(newStartPositionRight, endPositionRight, time);
            leftSide.transform.position = Vector3.Lerp(newStartPositionLeft, endPositionLeft, time);
            yield return null;
            time += Time.deltaTime * speed;
        }
        isOpen = false;
        isActive = true;
    }
}
