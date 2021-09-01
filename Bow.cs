using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bow : Weapon
{
	[SerializeField] protected Projectile arrow;
	private Projectile copy;

	protected Vector3 target;
	protected GameObject spawn;

	protected Animator anim;
	[SerializeField] protected string[] state;

    // Start is called before the first frame update
    void Awake()
    {
        setUp();
		spawn = transform.Find("Spawn").gameObject;
        anim = GetComponent<Animator>();
        anim.Play(state[0], 0, 0);
    }

    // Update is called once per frame
    void Update()
    {

    }

	public override void attack() 
	{
		if (!attacking && !ph.dead) 
		{
			if ((Input.GetKey(KeyCode.Z) || Input.GetKey(KeyCode.Mouse0)) && !chargeInProg) 
			{
				chargeInProg = true;
				if (!pah.isPlaying(1) && !pah.isPlaying(2)) 
				{
					pah.playAnim(8);
				}
				StartCoroutine(charging());
			}
			if (Input.GetKeyUp(KeyCode.Z) || Input.GetKeyUp(KeyCode.Mouse0)) 
			{
				StopCoroutine(charging());
				StartCoroutine(fire());
			}
		}
		if (ph.dead) {
			pc.stoppedOff();
			attacking = false;
			chargeInProg = false;
			chargeMultiplier = 1;
			damageMultiplier = 1;
			anim.Play(state[0], 0, 0);
		}
	}

	public override void resetConditions() {
		pc.stoppedOff();
		attacking = false;
		chargeInProg = false;
		chargeMultiplier = 1;
		damageMultiplier = 1;
		anim.Play(state[0], 0, 0);
	}

	public IEnumerator charging() 
	{
		yield return new WaitForSeconds(0.5f);
		if (chargeMultiplier == 1) 
		{
			chargeMultiplier = 2;
			damageMultiplier = 2;
			anim.Play(state[1], 0, 0);
			chargeInProg = false;
		}
		else if (chargeMultiplier == 2) 
		{
			chargeMultiplier = 3;
			damageMultiplier = 3;
			anim.Play(state[2], 0, 0);
		}
	}

	public IEnumerator fire() {
		attacking = true;
		chargeInProg = false;

		/*
		target = Camera.main.ScreenToWorldPoint(new Vector2(Input.mousePosition.x, Input.mousePosition.y)) - transform.parent.transform.position;
		if ((target.x < 0 && !pc.getFacingLeft()) || (0 < target.x && pc.getFacingLeft())) 
		{
			pc.flip();
		}
		*/

		pc.switchStopped();
		pah.playAnim(8);
		yield return new WaitForSeconds(0.01f);

		if (!pc.getFacingLeft()) {
			target = new Vector2(transform.localScale.x, 0);
		}
		else {
			target = new Vector2(-transform.localScale.x, 0);
		}

		pah.playAnim(9);
		yield return new WaitForSeconds(0.40f);

		copy = Instantiate(arrow, spawn.transform.position, Quaternion.identity).GetComponent<Projectile>();
		copy.setSpawnPosition(spawn.transform.position);
		copy.setSpeed(chargeMultiplier * 100f);
		copy.setMultiplier(chargeMultiplier);
		copy.setDir(target);
		copy.setAttack((int)((weaponAttack + (ps.getAttack() / 2)) * damageMultiplier));
		copy.setCritChance(ps.getCritChance() + critRateUp);
		copy.setCritDamage(ps.getCritDamage() + critDamageUp);
		copy.setKnockback(knockback);

		yield return new WaitForSeconds(0.05f);

		pc.switchStopped();
		pah.playAnim(0);
		yield return new WaitForSeconds(delayBtwAttacks);
		attacking = false;
		chargeMultiplier = 1;
		damageMultiplier = 1;
		anim.Play(state[0], 0, 0);
	}
}
