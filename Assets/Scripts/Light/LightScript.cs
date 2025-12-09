using UnityEngine;

public class LightScript : MonoBehaviour
{
    private GameObject player;
    private Light light;

    public bool broken;
    private bool on;
    public int floor;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        light = GetComponent<Light>();
        //light.GetComponent<Light>().intensity = 1;
    }

    // Update is called once per frame
    void Update()
    {
        DisableLight();
    }

    public void TurnOff()
    {
        light.enabled = false;
        on = false;
    }

    public void TurnOn()
    {
        light.enabled = true;
        on = true;
    }

    public void DisableLight()
    {
        // získat tøetí èást podmínky, pravdìpodobnì z výtahù a shodù. Zde budou triggery, které budou hlídat patra, a to budu ukládat do currentFloor v GameManageru
        if (Vector3.Distance(light.transform.position, player.transform.position) < 50.0f && on /* && floor == GameManager.currentFloor*/)
        {
            light.enabled = true;
        }
        else
        {
            light.enabled = false;
        }
    }
}
