using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Tomato : Monster
{
    protected Animator anim;
    public string[] state;
    protected Platform platforms;

    public bool aggroed = false;
    protected bool attacking = false;

	protected bool tackleOnCooldown = false;
	protected float tackleCooldown = 4f;

	protected bool leavesOnCooldown = false;
	protected float leavesCooldown = 8f;
    [SerializeField] protected MonsterProjectile leaves;
    protected GameObject spawn;
    protected int amountOfLeaves = 2;

	protected int attackChoice;
	[SerializeField] protected int numberOfMoves;
	protected bool selectingAttack = false;

	protected bool selectingIdle = false;
	protected bool standStill = false;
	protected int idleChoice;

    protected bool angry = false;
    protected GameObject hostage;
    [SerializeField] protected GameObject kuro;
    protected GameObject kuroFree;

    // Start is called before the first frame update
    void Awake()
    {
		setUp();
		anim = GetComponent<Animator>();
        spawn = transform.Find("Spawn").gameObject;
        hostage = transform.Find("Hostage").gameObject;
        if (pu.rescuedKuro) {
            hostage.SetActive(false);
        }
		platforms = FindObjectOfType<Platform>();
		if (platforms != null) {
			Physics2D.IgnoreCollision(platforms.GetComponent<TilemapCollider2D>(), col);
			Physics2D.IgnoreCollision(platforms.GetComponent<CompositeCollider2D>(), col);
		}
    }

    void Update()
    {
		if (aggroed && !dead && !hurt) {
			if (!selectingAttack) {
				StartCoroutine(selectAttack());
			}
			if (!tackleOnCooldown && !attacking && attackChoice == 0) {
				StartCoroutine(tackle());
			}
			if ((!leavesOnCooldown && !attacking && attackChoice == 1)) {
				StartCoroutine(shootLeaves());
			}
			else if (!attacking) {
				movement();
			}
			lookAtPlayer();
		}
		else if (!dead && !hurt) {
			idleMovements();
		}
        if (health <= maxHealth / 2 && !angry) {
            angry = true;
            anim.Play(state[2], 0, 0);
            rb2D.mass *= 2;
            StartCoroutine(superSize());
            tackleCooldown = 2f;
            leavesCooldown = 4f;
            attack *= 2;
            healBy((int)(maxHealth / 2));
            amountOfLeaves = 3;
            damageOffset *= 2;
        }
		checkConditions();
		StartCoroutine(AnimationState());
    }

    public IEnumerator superSize() {
        attacking = true;
		immune = true;
        Vector3 scaleSize = new Vector3(transform.localScale.x / 10, transform.localScale.y / 10, transform.localScale.z / 10);
        Vector3 hostageResize = new Vector3(0.05f, 0.05f, 0.05f);
        Vector3 hostageReposition = new Vector3(0, 0.27f, 0);
        for (int i = 0; i < 10; i++) {
            transform.localScale += scaleSize;
            hostage.transform.localScale -= hostageResize;
            hostage.transform.GetChild(0).transform.localPosition += hostageReposition;
            yield return new WaitForSeconds(0.2f);
        }
        transform.localScale = new Vector3(2f, 2f, 2f);
        attacking = false;
		immune = false;
    }

    public IEnumerator shootLeaves() {
        attacking = true;
        leavesOnCooldown = true;

        for (int i = 0; i < amountOfLeaves; i++) {
            MonsterProjectile leavesCopy = Instantiate(leaves, transform.position, Quaternion.identity);
            leavesCopy.setDir(spawn.transform.position - leavesCopy.transform.position);
            leavesCopy.setAttack(attack);
            if (angry) {
                leavesCopy.transform.localScale *= 2;
                leavesCopy.setSpeed(leavesCopy.getSpeed() * 2);
            }
            leavesCopy.owner = this;
            yield return new WaitForSeconds(0.3f);
            leavesCopy.setDir(player.transform.position - leavesCopy.transform.position);
            yield return new WaitForSeconds(0.3f);
        }

        yield return new WaitForSeconds(1f);

		attacking = false;
		yield return new WaitForSeconds(leavesCooldown);
		leavesOnCooldown = false;
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
		yield return new WaitForSeconds(1f);
		attacking = false;
		yield return new WaitForSeconds(tackleCooldown);
		tackleOnCooldown = false;
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
		else if (facingLeft && grounded && !dead) {
			rb2D.AddForce(new Vector2(-1.5f, 5f) * rb2D.mass * 0.5f, ForceMode2D.Impulse);
		}
		else if (!facingLeft && grounded && !dead) {
			rb2D.AddForce(new Vector2(1.5f, 5f) * rb2D.mass * 0.5f, ForceMode2D.Impulse);
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
            if (angry) {
                anim.Play(state[2], 0, 0);
            }
            else {
			    anim.Play(state[0], 0, 0);
            }
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
        if (!pu.rescuedKuro) {
            pu.rescuedKuro = true;
            kuroFree = Instantiate(kuro, hostage.transform.position, Quaternion.identity);
            kuroFree.transform.position = new Vector3(kuroFree.transform.position.x, player.transform.position.y, kuroFree.transform.position.z);
            hostage.SetActive(false);
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
