using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : Weapon
{
	[SerializeField] protected Projectile bullet;
	protected Projectile copy;
	protected GameObject spawn;

	protected Vector3 target;
	protected int maxjumps = 1;
	protected int currentjumps = 0;
	protected float force = 10f;

    // Start is called before the first frame update
    void Start()
    {
        setUp();
		spawn = transform.Find("Spawn").gameObject;
    }

    // Update is called once per frame
    void Update()
    {

    }

	public override void attack() 
	{
		if (!ph.dead) {
			if ((Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.Mouse0))) {
				pc.dontFlip = true;
			}
			if ((Input.GetKey(KeyCode.Z) || Input.GetKey(KeyCode.Mouse0)) && !attacking) 
			{
				StartCoroutine(fire());
			}
			if ((Input.GetKeyUp(KeyCode.Z) || Input.GetKeyUp(KeyCode.Mouse0))) {
				StartCoroutine(resetAnim());
			}
			if (Input.GetKeyDown(KeyCode.Space) && !pc.isGrounded() && currentjumps != maxjumps) {
					pc.GetComponent<Rigidbody2D>().velocity = Vector2.up * 35f;
					currentjumps++;
			}
			if (pc.isGrounded()) {
				currentjumps = 0;
			}
		}
	}

	public override void resetConditions() {
		attacking = false;
		StartCoroutine(resetAnim());
	}

	public IEnumerator resetAnim() {
		while (attacking) {
			yield return new WaitForSeconds(0.01f);
		}
		if (!pc.isGrounded()) {
			pah.playAnim(2);
		}
		else {
			pah.playAnim(0);
		}
	}

	public IEnumerator fire() {
		/*
		target = Camera.main.ScreenToWorldPoint(new Vector2(Input.mousePosition.x, Input.mousePosition.y)) - transform.parent.transform.position;
		if ((target.x < 0 && !pc.getFacingLeft()) || (0 < target.x && pc.getFacingLeft())) 
		{
			pc.flip();
		}
		*/
		attacking = true;
		pc.slowFor(0.5f);
		if (!pc.getFacingLeft()) {
			target = new Vector2(1.2f, 0f) * rb2D.mass * force;
		}
		else {
			target = new Vector2(-1.2f, 0f) * rb2D.mass * force;
		}

		if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.LeftArrow)) {
			rb2D.AddForce(target, ForceMode2D.Impulse);
		} 
		pah.playAnim(9);

		yield return new WaitForSeconds(0.34f);

		for (int i = 0; i < 3; i++) {
			copy = Instantiate(bullet, spawn.transform.position, Quaternion.identity).GetComponent<Projectile>();
			copy.setSpawnPosition(spawn.transform.position);
			copy.setSpeed(80f);
			copy.setDir(target);
			copy.setAttack(weaponAttack + (ps.getAttack() / 2));
			copy.setCritChance(ps.getCritChance() + critRateUp);
			copy.setCritDamage(ps.getCritDamage() + critDamageUp);
			copy.setKnockback(knockback);
			yield return new WaitForSeconds(0.05f);
		}

		yield return new WaitForSeconds(delayBtwAttacks);
		attacking = false;
	}
}
