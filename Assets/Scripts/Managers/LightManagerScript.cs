using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class LightManagerScript : MonoBehaviour
{
    public static LightManagerScript Instance { get; private set; }

    [SerializeField] private LightScript[] lights;
    private GameObject player;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        lights = FindObjectsByType<LightScript>(FindObjectsSortMode.None);
        Debug.Log("LightManagerScript: pocet svetel: " + lights.Length);
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        TurnLightOff();
    }

    /// <summary>
    /// Vypíná svìtla, která jsou daleko a na jiném patøe, než na kterém se nachází hráè
    /// </summary>
    private void TurnLightOff()
    {
        for (int i = 0; i < lights.Length; i++)
        {
            bool outOfRange = Vector3.Distance(lights[i].gameObject.transform.position, player.transform.position) > 50;
            bool wrongFloor = lights[i].floor != GameManagerScript.Instance.currentFloor && lights[i].floor != -1;

            lights[i].SetIsInRange(!(outOfRange || wrongFloor));
        }
    }
}
