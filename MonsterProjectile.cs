using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterProjectile : Projectile
{

    public Monster owner;
    
	new protected void OnTriggerEnter2D(Collider2D other) {
		if (other.gameObject.tag == "Hunter") {
            PlayerController playerHit = other.gameObject.GetComponent<PlayerController>();
            Rigidbody2D playerRb2D = other.gameObject.GetComponent<Rigidbody2D>();
			if (playerHit.getFacingLeft()) {
				playerRb2D.AddForce(new Vector2(1f, 1f) * playerRb2D.mass, ForceMode2D.Impulse);
			}
			else {
				playerRb2D.AddForce(new Vector2(-1f, 1f) * playerRb2D.mass, ForceMode2D.Impulse);
			}

			Invoke("DestroyProjectile", 0.05f);
		}
		if (other.gameObject.tag == "Ground") {
			DestroyProjectile();
		}
	}
}
