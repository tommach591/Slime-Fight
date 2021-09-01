using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : Item
{
	public Rigidbody2D rb2D;
	public PolygonCollider2D col;
	protected SpriteRenderer sr;

	[SerializeField] public GameObject player;
	public BoxCollider2D playerCol;
	public Rigidbody2D playerRb2D;
	public Player_Stats ps;
	public PlayerController pc;

	public bool facingLeft = true;
	protected bool hurt = false;
    [SerializeField] protected LayerMask ground;
	public bool grounded;

	[SerializeField] public int maxHealth;
	[SerializeField] public int attack;
	[SerializeField] protected float despawnTime;
	protected int health;
	protected int damage;
	[SerializeField] public int flinchDamage;

	public HealthBar healthBar;

	[SerializeField] protected Damage dmgDisplay;
	[SerializeField] protected Vector3 damageOffset;
	protected Damage dmgDisplayCopy;

	[SerializeField] protected int rewardMoneyLow;
	[SerializeField] public int rewardMoneyHigh;

	protected bool dead = false;
	public bool immune = false;

	protected Monster copy;

	[SerializeField] protected Coin reward;
	protected Coin rewardCopy;

	[SerializeField] public string monsterType;
    protected Player_Unlocks pu;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        checkConditions();
    }

    protected void OnTriggerEnter2D(Collider2D collision) {
		if (immune && (collision.tag == "Weapon" || collision.tag == "Projectile")) {
			if (gameObject.name == "Chiken" && grounded && dmgDisplayCopy == null) {
				dmgDisplayCopy = Instantiate(dmgDisplay, Vector3.zero, Quaternion.identity);
				dmgDisplayCopy.transform.SetParent(gameObject.transform);
				dmgDisplayCopy.setUp(1337, damageOffset, true);
			}
			else if (!(gameObject.name == "Chiken")) {
				dmgDisplayCopy = Instantiate(dmgDisplay, Vector3.zero, Quaternion.identity);
				dmgDisplayCopy.transform.SetParent(gameObject.transform);
				dmgDisplayCopy.setUp(0, damageOffset, false);
			}
		}
		if (collision.tag == "Weapon" && health > 0 && !immune) {
			dmgDisplayCopy = Instantiate(dmgDisplay, Vector3.zero, Quaternion.identity);
			dmgDisplayCopy.transform.SetParent(gameObject.transform);

			if (collision.GetComponent<Weapon>().isCrit()) {
				damage = collision.GetComponent<Weapon>().critDamageDealt();
				dmgDisplayCopy.setUp(damage, damageOffset, true);
			}
			else {
				damage = collision.GetComponent<Weapon>().damageDealt();
				dmgDisplayCopy.setUp(damage, damageOffset, false);
			}

			takeDamage(damage);
		}
		if (collision.tag == "Projectile" && health > 0 && !immune) {
			dmgDisplayCopy = Instantiate(dmgDisplay, Vector3.zero, Quaternion.identity);
			dmgDisplayCopy.transform.SetParent(gameObject.transform);

			if (collision.GetComponent<Projectile>().isCrit()) {
				damage = collision.GetComponent<Projectile>().critDamageDealt();
				dmgDisplayCopy.setUp(damage, damageOffset, true);
			}
			else {
				damage = collision.GetComponent<Projectile>().damageDealt();
				dmgDisplayCopy.setUp(damage, damageOffset, false);
			}

			takeDamage(damage);
		}
		if (health <= 0 && !dead) {
			StartCoroutine(die());
		}
	}

    protected void OnTriggerExit2D(Collider2D collision) {
		if (collision.tag == "Weapon") {
			damage = 0;
			hurt = false;
		}
		if (collision.tag == "Projectile") {
			damage = 0;
			hurt = false;
		}
	}

    void OnCollisionStay2D(Collision2D collision) {
		if (collision.gameObject.tag == "Coin") {
            Physics2D.IgnoreCollision(collision.gameObject.GetComponent<CircleCollider2D>(), col);
        }
	}

    void OnCollisionExit2D(Collision2D collision) {
		if (collision.gameObject.tag == "Hunter" && hurt) {
			if (health <= 0 && !dead) {
				StartCoroutine(die());
			}
			else {
				StartCoroutine(stopHurting());
			}
		}
	}

	public IEnumerator stopHurting() {
		yield return new WaitForSeconds(0.75f);
		hurt = false;
	}

	protected void checkConditions() {
		if (col.IsTouchingLayers(ground)) {
			grounded = true;
		}
		else if (!col.IsTouchingLayers(ground)) {
			grounded = false;
		}
	}

	public void lookAtPlayer() {
        if (player.transform.position.x > transform.position.x && facingLeft) {
			flip();
		}
		else if (player.transform.position.x < transform.position.x && !facingLeft) {
			flip();
		}
	}

    public void setUp() {
        rb2D = GetComponent<Rigidbody2D>();
		col = GetComponent<PolygonCollider2D>();
		sr = GetComponent<SpriteRenderer>();
		health = maxHealth;
		healthBar.setHealthBar(health, maxHealth);
		try {
			pc = FindObjectOfType<PlayerController>();
			player = pc.gameObject;
			playerCol = player.GetComponent<BoxCollider2D>();
			playerRb2D = player.GetComponent<Rigidbody2D>();
			ps = player.GetComponent<Player_Stats>();
        	pu = player.GetComponent<Player_Unlocks>();
		} 
		catch {

		}
    }

	public void flip() {
		facingLeft = !facingLeft;
		transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
	}

	public bool isFacingLeft() {
		return facingLeft;
	}

	public int damageDealt() {
		return attack;
	}

	public void takeDamage(int damage) {
		dmgDisplayCopy = Instantiate(dmgDisplay, Vector3.zero, Quaternion.identity);
		dmgDisplayCopy.transform.SetParent(gameObject.transform);
		dmgDisplayCopy.setUp(damage, damageOffset, false);
		health -= damage;
		healthBar.setHealthBar(health, maxHealth);
	}

	public void healBy(int amount) {
		health += amount;
		healthBar.setHealthBar(health, maxHealth);
	}

    protected virtual IEnumerator die()
    {
		dead = true;
		Physics2D.IgnoreCollision(player.GetComponent<BoxCollider2D>(), col);
		for (int i = 0; i < 3; i++) {
			int amount = (int)(Random.Range(rewardMoneyLow, rewardMoneyHigh));
			if (amount != 0) {
				rewardCopy = Instantiate(reward, transform.position, Quaternion.identity);
				rewardCopy.value = amount;
			}
			yield return new WaitForSeconds(0.5f);
		}
		
        yield return new WaitForSeconds(despawnTime);

        Destroy(gameObject);
    }

	protected virtual void setMaxHealth(int x) {
		maxHealth = x;
	}

	public bool getDead() {
		return dead;
	}

}
