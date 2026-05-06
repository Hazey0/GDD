using UnityEngine;
using UnityEngine.UI;

public class HealthBarColor : MonoBehaviour
{
    public Slider healthSlider;
    public Image fillImage;

    void Update()
    {
        float percent = healthSlider.value / healthSlider.maxValue * 100f;

        if (percent >= 80f)
            fillImage.color = new Color32(0, 255, 68, 255);   // green 0-80
        else
            fillImage.color = new Color32(255, 0, 0, 255);    // red 80-100
    }
}