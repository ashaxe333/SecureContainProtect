using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "Item/Mask")]
public class MaskData : ItemData
{
    public GameObject maskVision;
    public override void Equip(bool equip)
    {
        isEquiped = equip;
        MaskEffectController.Instance.SetActive(equip);
    }
}
