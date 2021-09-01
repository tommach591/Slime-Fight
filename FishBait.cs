using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishBait : Projectile
{
    [SerializeField] protected Projectile shark;
    protected Projectile copyShark;

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

	new protected void OnTriggerEnter2D(Collider2D other) {
		if (other.gameObject.tag == "Ground" || other.gameObject.tag == "Platform") {
            speed = 0;
			rb2D.gravityScale = 0;
			StartCoroutine(waitToSpawn());
		}
		if (other.gameObject.tag == "Monster") {
			StartCoroutine(spawnShark());
		}
	}

	protected IEnumerator waitToSpawn() {
		yield return new WaitForSeconds(7f);
		StartCoroutine(spawnShark());
	}

    protected IEnumerator spawnShark() {
		yield return new WaitForSeconds(0.1f);
        copyShark = Instantiate(shark, transform.position, Quaternion.identity);
		copyShark.setSpawnPosition(transform.position);
		copyShark.setSpeed(100f);
		copyShark.setDir(Vector3.up);
		copyShark.setAttack((int)(attack * 1.5f));
		copyShark.setCritChance(critChance);
		copyShark.setCritDamage(critDamage + 0.5f);
		copyShark.setKnockback(knockback);
        DestroyProjectile();
    }

}
