using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class LightTriggerScript : MonoBehaviour
{
    public List<LightScript> lights = new List<LightScript>();

    public void LightSwitch()
    {
        for (int i = 0; i < lights.Count; i++)
        {
            lights[i].ToggleLight();
        }
    }
}
