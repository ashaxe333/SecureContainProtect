using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerHealthScript : MonoBehaviour
{
    private GameObject player;
    private float hpValue;
    private PlayerController playerController;

    public bool isTakingDamage = false;
    public Slider hpSlider;
    [HideInInspector] public bool isDead = false;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerController = GetComponent<PlayerController>();
        hpValue = hpSlider.maxValue;
        hpSlider.value = hpSlider.maxValue;
        Debug.Log($"HP = {hpSlider.value}");
    }

    void Update()
    {
        Debug.Log($"HP = {hpSlider.value}");
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
        Debug.Log($"Value: {hpValue}");
    }

    public void Regenerate()
    {
        hpValue += Time.deltaTime * 0.05f;
    }
}
