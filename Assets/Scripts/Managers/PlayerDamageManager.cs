using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerDamageManager : MonoBehaviour
{
    // NÁPADY:
    // 1) Plyn se bude šíøit dál

    public static PlayerDamageManager instance;

    public PlayerHealthScript playerHealthScript;
    public List<GasTrigger> gasTriggerScripts = new List<GasTrigger>();

    public float gasDamage = 0.01f;     // každý frame
    public float scp173Damage = 100f;   // jednou
    public float scp939Damage = 100f;   // jednou
    public bool isTakingGas = false;
    public bool isTaking173 = false;
    public bool isTaking939 = false;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        gasTriggerScripts = FindObjectsByType<GasTrigger>(FindObjectsSortMode.None).ToList();
    }

    void Update()
    {
        if(isTakingGas)
            playerHealthScript.TakeDamage(gasDamage);

        if (isTaking173)
        {
            playerHealthScript.TakeDamage(scp173Damage);
            isTaking173 = false;
        }

        if (isTaking939)
        {
            playerHealthScript.TakeDamage(scp939Damage);
            isTaking939 = false;
        }
    }
}
