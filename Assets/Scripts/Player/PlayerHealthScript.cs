using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerHealthScript : MonoBehaviour
{
    private float hpValue;
    public bool isTakingDamage = false;
    public Slider hpSlider;
    [HideInInspector] public bool isDead = false;

    void Start()
    {
        hpValue = hpSlider.maxValue;
        hpSlider.value = hpSlider.maxValue;
    }

    void Update()
    {
        hpSlider.value = hpValue;
        if(hpSlider.value < hpSlider.maxValue)
        {
            Regenerate();
        }
        if (hpSlider.value <= 0)
        {
            isDead = true;
        }
        if (isDead) SceneManager.LoadScene(2);
    }

    public void TakeDamage(float dmg)
    {
        hpValue -= dmg;
    }

    public void Regenerate()
    {
        hpValue += Time.deltaTime * 0.05f;
    }
}
