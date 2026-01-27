using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class LightScript : MonoBehaviour
{
    public Light lightInScene;

    public bool isBroken;
    private bool isOn = true;
    private bool isInRange;
    public int floor;   // užiteèné v pøípadì, že nemám optimalizaci svìta, jen svìtel

    void Awake()
    {
        lightInScene = gameObject.GetComponent<Light>();
        //ApplyState();
        //light.GetComponent<Light>().intensity = 1;
    }

    private void OnEnable()
    {
        ApplyState();
    }

    public void ApplyState()
    {
        lightInScene.enabled = isOn && !isBroken && isInRange;
    }

    public void ToggleLight()
    {
        isOn = !isOn;
        ApplyState();
    }

    public void SetIsInRange(bool value)
    {
        if (isInRange == value) return;    
        isInRange = value;
        ApplyState();
    }
}
