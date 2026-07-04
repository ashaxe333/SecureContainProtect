using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerDamageManager : MonoBehaviour
{
    // NÁPADY:
    // 1) Plyn se bude šíøit dál

    public static PlayerDamageManager instance;

    public PlayerHealthScript playerHealthScript;
    public InventoryScript inventoryScript;
    public List<GasTrigger> gasTriggerScripts = new List<GasTrigger>();

    [HideInInspector] public float gasDamage;      // každý frame
    [HideInInspector] public float scp173Damage;   // jednou
    [HideInInspector] public float scp939Damage;   // jednou
    [HideInInspector] public bool isTakingGas = false;
    [HideInInspector] public bool isTaking173 = false;
    [HideInInspector] public bool isTaking939 = false;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        gasTriggerScripts = FindObjectsByType<GasTrigger>().ToList();
        gasDamage = 0.03f;
        scp173Damage = 1000.0f;
        scp939Damage = 40.0f;
    }

    void Update()
    {
        if(isTakingGas && !inventoryScript.IsMaskActive())
        {
            playerHealthScript.TakeDamage(gasDamage);
            DeathInfoScript.msg = "You were killed by toxic gas";
        }

        if (isTaking173)
        {
            playerHealthScript.TakeDamage(scp173Damage);
            DeathInfoScript.msg = "You were killed by SCP-173";
            isTaking173 = false;
        }

        if (isTaking939)
        {
            playerHealthScript.TakeDamage(scp939Damage);
            DeathInfoScript.msg = "You were killed by SCP-939";
            isTaking939 = false;
        }
    }
}
