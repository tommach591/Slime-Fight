using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Jolly : Monster
{
	protected Animator anim;
	
	public string[] state;
	[SerializeField] public int copies;
	protected Jolly copyJolly;

	public bool aggroed = false;
	protected bool attacking = false;

	protected bool tackleOnCooldown = false;
	protected float tackleCooldown = 4f;

	protected bool teleOnCooldown = false;
	protected float teleCooldown = 15f;
	protected Vector3 teleLocation;

	protected int attackChoice;
	[SerializeField] protected int numberOfMoves;
	protected bool selectingAttack = false;

	protected int stuckCounter = 0;
	protected bool checkingStuck = false;

	protected bool selectingIdle = false;
	protected bool standStill = false;
	protected int idleChoice;

    protected TilemapCollider2D platformTc2d;
    protected CompositeCollider2D platformCc2d;

    // Start is called before the first frame update
    void Awake()
    {
		setUp();
		anim = GetComponent<Animator>();
        platformTc2d = FindObjectOfType<Platform>().GetComponent<TilemapCollider2D>();
        platformCc2d = FindObjectOfType<Platform>().GetComponent<CompositeCollider2D>();
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

	public void idleMovements() {
		if (!selectingIdle) {
			StartCoroutine(flipRandom());
		}
		if (standStill) {
			StartCoroutine(standingStill());
		}
		else if (facingLeft && grounded && !dead ) {
			rb2D.AddForce(new Vector2(-1.5f, 5f) * rb2D.mass * 0.5f, ForceMode2D.Impulse);
		}
		else if (!facingLeft && grounded && !dead) {
			rb2D.AddForce(new Vector2(1.5f, 5f) * rb2D.mass * 0.5f, ForceMode2D.Impulse);
		}
	}

	public IEnumerator flipRandom() {
		selectingIdle = true;
		yield return new WaitForSeconds(2f);
		idleChoice = Random.Range(0, 3);
		if (idleChoice == 1) {
			flip();
		}
		if (idleChoice == 2) {
			standStill = true;
		}
		selectingIdle = false;
	}

	public IEnumerator selectAttack() {
		selectingAttack = true;
		yield return new WaitForSeconds(3f);
		attackChoice = Random.Range(0, numberOfMoves);
		selectingAttack = false;
	}

	public IEnumerator standingStill() {
		yield return new WaitForSeconds(3f);
		standStill = false;
	}

	public IEnumerator tackle() {
		tackleOnCooldown = true;
		attacking = true;
		yield return new WaitForSeconds(0.50f);
		rb2D.AddForce(new Vector2(0, 5f) * rb2D.mass * 0.5f, ForceMode2D.Impulse);
		yield return new WaitForSeconds(0.25f);
		if (facingLeft && !dead) {
			rb2D.AddForce(new Vector2(-10f, 5f) * rb2D.mass * 4f, ForceMode2D.Impulse);
		}
		else if (!facingLeft && !dead) {
			rb2D.AddForce(new Vector2(10f, 5f) * rb2D.mass * 4f, ForceMode2D.Impulse);
		}
		yield return new WaitForSeconds(0.5f);
		attacking = false;
		yield return new WaitForSeconds(tackleCooldown);
		tackleOnCooldown = false;
	}
	
	public IEnumerator teleport() {
		teleOnCooldown = true;
		attacking = true;
		anim.Play(state[3], 0, 0);
		Collider2D weaponCol = FindObjectOfType<Weapon>().GetComponent<Collider2D>();
		Physics2D.IgnoreCollision(playerCol, col, true); 
		immune = true;

		teleLocation = new Vector3(player.transform.position.x, player.transform.position.y + 15f);
		yield return new WaitForSeconds(1f);

		Physics2D.IgnoreCollision(playerCol, col, false); 
		immune = false;

		transform.position = teleLocation;
		anim.Play(state[4], 0, 0);
		yield return new WaitForSeconds(1f);
		anim.Play(state[0], 0, 0);
		attacking = false;

		checkingStuck = false;
		stuckCounter = 0;

		yield return new WaitForSeconds(teleCooldown);
		teleOnCooldown = false;
	}

	public void movement() {
		if (player.transform.position.y > transform.position.y + 5f && grounded && !dead) {
			if (facingLeft) {
				rb2D.AddForce(new Vector2(-1.5f, 12f) * rb2D.mass * 0.5f, ForceMode2D.Impulse);
			}
			else if (!facingLeft) {
				rb2D.AddForce(new Vector2(1.5f, 12f) * rb2D.mass * 0.5f, ForceMode2D.Impulse);
			}
		}
		if (player.transform.position.y + 2f < transform.position.y && grounded && !dead) {
			StartCoroutine(ignorePlatform());
		}
		else if (facingLeft && grounded && !dead) {
			rb2D.AddForce(new Vector2(-1.5f, 5f) * rb2D.mass * 0.5f, ForceMode2D.Impulse);
		}
		else if (!facingLeft && grounded && !dead) {
			rb2D.AddForce(new Vector2(1.5f, 5f) * rb2D.mass * 0.5f, ForceMode2D.Impulse);
		}
	}

	public IEnumerator ignorePlatform() {
		try {
			Physics2D.IgnoreCollision(col, platformTc2d, true);
			Physics2D.IgnoreCollision(col, platformCc2d, true);
		}
		catch {

		}
        yield return new WaitForSeconds(0.75f);
		try {
			Physics2D.IgnoreCollision(col, platformTc2d, false);
			Physics2D.IgnoreCollision(col, platformCc2d, false);
		}
		catch {
			
		}
    }

    protected new void OnTriggerEnter2D(Collider2D collision) {
		base.OnTriggerEnter2D(collision);
		if (!aggroed && (collision.tag == "Weapon" || collision.tag == "Projectile" || collision.tag == "Hunter")) {
			aggroed = true;
		}
		if (collision.tag == "Weapon" && damage >= flinchDamage && !dead && !attacking) {
			hurt = true;
			anim.Play(state[1], 0, 0);
		}
		if (collision.tag == "Projectile" && damage >= flinchDamage && !dead && !attacking) {
			hurt = true;
			anim.Play(state[1], 0, 0);
		}
	}

    protected new void OnTriggerExit2D(Collider2D collision) {
		base.OnTriggerExit2D(collision);
		if (collision.tag == "Weapon") {

		}
	}

    protected void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Hunter" && !ps.guardOn) {
			playerRb2D.velocity = Vector3.zero;
			if (pc.getFacingLeft()) {
				playerRb2D.AddForce(new Vector2(1f, 1f) * rb2D.mass * (playerRb2D.mass / 4), ForceMode2D.Impulse);
			}
			else {
				playerRb2D.AddForce(new Vector2(-1f, 1f) * rb2D.mass * (playerRb2D.mass / 4), ForceMode2D.Impulse);
			}
		}
        if (collision.gameObject.tag == "Hunter" && ps.perfectGuard) {
			hurt = true;
			anim.Play(state[1], 0, 0);
		}
		if (collision.gameObject.tag == "Monster") {
			Physics2D.IgnoreCollision(collision.gameObject.GetComponent<Collider2D>(), col);
		}
    }

	protected IEnumerator AnimationState() {
		yield return new WaitForSeconds(0.3f);
		if (anim.GetCurrentAnimatorStateInfo(0).IsName(state[1]) && !hurt && !dead && !attacking) {
			yield return new WaitForSeconds(0.25f);
			anim.Play(state[0], 0, 0);
		}
	}

    protected override IEnumerator die()
    {
		dead = true;
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
				copyJolly = copy.GetComponent<Jolly>();
				copy.transform.localScale = new Vector3(this.transform.localScale.x / 2f, this.transform.localScale.y / 2f, this.transform.localScale.z / 2f);
				copyJolly.aggroed = true;
				copy.maxHealth = (int)(maxHealth / 2);
				copyJolly.attack = attack - 1;
				copy.rb2D.mass = (rb2D.mass / 2);
				copyJolly.setUp();
				Monster[] allies = FindObjectsOfType<Monster>();
				for (int j = 0; j < allies.Length; j++) {
					Physics2D.IgnoreCollision(allies[j].col, copy.col);
				}
				copy.rb2D.AddForce(new Vector2(Random.Range(-5f, 5f), Random.Range(2f, 5f)) * copy.rb2D.mass * 10f, ForceMode2D.Impulse);
				copyJolly.copies = copies - 1;
			}
		}

        Destroy(gameObject);
    }
	
    protected IEnumerator minionDie()
    {
		if (!dead) {
			dead = true;
			anim.Play(state[1], 0, 0);
			Physics2D.IgnoreCollision(player.GetComponent<BoxCollider2D>(), col);

			anim.Play(state[2], 0, 0);
			yield return new WaitForSeconds(despawnTime);

			Destroy(gameObject);
		}
		else {
			yield return new WaitForSeconds(0.01f);
		}
    }
}
