using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Bee : Jolly
{
	protected bool summonOnCooldown = false;
	protected float summonCooldown = 20f;
	protected int summonCount = 2;

	protected bool slamming = false;

	[SerializeField] protected Jolly jolly;
	protected Bee copyBee;

    protected Platform platforms;

    void Awake()
    {
		setUp();
		anim = GetComponent<Animator>();
		platforms = FindObjectOfType<Platform>();
		if (platforms != null) {
			Physics2D.IgnoreCollision(platforms.GetComponent<TilemapCollider2D>(), col);
			Physics2D.IgnoreCollision(platforms.GetComponent<CompositeCollider2D>(), col);
		}
    }

    // Update is called once per frame
    void Update()
    {
		if (aggroed && !dead && !hurt) {
			if (!selectingAttack) {
				StartCoroutine(selectAttack());
			}
			if (!tackleOnCooldown && !attacking && attackChoice == 1) {
				StartCoroutine(tackle());
			}
			if ((!teleOnCooldown && !attacking && attackChoice == 2) || stuckCounter == 2) {
				StartCoroutine(teleport());
			}
			if ((!summonOnCooldown && !attacking && grounded && attackChoice == 3)) {
				StartCoroutine(summon());
			}
			else if (!attacking) {
				movement();
			}
			lookAtPlayer();
		}
		else if (!dead && !hurt) {
			idleMovements();
		}
		checkConditions();
		StartCoroutine(AnimationState());
    }

    new void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision);
    }

	public new void movement() {
		if (player.transform.position.y > transform.position.y + 5f && grounded && !dead) {
			if (facingLeft) {
				rb2D.AddForce(new Vector2(-2f, 8f) * rb2D.mass, ForceMode2D.Impulse);
			}
			else if (!facingLeft) {
				rb2D.AddForce(new Vector2(2f, 8f) * rb2D.mass, ForceMode2D.Impulse);
			}
		}
		if (player.transform.position.y + 50f < transform.position.y && !grounded && !dead && !slamming) {
			StartCoroutine(slamDown());
		}
		else if (facingLeft && grounded && !dead) {
			rb2D.AddForce(new Vector2(-2f, 5f) * rb2D.mass, ForceMode2D.Impulse);
		}
		else if (!facingLeft && grounded && !dead) {
			rb2D.AddForce(new Vector2(2f, 5f) * rb2D.mass, ForceMode2D.Impulse);
		}
	}

	public IEnumerator slamDown() {
		slamming = true;
		yield return new WaitForSeconds(0.5f);
		rb2D.velocity = Vector2.zero;
		rb2D.AddForce(new Vector2(0f, -15f) * rb2D.mass, ForceMode2D.Impulse);
		yield return new WaitForSeconds(1f);
		slamming = false;
	}

	public new IEnumerator tackle() {
		tackleOnCooldown = true;
		attacking = true;
		rb2D.AddForce(new Vector2(0, 5f) * rb2D.mass * 0.25f, ForceMode2D.Impulse);
		yield return new WaitForSeconds(0.25f);
		if (facingLeft && !dead) {
			rb2D.AddForce(new Vector2(-10f, 5f) * rb2D.mass * 2f, ForceMode2D.Impulse);
		}
		else if (!facingLeft && !dead) {
			rb2D.AddForce(new Vector2(10f, 5f) * rb2D.mass * 2f, ForceMode2D.Impulse);
		}
		yield return new WaitForSeconds(0.5f);
		attacking = false;
		yield return new WaitForSeconds(tackleCooldown);
		tackleOnCooldown = false;
	}

	public IEnumerator summon() {
		summonOnCooldown = true;
		attacking = true;

			for (int i = 0; i < summonCount; i++) {
				copy = Instantiate(jolly, transform.position, Quaternion.identity);
				copy.player = this.player;
				copyJolly = copy.GetComponent<Jolly>();
				copy.transform.localScale = new Vector3(1f, 1f, 1f);
				copyJolly.aggroed = true;
				copy.maxHealth = 1;
				copyJolly.attack = 1;
				copy.rb2D.mass = 10;
				copyJolly.setUp();
				Physics2D.IgnoreCollision(this.col, copyJolly.col);
				if (facingLeft) {
					copy.rb2D.AddForce(new Vector2(-5f, 0) * copy.rb2D.mass * 10f, ForceMode2D.Impulse);
				}
				else {
					copy.rb2D.AddForce(new Vector2(5f, 0) * copy.rb2D.mass * 10f, ForceMode2D.Impulse);
				}
				copyJolly.copies = 0;
				copyJolly.rewardMoneyHigh = 0;
				copy.Invoke("minionDie", 5f);
				yield return new WaitForSeconds(0.5f);
			}

		yield return new WaitForSeconds(3f);
		attacking = false;
		yield return new WaitForSeconds(summonCooldown);
		summonOnCooldown = false;
	}
	
    protected override IEnumerator die()
    {
		dead = true;
		StopCoroutine(summon());
		anim.Play(state[1], 0, 0);
		Physics2D.IgnoreCollision(player.GetComponent<BoxCollider2D>(), col);
		for (int i = 0; i < 3; i++) {
			int amount = (int)(Random.Range(rewardMoneyLow, rewardMoneyHigh));
			if (amount != 0) {
				rewardCopy = Instantiate(reward, transform.position, Quaternion.identity);
				rewardCopy.value = amount;
			}
			yield return new WaitForSeconds(0.5f);
		}
		anim.Play(state[2], 0, 0);
        yield return new WaitForSeconds(despawnTime);

		if (health <= 0 && copies > 0) {
			for(int i = 0; i < 2; i++) {
				copy = Instantiate(this, transform.position, Quaternion.identity);
				copy.player = this.player;
				copyBee = copy.GetComponent<Bee>();
				copy.transform.localScale = new Vector3(this.transform.localScale.x / 2f, this.transform.localScale.y / 2f, this.transform.localScale.z / 2f);
				copyBee.aggroed = true;
				copy.maxHealth = (int)(maxHealth / 2);
				copyBee.attack = attack - 2;
				copy.rb2D.mass = (rb2D.mass / 2);
				copyBee.summonCount = summonCount - 1;
				copyBee.setUp();
				copy.rb2D.AddForce(new Vector2(Random.Range(-2f, 2f), Random.Range(0f, 3f)) * copy.rb2D.mass * 10f, ForceMode2D.Impulse);
				copyBee.copies = copies - 1;
			}
		}

        Destroy(gameObject);
    }

}
