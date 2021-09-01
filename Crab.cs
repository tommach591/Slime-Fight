using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Crab : Monster
{
    protected Animator anim;
    public string[] state;
    protected TilemapCollider2D platformTc2d;
    protected CompositeCollider2D platformCc2d;

    public bool aggroed = false;
    protected bool attacking = false;
	protected bool leap = false;

	public int attackChoice;
	[SerializeField] protected int numberOfMoves;
	protected bool selectingAttack = false;

	protected bool selectingIdle = false;
	protected bool standStill = false;
	protected int idleChoice;

	protected bool ignorePlatformOnCooldown = false;
	protected float ignorePlatformCooldown = 2f;

	protected bool shurikenOnCooldown = false;
	protected float shurikenCooldown = 2f;
    [SerializeField] protected MonsterProjectile shuriken;
    protected GameObject spawn;
    protected int amountOfShurikens = 2;

	protected bool guardOnCooldown = false;
	protected float guardCooldown = 5f;

    // Start is called before the first frame update
    void Start()
    {
		setUp();
		anim = GetComponent<Animator>();
		spawn = transform.Find("Spawn").gameObject;
        platformTc2d = FindObjectOfType<Platform>().GetComponent<TilemapCollider2D>();
        platformCc2d = FindObjectOfType<Platform>().GetComponent<CompositeCollider2D>();
        transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
    }

    // Update is called once per frame
    void Update()
    {
		if (aggroed && !dead && !hurt) {
			if (!selectingAttack) {
				StartCoroutine(selectAttack());
			}
			if ((!guardOnCooldown && !attacking && attackChoice == 0)) {
				StartCoroutine(guard());
			}
			if ((!shurikenOnCooldown && !attacking && !immune && grounded && attackChoice == 1)) {
				StartCoroutine(throwShurikens());
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
		if (grounded) {
			leap = false;
		}
		StartCoroutine(AnimationState());
    }

    public IEnumerator guard() {
        guardOnCooldown = true;
		attacking = true;

		anim.Play(state[3], 0, 0);
		immune = true;
        yield return new WaitForSeconds(2f);
		anim.Play(state[0], 0, 0);
		attacking = false;
		immune = false;

		yield return new WaitForSeconds(guardCooldown);
		guardOnCooldown = false;
    }

    public IEnumerator throwShurikens() {
        attacking = true;
        shurikenOnCooldown = true;

        for (int i = 0; i < amountOfShurikens; i++) {
			anim.Play(state[4], 0, 0);
			yield return new WaitForSeconds(0.35f);

            MonsterProjectile shurikenCopy = Instantiate(shuriken, transform.position, Quaternion.identity);
            shurikenCopy.setAttack(attack);
            shurikenCopy.owner = this;
            shurikenCopy.setDir(player.transform.position - shurikenCopy.transform.position);
        }

		anim.Play(state[0], 0, 0);
        yield return new WaitForSeconds(1f);

		attacking = false;
		yield return new WaitForSeconds(shurikenCooldown);
		shurikenOnCooldown = false;
    }

	public void idleMovements() {
		if (!selectingIdle) {
			StartCoroutine(flipRandom());
		}
		if (standStill) {
			StartCoroutine(standingStill());
		}
		else if (facingLeft && grounded && !dead ) {
            if (!anim.GetCurrentAnimatorStateInfo(0).IsName(state[2])) {
                anim.Play(state[2], 0, 0);
            }
			rb2D.AddForce(new Vector2(-0.5f, 0f) * rb2D.mass * 0.5f, ForceMode2D.Impulse);
		}
		else if (!facingLeft && grounded && !dead) {
            if (!anim.GetCurrentAnimatorStateInfo(0).IsName(state[2])) {
                anim.Play(state[2], 0, 0);
            }
			rb2D.AddForce(new Vector2(0.5f, 0f) * rb2D.mass * 0.5f, ForceMode2D.Impulse);
		}
	}

	public IEnumerator flipRandom() {
		selectingIdle = true;
		yield return new WaitForSeconds(2f);
		idleChoice = Random.Range(0, 4);
		if (idleChoice == 1) {
			flip();
		}
		if (idleChoice == 2) {
			standStill = true;
		}
		if (idleChoice == 3) {
			StartCoroutine(ignorePlatform());
		}
		selectingIdle = false;
	}

	public IEnumerator ignorePlatform() {
		ignorePlatformOnCooldown = true;
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
		yield return new WaitForSeconds(ignorePlatformCooldown);
		ignorePlatformOnCooldown = false;
    }

	public IEnumerator selectAttack() {
		selectingAttack = true;
		yield return new WaitForSeconds(3f);
		attackChoice = Random.Range(0, numberOfMoves);
		selectingAttack = false;
	}

	public IEnumerator standingStill() {
        if (!anim.GetCurrentAnimatorStateInfo(0).IsName(state[0]) && !anim.GetCurrentAnimatorStateInfo(0).IsName(state[3])) {
            anim.Play(state[0], 0, 0);
        }
		yield return new WaitForSeconds(3f);
		standStill = false;
	}

	public void movement() {
		if (player.transform.position.y > transform.position.y + 3f && grounded && !dead) {
			StartCoroutine(ignorePlatform());
			if (facingLeft) {
				rb2D.AddForce(new Vector2(-3f, 30f) * rb2D.mass * 0.5f, ForceMode2D.Impulse);
			}
			else if (!facingLeft) {
				rb2D.AddForce(new Vector2(3f, 30f) * rb2D.mass * 0.5f, ForceMode2D.Impulse);
			}
		}
        else if (player.transform.position.y + 5f < transform.position.y && !ignorePlatformOnCooldown) {
            StartCoroutine(ignorePlatform());
        }
		else if (facingLeft && !grounded && !dead && !leap) {
            if (!anim.GetCurrentAnimatorStateInfo(0).IsName(state[2]) && !anim.GetCurrentAnimatorStateInfo(0).IsName(state[3])) {
                anim.Play(state[2], 0, 0);
            }
			rb2D.AddForce(new Vector2(-40f, 0f) * rb2D.mass * 0.5f, ForceMode2D.Impulse);
			leap = true;
		}
		else if (!facingLeft && !grounded && !dead && !leap) {
            if (!anim.GetCurrentAnimatorStateInfo(0).IsName(state[2]) && !anim.GetCurrentAnimatorStateInfo(0).IsName(state[3])) {
                anim.Play(state[2], 0, 0);
            }
			rb2D.AddForce(new Vector2(40f, 0f) * rb2D.mass * 0.5f, ForceMode2D.Impulse);
			leap = true;
		}
		else if (facingLeft && grounded && !dead) {
            if (!anim.GetCurrentAnimatorStateInfo(0).IsName(state[2]) && !anim.GetCurrentAnimatorStateInfo(0).IsName(state[3])) {
                anim.Play(state[2], 0, 0);
            }
			rb2D.AddForce(new Vector2(-0.75f, 0f) * rb2D.mass * 0.5f, ForceMode2D.Impulse);
		}
		else if (!facingLeft && grounded && !dead) {
            if (!anim.GetCurrentAnimatorStateInfo(0).IsName(state[2]) && !anim.GetCurrentAnimatorStateInfo(0).IsName(state[3])) {
                anim.Play(state[2], 0, 0);
            }
			rb2D.AddForce(new Vector2(0.75f, 0f) * rb2D.mass * 0.5f, ForceMode2D.Impulse);
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
        if (collision.gameObject.tag == "Hunter" && ps.perfectGuard && !immune) {
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

    override protected IEnumerator die()
    {
		dead = true;
		Physics2D.IgnoreCollision(player.GetComponent<BoxCollider2D>(), col);
        anim.Play(state[1], 0, 0);
		for (int i = 0; i < 3; i++) {
			int amount = (int)(Random.Range(rewardMoneyLow, rewardMoneyHigh));
			if (amount != 0) {
				rewardCopy = Instantiate(reward, transform.position, Quaternion.identity);
				rewardCopy.value = amount;
			}
			yield return new WaitForSeconds(0.5f);
		}
        Vector3 decrease = new Vector3(transform.localScale.x / 10, transform.localScale.y / 10, transform.localScale.z / 10);
        for (int i = 0; i < 10; i++) {
            transform.localScale -= decrease;
            yield return new WaitForSeconds(despawnTime / 10f);
        }

        yield return new WaitForSeconds(0.03f);

        Destroy(gameObject);
    }
	
}
