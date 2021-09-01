using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : Magic
{

    protected bool ready = false;
    protected bool spawning = false;
    [SerializeField] Vector3 scaleChange;
    [SerializeField] Sprite[] fireballs;

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
        sr.sprite = fireballs[Random.Range(0, fireballs.Length)];
        for (int i = 0; i < 10; i++) {
            transform.localScale += scaleChange;
            yield return new WaitForSeconds(0.025f);
        }
        ready = true;
	}

	new protected void OnTriggerEnter2D(Collider2D other) {
		if (other.gameObject.tag == "Monster") {
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
