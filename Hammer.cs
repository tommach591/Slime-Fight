using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hammer : Weapon
{

	[SerializeField] protected float downwardForce;
	protected Animator anim;

	protected int maxjumps = 1;
	protected int currentjumps = 0;

	protected bool doingTheGigaSlam = false;
	protected Vector3 rotation = new Vector3(0, 0, 5);

	[SerializeField] protected string[] state;

    void Awake()
    {
        setUp();
        anim = GetComponent<Animator>();
        anim.Play(state[0], 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
		if (ph.hurt && col.enabled) {
			col.enabled = false;
		}
    }

	public override void attack() {
		if (!attacking && !doingTheGigaSlam && !ph.dead) {
			if ((Input.GetKey(KeyCode.Z) || Input.GetKey(KeyCode.Mouse0)) && !chargeInProg) {
				StartCoroutine(charging());
			}
			if (chargeInProg || doingTheGigaSlam) {
				pc.slowFor(0.01f);
			}
			if (chargeInProg && !pc.isGrounded()) {
				pah.playAnim(6);
			}
			else if (chargeInProg && pc.isGrounded() && (!Input.GetKey(KeyCode.LeftArrow) && !Input.GetKey(KeyCode.A)) && (!Input.GetKey(KeyCode.RightArrow) && !Input.GetKey(KeyCode.D))) {
				pah.playAnim(6);
			}
			else if (chargeInProg && pc.isGrounded() && ((Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A)) || (!Input.GetKey(KeyCode.RightArrow) || !Input.GetKey(KeyCode.D)))) {
				pah.playAnim(1);
			}
			if ((Input.GetKeyUp(KeyCode.Z) || Input.GetKeyUp(KeyCode.Mouse0))) {
				StartCoroutine(swingDown());
			}
			if (chargeInProg && (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W)) && Input.GetKeyDown(KeyCode.Space) && !pc.isGrounded() && currentjumps != maxjumps) {
				rb2D.velocity = Vector2.up * 35f;
				currentjumps++;
			}
			if (pc.isGrounded()) {
				currentjumps = 0;
			}
		}
	}

	public override void resetConditions() {
		transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
		pc.stoppedOff();
		attacking = false;
		chargeInProg = false;
		chargeMultiplier = 1;
		damageMultiplier = 1;
		ps.guardDown();
		doingTheGigaSlam = false;
	}

	virtual public IEnumerator charging() {
		chargeInProg = true;
		yield return new WaitForSeconds(0.78f);
		if (chargeMultiplier == 1 && chargeInProg) {
			chargeMultiplier = 2;
			damageMultiplier = 1.5f;
			transform.localScale = new Vector3(2f, 2f, 2f);
			anim.Play(state[1], 0, 0);
			chargeInProg = false;
		}
		else if (chargeMultiplier == 2 && chargeInProg) {
			chargeMultiplier = 3;
			damageMultiplier = 2f;
			transform.localScale = new Vector3(2.5f, 2.5f, 2.5f);
			anim.Play(state[2], 0, 0);
			chargeInProg = false;
		}
		else if (chargeMultiplier == 3 && chargeInProg) {
			chargeMultiplier = 4;
			damageMultiplier = 3f;
			transform.localScale = new Vector3(3f, 3f, 3f);
			anim.Play(state[3], 0, 0);
		}
	}

	virtual public IEnumerator swingDown() {
		attacking = true;
		chargeInProg = false;
		col.enabled = true;
		pc.stoppedOn();
		pah.playAnim(6);
		yield return new WaitForSeconds(0.2f);
		pah.playAnim(7);
		rb2D.velocity = Vector2.down * downwardForce;
		while (!pc.isGrounded()) {
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
		transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
		pc.stoppedOff();
		yield return new WaitForSeconds(delayBtwAttacks);
		attacking = false;
		chargeMultiplier = 1;
		damageMultiplier = 1;
		anim.Play(state[0], 0, 0);
	}

	protected void OnTriggerEnter2D(Collider2D other) {
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
			if (!pc.isGrounded() && !doingTheGigaSlam && !ph.hurt) {
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

	protected IEnumerator gigaSlam() {
		doingTheGigaSlam = true;
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
}
