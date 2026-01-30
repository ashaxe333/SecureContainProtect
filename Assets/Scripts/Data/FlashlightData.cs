using UnityEngine;

public class FlashlightData : ItemData
{
    public Light light;

    public override void Equip(bool equip)
    {
        isEquiped = equip;
    }
}
