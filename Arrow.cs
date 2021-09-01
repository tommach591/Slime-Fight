using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : Projectile
{
	new protected void OnTriggerEnter2D(Collider2D other) {
		if (other.gameObject.tag == "Ground") {
			Invoke("DestroyProjectile", 0.05f);
		}
	}
}
