using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    private Player_Stats ps;
    private PlayerController pc;
	private Player_Animator_Handler pah;
    public HealthBar healthBar;
	[SerializeField] protected float despawnTime;

    public int health;
    public int maxHealth;

    public float iframe = 0.5f;
    public bool hurt;

    public bool dead = false;

    public bool takingDoT = false;

    // Start is called before the first frame update
    void Start()
    {
        ps = GetComponent<Player_Stats>();
        pc = GetComponent<PlayerController>();
        pah = GetComponent<Player_Animator_Handler>();
        maxHealth = ps.getMaxHealth();
        health = maxHealth;
        healthBar.setHealthBar(health, maxHealth);
    }

    // Update is called once per frame
    void Update()
    {
        healthBar.setHealthBar(health, maxHealth);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
		if (collision.gameObject.tag == "Monster" && collision.gameObject.name != "Dummy" && collision.gameObject.name != "Chiken") {
            Monster collidedMonster = collision.gameObject.GetComponent<Monster>();
            Rigidbody2D monRb2D = collidedMonster.GetComponent<Rigidbody2D>();
            if (!hurt && !ps.guardOn) {
                hurt = true;
                StartCoroutine(immuneFor(iframe));
                float dmg = collidedMonster.damageDealt();
                health -= (int)(dmg * (dmg / (dmg + ps.getDefense())));
                healthBar.setHealthBar(health, maxHealth);
            }

            if (ps.guardOn) {
                if (ps.perfectGuard && !collidedMonster.immune) {
                    collidedMonster.takeDamage(collidedMonster.flinchDamage);
                }
                else {
                    health -= 1;
                    healthBar.setHealthBar(health, maxHealth);
                }
                if (collidedMonster.isFacingLeft()) {
                    monRb2D.AddForce(new Vector2(1f, 1f) * monRb2D.mass * 25f, ForceMode2D.Impulse);
                }
                else {
                    monRb2D.AddForce(new Vector2(-1f, 1f) * monRb2D.mass * 25f, ForceMode2D.Impulse);
                }
            }

            if (health <= 0 && !dead) {
                Physics2D.IgnoreCollision(collision.gameObject.GetComponent<Collider2D>(), GetComponent<BoxCollider2D>());
                StartCoroutine(die());
            }
		}
    }

    void OnTriggerEnter2D (Collider2D other) {
		if (other.gameObject.tag == "Monster Projectile") {
            MonsterProjectile collidedProj = other.gameObject.GetComponent<MonsterProjectile>();
            if (!hurt && !ps.guardOn) {
                hurt = true;
                StartCoroutine(immuneFor(iframe));
                float dmg = collidedProj.damageDealt();
                health -= (int)(dmg * (dmg / (dmg + ps.getDefense())));
                healthBar.setHealthBar(health, maxHealth);
            }

            if (ps.guardOn && !ps.perfectGuard) {
                health -= 1;
                healthBar.setHealthBar(health, maxHealth);
            }
        }
    }

    void OnTriggerStay2D (Collider2D other) {
		if (other.gameObject.tag == "Poison" && !takingDoT) {
            StartCoroutine(damageOverTime());
        }
        if (health <= 0 && !dead) {
            Physics2D.IgnoreCollision(other, pc.GetComponent<BoxCollider2D>());
            StartCoroutine(die());
        }
    }

    public IEnumerator damageOverTime() {
        takingDoT = true;
        health -= 1;
        healthBar.setHealthBar(health, maxHealth);
        pc.slowFor(1f);
        yield return new WaitForSeconds(1f);
        takingDoT = false;
    }

    private IEnumerator immuneFor(float x) {
        yield return new WaitForSeconds(x);
        hurt = false;
    }

    private IEnumerator die()
    {
        dead = true;
        if (!pc.getFacingLeft()) {
            pc.flip();
        }
        pc.stopAttack();
        pc.switchStopped();
        ps.guardDown();
        ps.coins /= 2;
        yield return new WaitForSeconds(despawnTime);
        heal();
        pc.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
        pc.startAttack();
        pc.switchStopped();
        pc.dontFlip = false;
        dead = false;
        SceneManager.LoadScene("TownyTown");
        yield return new WaitForSeconds(0.5f);
        pc.resetConditions();
    }

    public void heal() {
        maxHealth = ps.getMaxHealth();
        health = maxHealth;
        healthBar.setHealthBar(health, maxHealth);
    }

    public void healBy(int x) {
        maxHealth = ps.getMaxHealth();
        if (health + x < maxHealth) {
            health += x;
        }
        else {
            health = maxHealth;
        }
        healthBar.setHealthBar(health, maxHealth);
    }
}
