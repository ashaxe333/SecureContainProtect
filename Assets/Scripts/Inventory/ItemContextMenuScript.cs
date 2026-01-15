using System.Runtime.InteropServices.WindowsRuntime;
using TMPro;
using UnityEngine;

public class ItemContextMenuScript : MonoBehaviour
{
    private InventorySlot currentSlotScript;
    private InventoryScript inventoryScript;
    private GameObject player;

    public TMP_Text itemName;
    public TMP_Text itemDescription;

    private CanvasGroup canvasGroup;

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        inventoryScript = player.GetComponent<InventoryScript>();

        canvasGroup = GetComponent<CanvasGroup>();
        canvasGroup.blocksRaycasts = false;
    }

    void Start()
    {
        Hide();
    }

    /// <summary>
    /// Schová okno
    /// </summary>
    public void Hide()
    {
        currentSlotScript = null;
        gameObject.SetActive(false);
    }

    /// <summary>
    /// Zobrazí toto okno nad danným slotem
    /// </summary>
    /// <param name="inventorySlot"> slot nad kterým se okno ukáže </param>
    public void Show(InventorySlot inventorySlot)
    {
        currentSlotScript = inventorySlot;
        itemName.text = inventorySlot.inventoryItem.itemData.name;
        itemDescription.text = inventorySlot.inventoryItem.itemData.description;
        gameObject.SetActive(true);
    }

    /// <summary>
    /// Volá metodu Drop inventáøe, a schová toto okno
    /// </summary>
    public void OnDrop()
    {
        if (currentSlotScript == null) return;

        //Debug.Log("ItemContextMenuScript - OnDrop: currentSlot není null!!!!");
        inventoryScript.Drop(currentSlotScript.inventoryItem, player.transform.position + player.transform.forward);
        Hide();
    }

    /// <summary>
    /// Volá metodu Equip inventáøe, a schová toto okno
    /// </summary>
    public void OnEquip()
    {
        if (currentSlotScript == null) return;

        //Debug.Log("ItemContextMenuScript - OnEquip: currentSlot není null!!!!");
        inventoryScript.Equip(currentSlotScript.inventoryItem);
        Hide();
    }
}
