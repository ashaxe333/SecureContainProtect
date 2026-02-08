using UnityEngine;

[CreateAssetMenu(menuName = "Item/FlashLight")]
public class FlashlightData : ItemData
{
    public Light light;

    public override void Equip(bool equip)
    {
        isEquiped = equip;
        FlashLightController.Instance.SetLightActive(equip);
    }
}
