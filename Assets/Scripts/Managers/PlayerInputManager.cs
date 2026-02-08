using UnityEngine;

public class PlayerInputManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        CheckInput();
    }

    public void CheckInput()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            FlashLightController.Instance.SetLightActive(!FlashLightController.Instance.isActive);
        }
    }
}
