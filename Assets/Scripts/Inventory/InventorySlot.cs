using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
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

    void Update()
    {
        //updatovat position
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
        if (!used) return;

        if (eventData.button == PointerEventData.InputButton.Right) contextMenuScript.OnDrop();
        if (eventData.button == PointerEventData.InputButton.Left) contextMenuScript.OnUse();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!used) {
            Debug.Log("InventorySlot - used: " + used);
            return;
        }

        Debug.Log("InventorySlot - used: " + used);
        contextMenu.transform.position = new Vector3(eventData.position.x+50, eventData.position.y-50);
        contextMenuScript.Show(this);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!used) return;

        contextMenuScript.Hide();
    }
}
