using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider slider;

    public Gradient gradient;

    public Image fill;
    public Image dmgBar;
    float maxHealth;

    private float dmgBarTimer;
    private const float DMG_HEALTH_TIMER_MAX = 0.6f;

    public void SetMaxHealth(float health)
    {
        slider.maxValue = health;
        slider.value = health;

        dmgBar.fillAmount = health;

        maxHealth = health;    

        fill.color = gradient.Evaluate(1f);
    }

    public void SetHealth(float health)
    {
        slider.value = health;
        dmgBarTimer = DMG_HEALTH_TIMER_MAX;
        fill.color = gradient.Evaluate(slider.normalizedValue);
    }

    void Update()
    {
        dmgBarTimer -= Time.deltaTime;
        if (dmgBarTimer < 0)
        {
            if (slider.value < dmgBar.fillAmount * maxHealth)
            {
                float speed = 1f;
                dmgBar.fillAmount -= speed * Time.deltaTime;
            }
        }
    }
}
