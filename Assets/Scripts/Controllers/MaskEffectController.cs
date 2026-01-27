using UnityEngine;
using UnityEngine.UI;

public class MaskEffectController : MonoBehaviour
{
    public static MaskEffectController Instance;
    public GameObject maskVision;
    
    void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        SetActive(false);
    }

    /// <summary>
    /// Vypíná efekt masky
    /// </summary>
    /// <param name="active"> bool </param>
    public void SetActive(bool active)
    {
        maskVision.SetActive(active);
    }
}
