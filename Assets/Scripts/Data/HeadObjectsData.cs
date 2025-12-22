using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UIElements.UxmlAttributeDescription;

[CreateAssetMenu(menuName = "Item/Mask")]
public class HeadObjectsData : ItemData
{

    public override void Use(bool use)
    {
        isUsing = use;
        Debug.Log($"using {itemName}: {isUsing}");
    }
}
