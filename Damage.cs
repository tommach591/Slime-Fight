using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Damage : MonoBehaviour
{
    private float lifeTime = 1.5f;
    private float speed = 4f;

    [SerializeField] private Text damageNumber;

    [SerializeField] private Color normal;
    [SerializeField] private Color crit;

    private Vector3 offset;

    private string[] cluck = {
        "Dash while moving Left/Right makes you avoid getting hit!",
        "Press Down with the Sword to Guard!",
        "If you Guard with the Sword at the last moment, you will trigger Perfect Guard!",
        "Some monster projectiles can be hit back with the Sword or Hammer!",
        "Hold Attack to charge with Hammer or Bow!",
        "Hold Up and tap Jump while charging the Hammer to double jump!",
        "Slam your Hammer from the air to do the Giga Slam!",
        "The Bow has built in Crit Rate Up!",
        "Just using the Gun lets you double jump!",
        "Avoid getting hit with the Claw with Up and Down Attack!",
        "The summoned magic from the Tome is homing!",
        "Some weapons will have special effects on them!"
    };
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        damageNumber.transform.position = Camera.main.WorldToScreenPoint(new Vector3(transform.parent.position.x, transform.parent.position.y - speed, transform.parent.position.z) + offset);
        if (speed >= 0f) {
            speed -= speed / 100;
        }
    }

    public void setUp(int x, Vector3 o, bool y) {
        if (x == 0) {
            damageNumber.text = "IMMUNE";
        }
        else if (x == 1337) {
            damageNumber.text = cluck[Random.Range(0, cluck.Length)];
            lifeTime = 3f;
        }
        else {
            damageNumber.text = x.ToString();
        }
        offset = o;
        if (y) {
            damageNumber.color = crit;
        }
        else {
            damageNumber.color = normal;
        }
        Invoke("DestroyMe", lifeTime);
    }

    public void DestroyMe()
    {
        Destroy(gameObject);
    }
}
