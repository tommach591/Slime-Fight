using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{

    public Slider slider;
    public Color low;
    public Color high;
    public Vector3 offset;
    public Text text;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        slider.transform.position = Camera.main.WorldToScreenPoint(transform.parent.position + offset);
        text.transform.position = Camera.main.WorldToScreenPoint(transform.parent.position + offset);
    }

    public virtual void setHealthBar(int health, float maxHealth) {
        slider.gameObject.SetActive(health < maxHealth);
        text.gameObject.SetActive(health < maxHealth);
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
