using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class LightTriggerScript : MonoBehaviour
{
    public List<LightScript> lights = new List<LightScript>();
    [SerializeField] private AudioClip[] lightSwitchClicks;

    public void LightSwitch()
    {
        SoundFXManagerScript.instance.PlaySoundFX(lightSwitchClicks, gameObject.transform, 0.08f);
        //Debug.Log(Time.time);

        for (int i = 0; i < lights.Count; i++)
        {
            lights[i].ToggleLight();
        }
    }
}
