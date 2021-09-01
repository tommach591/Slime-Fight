using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalArrow : Projectile
{
    protected bool shattered = false;
    protected Vector3 target;

    protected Vector3 spawn;
    protected CrystalArrow copy;
    [SerializeField] Sprite[] crystals;

	new protected void OnTriggerEnter2D(Collider2D other) {
		if (other.gameObject.tag == "Monster") {
            Monster mon = other.gameObject.GetComponent<Monster>();
            Rigidbody2D monRb2D = other.gameObject.GetComponent<Rigidbody2D>();
			if (mon.isFacingLeft()) {
				monRb2D.AddForce(new Vector2(1f, 1f) * monRb2D.mass * knockbackLevel[knockback] * chargeMultiplier * 0.1f, ForceMode2D.Impulse);
			}
			else {
				monRb2D.AddForce(new Vector2(-1f, 1f) * monRb2D.mass * knockbackLevel[knockback] * chargeMultiplier * 0.1f, ForceMode2D.Impulse);
			}

            if (!shattered) {
                StartCoroutine(crystalStrike());
            }
            else {
			    Invoke("DestroyProjectile", 0.05f);
            }
		}
		if (other.gameObject.tag == "Ground") {
            DestroyProjectile();
		}
	}

    protected IEnumerator crystalStrike() {
        target = transform.position;
        yield return new WaitForSeconds(0.15f);
        for (int i = 0; i < chargeMultiplier; i++) {
            spawn = new Vector3(target.x + Random.Range(-10f, 10f), target.y + Random.Range(15f, 20f), target.z);
            copy = Instantiate(this, spawn, Quaternion.LookRotation(target - spawn));
            copy.GetComponent<SpriteRenderer>().sprite = crystals[Random.Range(0, crystals.Length)];
            copy.transform.localScale += new Vector3(0.7f, 0.7f, 0.7f);
            copy.setSpawnPosition(spawn);
            copy.setDir(target - copy.transform.position);
            copy.setSpeed(speed);
            copy.setLifeTime(lifeTime);
            copy.setAttack(attack);
            copy.setCritChance(critChance);
            copy.setCritDamage(critDamage);
            copy.setKnockback(knockback);
            copy.setMultiplier(chargeMultiplier);
            copy.shattered = true;
            yield return new WaitForSeconds(0.05f);
        }
        DestroyProjectile();
    }

    
}
