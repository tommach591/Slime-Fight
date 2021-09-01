using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JollyAxe : Hammer
{
	private Vector3 rotatation = new Vector3(0, 1, 0);
	private bool spinning = false;

	private int spinAttackVal = 1;
	private int normalAttackVal = 5;

    // Start is called before the first frame update
    void Awake()
    {
        setUp();
        anim = GetComponent<Animator>();
        anim.Play(state[0], 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
		if (ph.hurt) {
			col.enabled = false;
			transform.localScale = new Vector3(1f, 1f, 1f);
			anim.Play(state[0], 0, 0);
			attacking = false;
			chargeMultiplier = 1;
			damageMultiplier = 1;
		}
    }

	public override void attack() {
		if (!spinning && !doingTheGigaSlam && !ph.dead) {
			pc.transform.rotation = Quaternion.identity;
		}
		if (!attacking && !doingTheGigaSlam && !ph.dead) {
			if ((Input.GetKey(KeyCode.Z) || Input.GetKey(KeyCode.Mouse0)) && !chargeInProg) {
				StartCoroutine(charging());
			}
			if (chargeInProg || doingTheGigaSlam) {
				pc.slowFor(0.01f);
			}
			if ((Input.GetKey(KeyCode.Z) || Input.GetKey(KeyCode.Mouse0)) && !spinning) {
				StartCoroutine(spinAttack());
			}
			if ((Input.GetKeyUp(KeyCode.Z) || Input.GetKeyUp(KeyCode.Mouse0))) {;
				pc.slowFor(1.5f);
				pc.transform.rotation = Quaternion.identity;
				StartCoroutine(swingDown());
			}
		}
	}

	public override void resetConditions() {
		transform.localScale = new Vector3(1f, 1f, 1f);
		pc.stoppedOff();
		attacking = false;
		chargeInProg = false;
		chargeMultiplier = 1;
		damageMultiplier = 1;
		ps.guardDown();
		doingTheGigaSlam = false;
	}

	override public IEnumerator charging() {
		chargeInProg = true;
		yield return new WaitForSeconds(0.78f);
		if (chargeMultiplier == 1 && chargeInProg) {
			chargeMultiplier = 2;
			damageMultiplier = 1.5f;
			transform.localScale = new Vector3(1.25f, 1.25f, 1.25f);
            anim.Play(state[1], 0, 0);
			chargeInProg = false;
		}
		else if (chargeMultiplier == 2 && chargeInProg) {
			chargeMultiplier = 3;
			damageMultiplier = 2f;
			transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
            anim.Play(state[2], 0, 0);
			chargeInProg = false;
		}
		else if (chargeMultiplier == 3 && chargeInProg) {
			chargeMultiplier = 4;
			damageMultiplier = 3f;
			transform.localScale = new Vector3(2.0f, 2.0f, 2.0f);
            anim.Play(state[3], 0, 0);
		}
	}

	public IEnumerator spinAttack() {
		col.enabled = true;
		spinning = true;
		weaponAttack = spinAttackVal;
		for (int i = 0; i < 18; i++) {
			pah.playAnim(7);
			pc.transform.Rotate (rotatation * 20);
			yield return new WaitForSeconds(0.005f);
		}
		pc.transform.rotation = Quaternion.identity;
		spinning = false;
	}

	override public IEnumerator swingDown() {
		attacking = true;
		chargeInProg = false;
		pc.stoppedOn();
		while (spinning) {
			yield return new WaitForSeconds(0.03f);
		}
		weaponAttack = normalAttackVal;
		pah.playAnim(6);
		rb2D.velocity = Vector2.up * 50f;
		yield return new WaitForSeconds(0.3f);
		col.enabled = true;
		rb2D.velocity = Vector2.down * downwardForce;
		while (!pc.isGrounded()) {
			pah.playAnim(7);
			yield return new WaitForSeconds(0.1f);
			damageMultiplier += (int)Mathf.Abs(rb2D.velocity.y / 50);
		}
		yield return new WaitForSeconds(0.05f);
		col.enabled = false;
		yield return new WaitForSeconds(0.25f);
		if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow)) {
			pah.playAnim(1);
		}
		else {
			pah.playAnim(0);
		}
		transform.localScale = new Vector3(1f, 1f, 1f);
		pc.stoppedOff();
        anim.Play(state[0], 0, 0);
		yield return new WaitForSeconds(delayBtwAttacks);
		attacking = false;
		chargeMultiplier = 1;
		damageMultiplier = 1;
	}

	new protected IEnumerator gigaSlam() {
		doingTheGigaSlam = true;
		pc.transform.rotation = Quaternion.identity;

		pc.stoppedOn();
		rb2D.velocity = Vector3.up * downwardForce;
		yield return new WaitForSeconds(0.25f);

		for (int i = 0; i < 36; i++) {
			pc.transform.Rotate (Vector3.forward * 10);
			yield return new WaitForSeconds(0.001f);
		}
		pc.transform.rotation = Quaternion.identity;
	
		ps.guardUp();
		rb2D.velocity = Vector3.down * 4f * downwardForce;

		while (!pc.isGrounded()) {
			pah.playAnim(7);
			yield return new WaitForSeconds(0.1f);
		}

		ps.guardDown();
		pc.stoppedOff();
		doingTheGigaSlam = false;
	}

	new protected void OnTriggerEnter2D(Collider2D other) {
		if (other.gameObject.tag == "Monster") {
			Rigidbody2D mon = other.gameObject.GetComponent<Rigidbody2D>();
			if (other.gameObject.GetComponent<Monster>().isFacingLeft() && pc.isGrounded()) {
				mon.AddForce(new Vector2(1f, 1f) * mon.mass * (knockbackLevel[knockback] * damageMultiplier * 0.1f), ForceMode2D.Impulse);
			}
			else if (pc.isGrounded()) {
				mon.AddForce(new Vector2(-1f, 1f) * mon.mass * (knockbackLevel[knockback] * damageMultiplier * 0.1f), ForceMode2D.Impulse);
			}
			if (doingTheGigaSlam && col.enabled) {
				col.enabled = false;
			}
			if (!pc.isGrounded() && !doingTheGigaSlam && !ph.hurt && !spinning) {
				mon.AddForce(new Vector2(0f, -3f) * mon.mass * (knockbackLevel[knockback] * damageMultiplier * 0.1f), ForceMode2D.Impulse);
				StartCoroutine(gigaSlam());
			}
		}
		if (other.gameObject.tag == "Monster Projectile") {
			try {
				MonsterProjectile monProj = other.GetComponent<MonsterProjectile>();
				monProj.setDir(monProj.owner.transform.position - monProj.transform.position);
				monProj.gameObject.tag = "Projectile";
			}
			catch {
				
			}
		}
	}
}
