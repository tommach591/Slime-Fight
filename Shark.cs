using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shark : Projectile
{
    private bool facingLeft;
    private Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        setUp();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    new void Update()
    {
        fire();
        lookAtDirection();
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
		}
	}

	new protected void setUp() {
        Invoke("DestroyProjectile", lifeTime);
		Invoke("reverseDir", lifeTime / 2);
		col = GetComponent<Collider2D>();
		rb2D = GetComponent<Rigidbody2D>();
		sr = GetComponent<SpriteRenderer>();
	}

    protected void reverseDir() {
        setDir(-getDir());
        anim.Play("shark_down", 0, 0);
    }

	new protected void fire() {
		dir.Normalize();
		rb2D.velocity = dir * speed;
		angle = Mathf.Atan2(rb2D.velocity.y, rb2D.velocity.x) * Mathf.Rad2Deg + -90f;
		transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
	}
}
