using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartBullet : Projectile
{

    private bool facingLeft;

    // Start is called before the first frame update
    void Start()
    {
        setUp();
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
}
