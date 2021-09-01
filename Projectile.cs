using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
	protected Collider2D col;
	protected Rigidbody2D rb2D;
	protected SpriteRenderer sr;
	
    [SerializeField] protected float speed;
    [SerializeField] protected float lifeTime;
    protected Vector3 spawnPosition;
	protected int attack;
	protected float critChance;
	protected float critDamage;
	protected int knockback;
	protected int chargeMultiplier = 1;

    protected Vector2 dir;
	protected float angle;

	protected Dictionary<int, float> knockbackLevel = new Dictionary<int, float>() {
        {0, 0f},
        {1, 25f},
		{2, 50f},
		{3, 100f},
		{4, 200f},
		{5, 400f}
    };

    // Start is called before the first frame update
    void Start()
    {
        setUp();
    }

    // Update is called once per frame
    protected void Update()
    {
        fire();
    }

	protected void setUp() {
		Invoke("DestroyProjectile", lifeTime);
		col = GetComponent<Collider2D>();
		rb2D = GetComponent<Rigidbody2D>();
		sr = GetComponent<SpriteRenderer>();
	}

	protected void fire() {
		dir.Normalize();
		rb2D.velocity = dir * speed;
		angle = Mathf.Atan2(rb2D.velocity.y, rb2D.velocity.x) * Mathf.Rad2Deg + 180f;
		transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
	}

	public int damageDealt() {
		return attack;
	}

	public int critDamageDealt() {
		return (int)(attack * critDamage);
	}

	public bool isCrit() {
		if (Random.Range(0f, 100f) <= critChance) {
			return true;
		}
		else {
			return false;
		}
	}

	public void setSpawnPosition(Vector3 x) {
		spawnPosition = x;
	}

	public int getAttack() { 
		return attack;
	}

	public void setAttack(int x) {
		attack = x;
	}

	public int getMultiplier() {
		return chargeMultiplier;
	}

	public void setMultiplier(int x) {
		chargeMultiplier = x;
	}

	public int getKnockback() {
		return knockback;
	}

	public void setKnockback(int x) {
		knockback = x;
	}

	public float getCritChance() {
		return critChance;
	}

	public void setCritChance(float x) {
		critChance = x;
	}

	public float getCritDamage() {
		return critChance;
	}

	public void setCritDamage(float x) {
		critDamage = x;
	} 

	public float getSpeed() {
		return speed;
	}

	public void setSpeed(float x) {
		speed = x;
	}

	public float getLifeTime() {
		return lifeTime;
	}

	public void setLifeTime(float x) {
		lifeTime = x;
	}

	public Vector2 getDir() {
		return dir;
	}

	public void setDir(Vector2 x) {
		dir = x;
	}

	private void OnCollisionEnter2D(Collision2D other) {

	}

	protected void OnTriggerEnter2D(Collider2D other) {
		if (other.gameObject.tag == "Monster") {
            Monster mon = other.gameObject.GetComponent<Monster>();
            Rigidbody2D monRb2D = other.gameObject.GetComponent<Rigidbody2D>();
			if (mon.isFacingLeft()) {
				monRb2D.AddForce(new Vector2(1f, 1f) * monRb2D.mass * knockbackLevel[knockback] * chargeMultiplier * 0.1f, ForceMode2D.Impulse);
			}
			else {
				monRb2D.AddForce(new Vector2(-1f, 1f) * monRb2D.mass * knockbackLevel[knockback] * chargeMultiplier * 0.1f, ForceMode2D.Impulse);
			}

			Invoke("DestroyProjectile", 0.05f);
		}
		if (other.gameObject.tag == "Ground") {
			DestroyProjectile();
		}
	}

    public void DestroyProjectile()
    {
        Destroy(gameObject);
    }
}
