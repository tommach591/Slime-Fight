using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Claw : Weapon
{

	[SerializeField] protected Projectile magic;
	protected Projectile copy;
	protected Vector3 target;

    protected GameObject castPoint;
    protected float force = 10f;

    protected TilemapCollider2D[] ground;

    protected float upAttackCooldown = 1.5f;
    protected bool upAttackOnCooldown = false;

    protected float downAttackCooldown = 1.5f;
    protected bool downAttackOnCooldown = false;

    // Start is called before the first frame update
    void Start()
    {
        setUp();
        castPoint = transform.GetChild(0).gameObject;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void castMagic(float x) {
        copy = Instantiate(magic, castPoint.transform.position, Quaternion.identity);
		copy.setDir(target);
        copy.setSpawnPosition(castPoint.transform.position);
        copy.setSpeed(100f);
        copy.setLifeTime(x);
        copy.setAttack((int)((weaponAttack + (ps.getAttack() / 2)) * damageMultiplier));
        copy.setCritChance(ps.getCritChance() + critRateUp);
        copy.setCritDamage(ps.getCritDamage() + critDamageUp);
        copy.setKnockback(knockback);
        copy.setMultiplier(chargeMultiplier);
    }

	public override void attack() {
		if (!ph.dead) {
			if ((Input.GetKey(KeyCode.Z) || Input.GetKey(KeyCode.Mouse0)) && (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W)) && !attacking && !upAttackOnCooldown) {
				StartCoroutine(upAttack());
			}
			else if ((Input.GetKey(KeyCode.Z) || Input.GetKey(KeyCode.Mouse0)) && (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S)) && !attacking && !downAttackOnCooldown) {
				StartCoroutine(downAttack());
			}
			else if ((Input.GetKey(KeyCode.Z) || Input.GetKey(KeyCode.Mouse0)) && (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D)) && !attacking) {
				StartCoroutine(forwardAttack());
			}
			else if ((Input.GetKey(KeyCode.Z) || Input.GetKey(KeyCode.Mouse0)) && (!Input.GetKey(KeyCode.UpArrow) && !Input.GetKey(KeyCode.W)) && (!Input.GetKey(KeyCode.DownArrow) && !Input.GetKey(KeyCode.S)) && (!Input.GetKey(KeyCode.LeftArrow) && !Input.GetKey(KeyCode.A)) && (!Input.GetKey(KeyCode.RightArrow) && !Input.GetKey(KeyCode.D)) && !attacking) {
				StartCoroutine(stabAttack());
			}
		}
	}

	virtual public IEnumerator upAttack() {
		attacking = true;
        upAttackOnCooldown = true;
		col.enabled = true;
        StartCoroutine(pc.avoidAllHitBoxFor(1.5f));

		pah.playAnim(4);
        rb2D.velocity = Vector2.zero;
		if (pc.getFacingLeft()) {
            target = new Vector2(-1f, 4f) * rb2D.mass * force;
		}
		else {
            target = new Vector2(1f, 4f) * rb2D.mass * force;
		}
        rb2D.AddForce(target, ForceMode2D.Impulse);
        pc.slowFor(0.27f);
        castMagic(0.15f);

		yield return new WaitForSeconds(0.27f);
		pah.playAnim(2);
		yield return new WaitForSeconds(delayBtwAttacks);
		attacking = false;
		col.enabled = false;

        yield return new WaitForSeconds(upAttackCooldown);
        upAttackOnCooldown = false;
	}

	virtual public IEnumerator downAttack() {
		attacking = true;
        downAttackOnCooldown = true;
		col.enabled = true;
        StartCoroutine(pc.avoidAllHitBoxFor(1.5f));

		pah.playAnim(5);
		if (pc.getFacingLeft()) {
            target = new Vector2(-5f, -5f) * rb2D.mass * force;
		}
		else {
            target = new Vector2(5f, -5f) * rb2D.mass * force;
		}
        rb2D.AddForce(target, ForceMode2D.Impulse);
        pc.slowFor(0.25f);
        castMagic(0.15f);

		yield return new WaitForSeconds(0.25f);
		pah.playAnim(2);
		yield return new WaitForSeconds(delayBtwAttacks);
		attacking = false;
		col.enabled = false;

        yield return new WaitForSeconds(downAttackCooldown);
        downAttackOnCooldown = false;
	}

	virtual public IEnumerator forwardAttack() {
		attacking = true;
		col.enabled = true;

		pah.playAnim(3);
		if (pc.getFacingLeft()) {
            target = new Vector2(-1.2f, 0f) * rb2D.mass * force;
		}
		else {
            target = new Vector2(1.2f, 0f) * rb2D.mass * force;
		}
        rb2D.AddForce(target, ForceMode2D.Impulse);
        pc.slowFor(0.27f);
		yield return new WaitForSeconds(0.20f);
        castMagic(0.002f);
        yield return new WaitForSeconds(0.05f);
        castMagic(0.002f);
		yield return new WaitForSeconds(0.02f);
		pah.playAnim(0);
		yield return new WaitForSeconds(delayBtwAttacks);
		attacking = false;
		col.enabled = false;
	}

	virtual public IEnumerator stabAttack() {
		attacking = true;
		col.enabled = true;
		pah.playAnim(3);
		if (pc.getFacingLeft()) {
            target = new Vector2(-2f, 0f) * rb2D.mass * force;
		}
		else {
            target = new Vector2(2f, 0f) * rb2D.mass * force;
		}
        pc.slowFor(0.27f);
		yield return new WaitForSeconds(0.20f);
        castMagic(0.002f);
        yield return new WaitForSeconds(0.05f);
        castMagic(0.002f);
		yield return new WaitForSeconds(0.02f);
		pah.playAnim(0);
		yield return new WaitForSeconds(delayBtwAttacks);
		attacking = false;
		col.enabled = false;
	}
}
