using UnityEngine;

public class InventorySlot : MonoBehaviour
{
    public bool used = false;
    public Sprite sprite;
    public InventoryItem InventoryItem;

    public void ShowSprite()
    {
        //nìjak zobrazit sprite
    }

    public void RemoveSprite()
    {
        //nìjak skrýt sprite
        sprite = null;
    }

}
