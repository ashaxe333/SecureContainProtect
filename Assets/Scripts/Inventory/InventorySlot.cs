using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    // NÁPADY:
    // 1) Updatovat podle kurzoru pozici toho okna

    public bool used;

    public InventoryItem inventoryItem;
    public GameObject contextMenu;
    public ItemContextMenuScript contextMenuScript;

    public GameObject image;
    private Image spriteImage; //Bylo public
    public Sprite sprite;

    void Start()
    {
        spriteImage = image.GetComponent<Image>();
        image.SetActive(false);
        contextMenuScript = contextMenu.GetComponent<ItemContextMenuScript>();
        used = false;
    }

    /// <summary>
    /// Zobrzí sprite itemu, který se na danném slotu nachází
    /// </summary>
    public void ShowSprite()
    {
        spriteImage.sprite = sprite;
        used = true;
        image.SetActive(true);
    }

    /// <summary>
    /// Odstraní sprite itemu, který se na danném slotu nachází
    /// </summary>
    public void RemoveSprite()
    {
        spriteImage.sprite = null;
        inventoryItem = null;
        used = false;
        image.SetActive(false);
    }

    /// <summary>
    /// Reaguje na kliky hráèe
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerClick(PointerEventData eventData)
    {
        if (!used) return;

        if (eventData.button == PointerEventData.InputButton.Right) contextMenuScript.OnDrop();
        if (eventData.button == PointerEventData.InputButton.Left) contextMenuScript.OnEquip();
    }

    /// <summary>
    /// Reaguje na najetí myši na slot, a zobrazí okno s informacemi o objektu
    /// </summary>
    /// <param name="eventData"> asi informace o kurzoru? </param>
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!used) {
            Debug.Log("InventorySlot - used: " + used);
            return;
        }

        Debug.Log("InventorySlot - used: " + used);
        contextMenu.transform.position = new Vector3(eventData.position.x+40, eventData.position.y-30);
        contextMenuScript.Show(this);
    }

    /// <summary>
    /// Reaguje na najetí myši na slot, a shová okno s informacemi o objektu
    /// </summary>
    /// <param name="eventData"> asi informace o kurzoru? </param>
    public void OnPointerExit(PointerEventData eventData)
    {
        if (!used) return;

        contextMenuScript.Hide();
    }
}
