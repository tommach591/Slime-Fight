using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magic : Projectile
{

    protected bool facingLeft;
    protected bool faded;
    [SerializeField] protected SpriteRenderer[] srEfx;
    protected Color tmp;

    // Start is called before the first frame update
    void Start()
    {
        setUp();
    }

    // Update is called once per frame
    new void Update()
    {
        lookAtDirection();
        fire();
    }

	new protected void setUp() {
		StartCoroutine(fadeAway(lifeTime));
        sr = GetComponent<SpriteRenderer>();
		col = GetComponent<Collider2D>();
		rb2D = GetComponent<Rigidbody2D>();
        tmp = sr.color;
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

	public void lookAtDirection() {
        if (spawnPosition.x > transform.position.x && facingLeft) {
			flip();
		}
		else if (spawnPosition.x < transform.position.x && !facingLeft) {
			flip();
		}
	}

	public void flip() {
		facingLeft = !facingLeft;
		transform.localScale = new Vector3(transform.localScale.x, -transform.localScale.y, transform.localScale.z);
	}

    public IEnumerator fadeAway(float delay) {
        if (delay != 0) {
           yield return new WaitForSeconds(delay);
        }
        while (sr.color.a >= 0) {
            tmp.a = sr.color.a - 0.1f;
            sr.color = tmp;
            for (int i = 0; i < srEfx.Length; i++) {
                srEfx[i].color = tmp;
            }
            if (sr.color.a <= 0.30) {
                col.enabled = false;
            }
            yield return new WaitForSeconds(0.01f);
        }
        DestroyProjectile();
    }
}
