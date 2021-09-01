using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthBar : HealthBar
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
    }
    
    public override void setHealthBar(int health, float maxHealth) {
        slider.gameObject.SetActive(true);
        text.gameObject.SetActive(true);
        slider.value = health;
        slider.maxValue = maxHealth;
        if (health < 0) {
            text.text = "0";
        }
        else {
            text.text = health.ToString();
        }
        slider.fillRect.GetComponentInChildren<Image>().color = Color.Lerp(low, high, slider.normalizedValue);
    }
}
