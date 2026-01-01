using UnityEngine;

[CreateAssetMenu(menuName = "Item/Mask")]
public class MaskData : ItemData
{
    public override void Equip(bool equip)
    {
        isEquiped = equip;
    }
}
