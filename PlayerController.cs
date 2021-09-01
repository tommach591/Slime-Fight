using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
	private static PlayerController playerInstance;

	void Awake(){
		DontDestroyOnLoad(this);
			
		if (playerInstance == null) {
			playerInstance = this;
		} 
		else {
			Destroy(gameObject);
		}
	}

	private Rigidbody2D rb2D;
	private Collider2D col;

    [SerializeField] private LayerMask ground;
	[SerializeField] private LayerMask water;
	[SerializeField] private LayerMask monster;

	private Player_Animator_Handler pah;
	private PlayerHealth ph;
	private Weapon primary;

	[SerializeField] private float speed;
	[SerializeField] private float jumpheight;
	[SerializeField] private float dashSpeed;

	[SerializeField] private float swimmingSpeed;
	[SerializeField] private float swimmingDashSpeed;

	private float currentSpeed;
	private float currentDashSpeed;

	private bool facingLeft = true;
	private bool grounded = true;

	private bool dashAvailable = true;
	public bool cantAttack = false;

	float dashCD = 0.75f;
	float lastDash = 0f;

	float timeBeforeNotSlowed = 1f;
	float lastSlowedTime = 0f;
	private bool slowed = false;

	public bool stopped = false;
	public bool talking = false;

	public bool dontFlip = false;

    Vector3 mousePos;
    Vector2 mousePos2D;
    RaycastHit2D hit;

	public bool swimming = false;

    // Start is called before the first frame update
    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
		col = GetComponent<Collider2D>();
		pah = GetComponent<Player_Animator_Handler>();
		ph = GetComponent<PlayerHealth>();
		primary = transform.Find("Weapon").GetChild(0).GetComponent<Weapon>();
    }

    // Update is called once per frame
    void Update()
    {
		if (!ph.dead) {
			movement();
		}
		attack();
		checkConditions();
		pah.beIdle();
    }

	private void movement() {
        if ((Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A)) && !stopped)
        {
			if (rb2D.velocity.x > 0) {
				if (rb2D.velocity.x > 50f) {
					rb2D.velocity = new Vector2(0, rb2D.velocity.y);
				}
				else {
					rb2D.velocity = new Vector2(-rb2D.velocity.x, rb2D.velocity.y);
				}
			}
			if (slowed) {
				transform.position += Vector3.left * (currentSpeed / 2) * Time.deltaTime;
			}
			else {
				transform.position += Vector3.left * currentSpeed * Time.deltaTime;
			}
			if (!pah.isPlaying(2) && !slowed && !pah.isPlaying(11) && !pah.isPlaying(6)) {
				pah.playAnim(1);
			}
			if (!facingLeft && !dontFlip) {
				flip();
			}
			if (Input.GetKeyDown(KeyCode.LeftShift) && dashAvailable)
			{
				dashAvailable = false;
				lastDash = Time.time;
				StartCoroutine(dash());
			}
		}
        else if ((Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D)) && !stopped)
        {	
			if (rb2D.velocity.x < 0) {
				if (rb2D.velocity.x < -50) {
					rb2D.velocity = new Vector2(0, rb2D.velocity.y);
				}
				else {
					rb2D.velocity = new Vector2(-rb2D.velocity.x, rb2D.velocity.y);
				}
			}
			if (slowed) {
				transform.position += Vector3.right * (currentSpeed / 2) * Time.deltaTime;
			}
			else {
				transform.position += Vector3.right* currentSpeed * Time.deltaTime;
			}
			if (!pah.isPlaying(2) && !slowed && !pah.isPlaying(11) && !pah.isPlaying(6)) {
				pah.playAnim(1);
			}
			if (facingLeft && !dontFlip) {
				flip();
			}
			if (Input.GetKeyDown(KeyCode.LeftShift) && dashAvailable)
			{
				dashAvailable = false;
				lastDash = Time.time;
				StartCoroutine(dash());
			}
        }
        if (Input.GetKeyDown(KeyCode.Space) && (isGrounded() || swimming) && !stopped)
        {
			if (!slowed) {
				pah.playAnim(2);
			}
			rb2D.velocity = Vector2.up * jumpheight;
        }
	}

	private void checkConditions() {
		if (col.IsTouchingLayers(ground)) {
			grounded = true;
		}
		else if (!col.IsTouchingLayers(ground)) {
			grounded = false;
		}
		if (col.IsTouchingLayers(water)) {
			swimming = true;
		}
		else if (!col.IsTouchingLayers(water)) {
			swimming = false;
		}
		if (swimming && (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))) {
			rb2D.gravityScale = 5f;
			rb2D.drag = 0f;
		}
		else if (swimming) {
			if (!col.IsTouchingLayers(ground)) {
				rb2D.drag = 1f;
			}
			else {
				rb2D.drag = 0f;
			}
			rb2D.gravityScale = 2.5f;
			currentSpeed = swimmingSpeed;
			currentDashSpeed = swimmingDashSpeed;
		}
		else if (!swimming) {
			rb2D.gravityScale = 5f;
			rb2D.drag = 0f;
			currentSpeed = speed;
			currentDashSpeed = dashSpeed;
		}
		if (slowed && (Time.time - lastSlowedTime) > timeBeforeNotSlowed) {
			slowed = false;
		}
		if (!dashAvailable && (Time.time - lastDash) > dashCD) {
			dashAvailable = true;
		}
	}

	public IEnumerator avoidAllHitBoxFor(float x) {
		PolygonCollider2D[] allEmemies = FindObjectsOfType<PolygonCollider2D>();
		for (int i = 0; i < allEmemies.Length; i++) {
			Physics2D.IgnoreCollision(allEmemies[i], col, true);
		}
		yield return new WaitForSeconds(x);
		allEmemies = FindObjectsOfType<PolygonCollider2D>();
		for (int i = 0; i < allEmemies.Length; i++) {
			try {
				if (!allEmemies[i].GetComponent<Monster>().getDead()) {
					Physics2D.IgnoreCollision(allEmemies[i], col, false); 
				}
			}
			catch {
				Physics2D.IgnoreCollision(allEmemies[i], col, false);
			}
		}
	}

	public IEnumerator dash() {
		StartCoroutine(avoidAllHitBoxFor(0.5f));
		rb2D.gravityScale = 0;
		if (!facingLeft) {
			rb2D.velocity = Vector2.right * currentDashSpeed;
		}
		else {
			rb2D.velocity = Vector2.left * currentDashSpeed;
		}
		yield return new WaitForSeconds(0.1f);
		rb2D.gravityScale = 5;
	}

	public void attack() {
		if (!cantAttack && !ph.hurt) {
			primary.attack();
		}
	}

	public void resetConditions() {
		primary.resetConditions();
	}

	public void slowFor(float t) {
		slowed = true;
		lastSlowedTime = Time.time;
		timeBeforeNotSlowed = t;
	}

	public void flip() {
		facingLeft = !facingLeft;
		transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
	}

	public bool getGrounded() {
		return grounded;
	}

	public bool getFacingLeft() {
		return facingLeft;
	}

	public bool getSlowed() {
		return slowed;
	}

	public bool getStopped() {
		return stopped;
	}

	public float getSpeed() {
		return speed;
	}

	public float getJumpHeight() {
		return jumpheight;
	}

	public float getDashSpeed() {
		return dashSpeed;
	}

	public void switchSlowed() {
		slowed = !slowed;
	}

	public void switchStopped() {
		stopped = !stopped;
	}

	public void stoppedOn() {
		stopped = true;
	}

	public void stoppedOff() {
		stopped = false;
	}

	public void setSpeed(float x) {
		speed = x;
	}

	public void setJumpHeight(float x) {
		jumpheight = x;
	}

	public void setDashSpeed(float x) {
		dashSpeed = x;
	}

	public void stopAttack() {
		cantAttack = true;
	}

	public void startAttack() {
		cantAttack = false;
	}

	public void changePrimaryWeapon(Weapon newWeapon) {
		primary = newWeapon;
		newWeapon.setUp();
	}

	public bool isGrounded() {
		return (col.IsTouchingLayers(ground) || col.IsTouchingLayers(monster));
	}

	public bool getAttacking() {
		return primary.getAttacking();
	}

	public bool getCharging() {
		return primary.getCharging();
	}
}
