using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishGun : Gun
{
    [SerializeField] protected Projectile bait;

    protected float baitAttackCooldown = 0.25f;
    protected bool baitAttackOnCooldown = false;

	public override void attack() 
	{
		if (!ph.dead) {
			if ((Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.Mouse0))) {
				pc.dontFlip = true;
			}
			if ((Input.GetKeyDown(KeyCode.Z) || Input.GetKey(KeyCode.Mouse0)) && (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S)) && !attacking && !baitAttackOnCooldown) {
				StartCoroutine(fireBait());
			} 
			else if ((Input.GetKey(KeyCode.Z) || Input.GetKey(KeyCode.Mouse0)) && (!Input.GetKey(KeyCode.DownArrow) && !Input.GetKey(KeyCode.S)) && !attacking) 
			{
				StartCoroutine(fire());
			}
			if ((Input.GetKeyUp(KeyCode.Z) || Input.GetKeyUp(KeyCode.Mouse0))) {
				pc.dontFlip = false;
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

	public IEnumerator fireBait() {
		attacking = true;
		baitAttackOnCooldown = true;
		pc.slowFor(0.34f);
		if (!pc.getFacingLeft()) {
			target = new Vector2(1.2f, -1.2f) * rb2D.mass * force;
		}
		else {
			target = new Vector2(-1.2f, -1.2f) * rb2D.mass * force;
		}

		pah.playAnim(9);

		yield return new WaitForSeconds(0.34f);

        copy = Instantiate(bait, spawn.transform.position, Quaternion.identity).GetComponent<Projectile>();
		copy.setSpawnPosition(spawn.transform.position);
		copy.setSpeed(80f);
		copy.setDir(target);
		copy.setAttack(weaponAttack + (ps.getAttack() / 2));
		copy.setCritChance(ps.getCritChance() + critRateUp);
		copy.setCritDamage(ps.getCritDamage() + critDamageUp);
		copy.setKnockback(knockback);

		yield return new WaitForSeconds(delayBtwAttacks + 0.15f);
		attacking = false;

		yield return new WaitForSeconds(baitAttackCooldown);
		baitAttackOnCooldown = false;
	}
}
