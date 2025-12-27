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

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        inventoryScript = player.GetComponent<InventoryScript>();

        canvasGroup = GetComponent<CanvasGroup>();
        canvasGroup.blocksRaycasts = false;
        Hide();
    }

    public void Hide()
    {
        currentSlotScript = null;
        gameObject.SetActive(false);
    }

    public void Show(InventorySlot inventorySlot)
    {
        currentSlotScript = inventorySlot;
        itemName.text = inventorySlot.inventoryItem.itemData.name;
        itemDescription.text = inventorySlot.inventoryItem.itemData.description;
        gameObject.SetActive(true);

        Vector2 position;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            transform.parent as RectTransform,
            Input.mousePosition,
            null,
            out position
        );
        transform.localPosition = position;
    }

    public void OnDrop()
    {
        if (currentSlotScript == null) return;

        Debug.Log("ItemContextMenuScript - OnDrop: currentSlot není null!!!!");
        inventoryScript.Drop(currentSlotScript.inventoryItem, player.transform.position + player.transform.forward);
        Hide();
    }

    public void OnUse()
    {
        if (currentSlotScript == null) return;

        Debug.Log("ItemContextMenuScript - OnUse: currentSlot není null!!!!");
        inventoryScript.Use(currentSlotScript.inventoryItem.itemData);
        Hide();
    }
}
