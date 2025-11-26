using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.UI;

public class GameManagerScript : MonoBehaviour
{
    // UPRAVIT
    // 1) vyøešit loading screen (v té scénì, kde se "zaseknu" dát panel s loadingem)
    public static GameManagerScript gameManagerInstance { get; private set; }

    private GameObject player;
    public int floor;   
    /*
    private GameObject f0;
    private GameObject f1;
    private GameObject f2;
    private GameObject f3;
    private GameObject stairs;
    */

    public TMP_Text info;
    private bool startFade = false;
    private float fadeTime = 5.0f;
    //private GameObject floor3;

    private Light[] lights;

    private void Awake()
    {
        // Singleton (bez toho je gameManagerInstance jen null)
        if (gameManagerInstance == null)
        {
            gameManagerInstance = this;
            //DontDestroyOnLoad(gameObject); // neznièí instanci pøi pøechodu do jiné scény
        }
        else
        {
            Destroy(gameObject); // znièí duplicitní instanci
        }
    }

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        /*
        f0 = GameObject.FindGameObjectWithTag("F0");
        f1 = GameObject.FindGameObjectWithTag("F1");
        f2 = GameObject.FindGameObjectWithTag("F2");
        f3 = GameObject.FindGameObjectWithTag("F3");
        stairs = GameObject.FindGameObjectWithTag("Stairs");
        */
        lights = FindObjectsByType<Light>(FindObjectsSortMode.None);
        //Debug.Log("GameManaerScript: pocet svetel: " + lights.Length);

        info.alpha = 0.0f;

        /*
        f0.SetActive(false);
        f2.SetActive(false);
        f3.SetActive(false);
        */
    }

    // Update is called once per frame
    void Update()
    {
        TurnLightOff();
        ShowTextInfo();
    }

    /*
    /// <summary>
    /// Vrac , kter  patro je ativn . Slou   pro triggery, kter  vypnou aktivn  patro, a zapnou to, o kter  se staraj  - optimaizace
    /// </summary>
    /// <returns></returns>
    public GameObject GetActiveFloor()
    {
        if (f0.activeInHierarchy) return f0;
        else if (f1.activeInHierarchy) return f1;
        else if (f2.activeInHierarchy) return f2;
        else if (f3.activeInHierarchy) return f3;
        else return null;
    }
    */

    /// <summary>
    /// Vypíná svìtla, která jsou daleko - optimalizace
    /// </summary>
    private void TurnLightOff()
    {
        for (int i = 0; i < lights.Length; i++)
        {
            if (Vector3.Distance(lights[i].transform.position, player.transform.position) < 40)
            {
                lights[i].enabled = true; 
            }
            else
            {
                lights[i].enabled = false;
            }
        }
    }

    /// <summary>
    /// Nastavuje bool startFade na true (spouští metodu ShowTextInfo), a nastaví text, který se ukáže hráèi ve høe
    /// </summary>
    /// <param name="text">text, který se napíše pøi høe, jako nìjaká informace pro hráèe</param>
    public void GetTextInfo(string text)
    {
        startFade = true;
        info.text = text;
    }

    /// <summary>
    /// Po urèitou dobu zobrazí hráèi nìjakou informaci, jako tøeba, že potøebuje lepší kartu pro otevøení dveøí
    /// </summary>
    private void ShowTextInfo()
    {
        if (startFade)
        {
            info.alpha = 1.0f;
            fadeTime -= Time.deltaTime;

            if(fadeTime < 0.0f)
            {
                startFade = false;
                info.alpha = 0.0f;
                fadeTime = 5.0f;
            }
        }
    }
}
