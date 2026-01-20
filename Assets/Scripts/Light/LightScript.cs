using UnityEngine;
using UnityEngine.WSA;

public class LightScript : MonoBehaviour
{
    public Light lightInScene;

    public bool broken;
    private bool on = true;
    public int floor;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
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
        lightInScene.enabled = on && !broken;
    }

    public void ToggleLight()
    {
        on = !on;
        ApplyState();
    }
}
