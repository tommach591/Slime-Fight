using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tome : Weapon
{
	[SerializeField] protected Projectile magic;
	[SerializeField] protected Vector3 offset;
	protected Projectile copy;
	protected Vector3 target;
    protected Monster enemy;
	protected Monster[] enemies;
	protected float closestDistance;
	protected float distanceBtwMonster;

	protected int combo = 0;
	protected float lastCombo = 0f;
	protected float comboDelay = 1f;

	protected float delayBtwFullCombos = 1f;

	protected bool cooldown = false;

    // Start is called before the first frame update
    void Start()
    {
        setUp();
    }

    // Update is called once per frame
    void Update()
    {
		resetCombo();
		if (cooldown) {
			StartCoroutine(attackCooldown());
		}
    }

    public override void attack() {
		if ((Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.Mouse0)) && !ph.dead) {
			if (lastCombo != 0f && (Time.time - lastCombo) < comboDelay && !cooldown && !attacking) {
				if (combo == 2) {
					StartCoroutine(attackComboThree());
					combo = 0;
					lastCombo = 0f;
				}
				else if (combo == 1) {
					StartCoroutine(attackComboTwo());
					combo = 2;
					lastCombo = Time.time;
				}
			}
			else if (!cooldown && !attacking) {
				StartCoroutine(attackComboOne());
				combo = 1;
				lastCombo = Time.time;
			}
		}
	}

	public void resetCombo() {
		if (lastCombo != 0f && (Time.time - lastCombo) > comboDelay) {
			combo = 0;
			lastCombo = 0f;
		}
	}

	public IEnumerator attackCooldown() {
		yield return new WaitForSeconds(delayBtwFullCombos);
		cooldown = false;
	}

	virtual public IEnumerator attackComboOne() {
		attacking = true;

        pc.slowFor(0.25f);
		pah.playAnim(5);
        castMagic(0.75f);
		yield return new WaitForSeconds(0.25f);
        if (!pc.isGrounded()) {
            pah.playAnim(2);
        }
        else {
		    pah.playAnim(0);
        }
		yield return new WaitForSeconds(delayBtwAttacks);
		attacking = false;
	}

	virtual public IEnumerator attackComboTwo() {
		attacking = true;

        pc.slowFor(0.25f);
		pah.playAnim(5);
		yield return new WaitForSeconds(0.25f);
        castMagic(0.75f);
        castMagic(0.75f);
        if (!pc.isGrounded()) {
            pah.playAnim(2);
        }
        else {
		    pah.playAnim(0);
        }
		yield return new WaitForSeconds(delayBtwAttacks);
		attacking = false;
	}

	virtual public IEnumerator attackComboThree() {
		attacking = true;
		cooldown = true;

        pc.slowFor(0.25f);
		pah.playAnim(5);
		yield return new WaitForSeconds(0.25f);
        castMagic(0.75f);
        castMagic(0.75f);
        castMagic(0.75f);
        if (!pc.isGrounded()) {
            pah.playAnim(2);
        }
        else {
		    pah.playAnim(0);
        }
		yield return new WaitForSeconds(delayBtwAttacks);
		attacking = false;
	}

    public void castMagic(float x) {
        Vector3 spawn = new Vector3(transform.position.x + Random.Range(-10f, 10f), transform.position.y + Random.Range(5f, 10f), transform.position.z);
        try {
			enemies = FindObjectsOfType<Monster>();
			enemy = null;
			for (int i = 0; i < enemies.Length; i++) {
				distanceBtwMonster = Vector3.Distance(transform.position, enemies[i].transform.position);
				if (enemy == null || closestDistance > distanceBtwMonster) {
					closestDistance = distanceBtwMonster;
					enemy = enemies[i];
				}
			}
            target = enemy.transform.position - transform.position - offset;
            copy = Instantiate(magic, spawn, Quaternion.identity);
            copy.setSpawnPosition(spawn);
            copy.setDir(target);
            copy.setSpeed(75f);
            copy.setLifeTime(x);
        }
        catch {
            copy = Instantiate(magic, spawn, Quaternion.identity);
            copy.setSpawnPosition(spawn);
            copy.setDir(spawn);
            copy.setSpeed(0f);
            copy.setLifeTime(x);
        }
        copy.setAttack((int)((weaponAttack + (ps.getAttack() / 2)) * damageMultiplier));
        copy.setCritChance(ps.getCritChance() + critRateUp);
        copy.setCritDamage(ps.getCritDamage() + critDamageUp);
        copy.setKnockback(knockback);
        copy.setMultiplier(chargeMultiplier);

    }
}
