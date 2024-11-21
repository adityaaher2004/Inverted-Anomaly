using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    // Start is called before the first frame update

    public Slider slider;
    public Slider easeSlider;
    public float maxHealth = 100f;
    public float health;
    void Start()
    {
        health = maxHealth;
        slider.value = health;
        easeSlider.value = health;
    }

    // Update is called once per frame
    void Update()
    {
        if (slider.value != health)
        {
            slider.value = health;
        }

        if (slider.value != easeSlider.value)
        {
            easeSlider.value -= 0.125f;
        }
    }

    public void TakeDamage(float amt)
    {
        health -= amt;
    }
}
