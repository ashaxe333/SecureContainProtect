using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlinkScript : MonoBehaviour
{
    public Image fadeImage;
    private float blinkTimer = 15.0f;
    private float currentTime;

    private float blinkDuration;
    public bool isBlinking = false;

    public Slider blinkSlider;

    // Start is called before the first frame update
    void Start()
    {
        currentTime = blinkTimer;
        blinkSlider.maxValue = blinkTimer;
        blinkSlider.value = currentTime;
    }

    // Update is called once per frame
    void Update()
    {
        Blink();

        blinkSlider.value = currentTime;
    }

    /// <summary>
    /// Každých 15 vteøin, nebo po stisknutí klávesy space, se na chvilku zviditelní èerný Image - reprezentuje mrkání
    /// </summary>
    public void Blink()
    {
        if (!isBlinking)
        {
            currentTime -= Time.deltaTime;

            if (currentTime <= 0.0f || Input.GetKeyDown(KeyCode.Space))
            {
                isBlinking = true;
                blinkDuration = 0.2f;
            }
        }

        if (isBlinking)
        {
            if (blinkDuration > 0.0f)
            {
                fadeImage.color = new Color(0, 0, 0, 1);
                blinkDuration -= Time.deltaTime;
            }
            else
            {
                fadeImage.color = new Color(0, 0, 0, 0);
                isBlinking = false;
                currentTime = blinkTimer;
            }
        }
    }
}
