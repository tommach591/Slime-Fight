using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonMist : Magic
{
    new void Update()
    {

    }

	new protected void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.tag == "Ground" || other.gameObject.tag == "Platform") {
            rb2D.gravityScale = 0f;
            rb2D.velocity = Vector3.zero;
        }
	}
}
