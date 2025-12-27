using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler
{
    public bool used;
    public Sprite sprite;

    public InventoryItem inventoryItem;
    public GameObject contextMenu;
    private ItemContextMenuScript contextMenuScript;

    void Start()
    {
        contextMenuScript = contextMenu.GetComponent<ItemContextMenuScript>();
        used = false;
    }

    /// <summary>
    /// Zobrzí sprite itemu, který se na danném slotu nachází
    /// </summary>
    public void ShowSprite()
    {
        gameObject.GetComponent<Image>().sprite = sprite;
        used = true;
    }

    /// <summary>
    /// Odstraní sprite itemu, který se na danném slotu nachází
    /// </summary>
    public void RemoveSprite()
    {
        GetComponent<Image>().sprite = null;
        inventoryItem = null;
        used = false;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (inventoryItem == null) return;

        if (eventData.button == PointerEventData.InputButton.Right)
        {
            contextMenuScript.Show(this, transform.position);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("Hover?");
    }
}
