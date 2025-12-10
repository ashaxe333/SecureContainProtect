using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerHealthScript : MonoBehaviour
{
    private GameObject player;
    private float hpValue;
    private PlayerController playerController;

    public Slider hpSlider;
    [SerializeField] public bool isDead = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerController = GetComponent<PlayerController>();
        hpValue = hpSlider.maxValue;
    }

    // Update is called once per frame
    void Update()
    {
        hpSlider.value = hpValue;
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

    public void regenerate()
    {
        hpValue += Time.deltaTime * 0.01f;
    }
}
