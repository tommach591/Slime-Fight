using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

public class Sword : Weapon
{
	protected int combo = 0;
	protected float lastCombo = 0f;
	protected float comboDelay = 1f;

	protected float delayBtwFullCombos = 0.15f;

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
		if (ph.hurt && col.enabled) {
			col.enabled = false;
		}
    }

	public override void attack() {
		if (!ph.dead) {
			if ((Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S)) && !cooldown) {
				ps.perfectGuardUp();
			}
			if ((Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S)) && !cooldown) {
				pc.stoppedOn();
				ps.guardUp();
				pah.playAnim(10);
			}
			else if ((Input.GetKeyUp(KeyCode.DownArrow) || Input.GetKeyUp(KeyCode.S)) && !cooldown) {
				pc.stoppedOff();
				ps.guardDown();
				ps.perfectGuardDown();
				pah.playAnim(0);
				cooldown = true;
			}
			else if ((Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.Mouse0))) {
				pc.slowFor(0.25f);

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

	public IEnumerator attackComboOne() {
		attacking = true;
		col.enabled = true;

		pah.playAnim(4);
		yield return new WaitForSeconds(0.27f);
		pah.playAnim(0);
		yield return new WaitForSeconds(delayBtwAttacks);
		attacking = false;
		col.enabled = false;
		pc.switchSlowed();
	}

	public IEnumerator attackComboTwo() {
		attacking = true;
		col.enabled = true;

		pah.playAnim(3);
		yield return new WaitForSeconds(0.27f);
		pah.playAnim(0);
		yield return new WaitForSeconds(delayBtwAttacks);
		attacking = false;
		col.enabled = false;
		pc.switchSlowed();
	}

	public IEnumerator attackComboThree() {
		attacking = true;
		col.enabled = true;
		cooldown = true;

		pah.playAnim(5);
		yield return new WaitForSeconds(0.25f);
		pah.playAnim(0);
		yield return new WaitForSeconds(delayBtwAttacks);
		attacking = false;
		col.enabled = false;
		pc.switchSlowed();
	}

	private void OnTriggerEnter2D(Collider2D other) {
		if (other.gameObject.tag == "Monster") {
			Monster mon = other.gameObject.GetComponent<Monster>();
			Rigidbody2D monRb2D = other.gameObject.GetComponent<Rigidbody2D>();
			if (mon.isFacingLeft()) {
				monRb2D.AddForce(new Vector2(1f, 1f) * monRb2D.mass * knockbackLevel[knockback] * 0.1f, ForceMode2D.Impulse);
			}
			else {
				monRb2D.AddForce(new Vector2(-1f, 1f) * monRb2D.mass * knockbackLevel[knockback] * 0.1f, ForceMode2D.Impulse);
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
