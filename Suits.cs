using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Suits : Magic
{
    protected bool ready = false;
    protected bool spawning = false;
    [SerializeField] Vector3 scaleChange;
    [SerializeField] Sprite[] suits;

    private int luck;
    private bool lifeSteal = false;
    private PlayerHealth ph;

    new void Update()
    {
        lookAtDirection();
        if (!spawning) {
            StartCoroutine(gettingReady());
        }
        if (ready) {
            fire();
        }
    }

	public IEnumerator gettingReady() {
        spawning = true;
        luck = Random.Range(0, suits.Length);
        sr.sprite = suits[luck];
        switch(luck) {
            case 0:
                critChance = 100f;
                break;
            case 1:
                lifeSteal = true;
                ph = FindObjectOfType<PlayerHealth>();
                speed /= 1.5f;
                lifeTime *= 2f;
                break;
            case 2:
                knockback = 5;
                speed /= 2f;
                lifeTime *= 3f;
                break;
            case 3:
                attack += 2;
                speed *= 2f;
                lifeTime /= 2f;
                break;
            default:
                attack += 2;
                speed *= 2f;
                lifeTime /= 2f;
                break;
        }
        for (int i = 0; i < 10; i++) {
            transform.localScale += scaleChange;
            yield return new WaitForSeconds(0.025f);
        }
        ready = true;
	}

	new protected void OnTriggerEnter2D(Collider2D other) {
		if (other.gameObject.tag == "Monster") {
            if (lifeSteal && !ph.dead) {
                ph.healBy(1);
            }
            Monster mon = other.gameObject.GetComponent<Monster>();
            Rigidbody2D monRb2D = other.gameObject.GetComponent<Rigidbody2D>();
			if (mon.isFacingLeft()) {
				monRb2D.AddForce(new Vector2(1f, 1f) * monRb2D.mass * knockbackLevel[knockback] * chargeMultiplier * 0.1f, ForceMode2D.Impulse);
			}
			else {
				monRb2D.AddForce(new Vector2(-1f, 1f) * monRb2D.mass * knockbackLevel[knockback] * chargeMultiplier * 0.1f, ForceMode2D.Impulse);
			}

			StartCoroutine(fadeAway(0));
		}
	}
}
