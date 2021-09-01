using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ApexJolly : Jolly
{

	protected bool slamming = false;

  protected bool dragPlayer = false;
  protected bool dragOnCooldown = false;
	protected float dragCooldown = 6f;
  [SerializeField] protected Mist mistHand;
  protected Mist mistHandCopy;
  protected Vector3 target;
  public bool hitPlayer = false;

  protected bool poisonOnCooldown = false;
	protected float poisonCooldown = 4f;
  [SerializeField] protected PoisonMist poisonMist;
  protected PoisonMist poisonMistCopy;

  protected bool rainOnCooldown = false;
	protected float rainCooldown = 8f;
  [SerializeField] protected RainMist rainMist;
  protected RainMist rainMistCopy;

  void Awake()
  {
    setUp();
    anim = GetComponent<Animator>();
    platformTc2d = FindObjectOfType<Platform>().GetComponent<TilemapCollider2D>();
    platformCc2d = FindObjectOfType<Platform>().GetComponent<CompositeCollider2D>();
    tackleCooldown = 3f;
  }

  void Update()
  {
    if (aggroed && !dead && !hurt) {
      if (!selectingAttack) {
        StartCoroutine(selectAttack());
      }
      if (!tackleOnCooldown && !attacking && (attackChoice == 1 || attackChoice == 0)) {
        StartCoroutine(tackle());
      }
      if (!poisonOnCooldown && !tackleOnCooldown && !attacking && attackChoice == 2  && grounded) {
        StartCoroutine(spawnPoison());
      }
      if (!dragOnCooldown && !attacking && attackChoice == 3 && grounded) {
        StartCoroutine(dragAttack());
      }
      if (!rainOnCooldown && !attacking && attackChoice == 4 && grounded) {
        StartCoroutine(rainPoison());
      }
      else if (!attacking) {
        movement();
        if (player.transform.position.y + 7.5f < transform.position.y && !grounded && !dead && !slamming) {
          StartCoroutine(slamDown());
        }
      }
      lookAtPlayer();
    }
    else if (!dead && !hurt) {
      idleMovements();
    }
    checkConditions();
    StartCoroutine(AnimationState());
    if (hitPlayer && mistHandCopy == null) {
      attacking = false;
      hitPlayer = false;
      anim.Play(state[0], 0, 0);
    }
  }

	public IEnumerator slamDown() {
		slamming = true;
		yield return new WaitForSeconds(0.5f);
		rb2D.velocity = Vector2.zero;
    rb2D.gravityScale = 10f;
		rb2D.AddForce(new Vector2(0f, -15f) * rb2D.mass, ForceMode2D.Impulse);
    while (!grounded) {
		  yield return new WaitForSeconds(0.01f);
    }
    rb2D.gravityScale = 5f;
    poisonMistCopy = Instantiate(poisonMist, transform.position, Quaternion.identity);
    poisonMistCopy.transform.localScale *= 0.5f;
		slamming = false;
	}

  public IEnumerator dragAttack() 
  {
    dragOnCooldown = true;
    attacking = true;
    hitPlayer = false;

    anim.Play(state[3], 0, 0);
    yield return new WaitForSeconds(0.5f);
    mistHandCopy = Instantiate(mistHand, transform.position, Quaternion.identity);
    mistHandCopy.setSpawnPosition(transform.position);
    mistHandCopy.apex = this;
    target = player.transform.position - transform.position;
    mistHandCopy.setDir(target);
        
    yield return new WaitForSeconds(0.75f);
    if (hitPlayer) {
      anim.Play(state[4], 0, 0);
      StartCoroutine(tackle());
    }
    else {
      anim.Play(state[4], 0, 0);
      yield return new WaitForSeconds(2f);
      attacking = false;
      anim.Play(state[0], 0, 0);
    }

    yield return new WaitForSeconds(dragCooldown);
    dragOnCooldown = false;
  }

  public IEnumerator spawnPoison() {
    poisonOnCooldown = true;
    attacking = true;

    anim.Play(state[5], 0, 0);
    yield return new WaitForSeconds(0.5f);
    poisonMistCopy = Instantiate(poisonMist, transform.position, Quaternion.identity);
    yield return new WaitForSeconds(0.5f);
    anim.Play(state[0], 0, 0);
    attacking = false;

    yield return new WaitForSeconds(poisonCooldown);
    poisonOnCooldown = false;
  }

  public IEnumerator rainPoison() {
    rainOnCooldown = true;
    attacking = true;

    anim.Play(state[6], 0, 0);
    yield return new WaitForSeconds(0.34f);
    
    for (int i = 0; i < 8; i++) {
      rainMistCopy = Instantiate(rainMist, transform.position, Quaternion.identity);
      Physics2D.IgnoreCollision(rainMistCopy.GetComponent<CircleCollider2D>(), col);
      rainMistCopy.setDir(player.transform.position - transform.position + new Vector3(Random.Range(-50, 50), 50f, transform.position.z));
      rainMistCopy.transform.localScale *= 0.75f;
    }

    yield return new WaitForSeconds(0.33f);
    anim.Play(state[0], 0, 0);
    attacking = false;

    yield return new WaitForSeconds(rainCooldown);
    rainOnCooldown = false;
  }

  protected override IEnumerator die()
  {
		dead = true;
		anim.Play(state[0], 0, 0);
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

    Destroy(gameObject);
  }
}
