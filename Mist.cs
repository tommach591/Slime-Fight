using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mist : Projectile
{

    public ApexJolly apex;
    public PlayerController player;
    protected Rigidbody2D playerRb2D;
    protected DistanceJoint2D dj;
    protected LineRenderer lr;
    protected Vector2 pull;
    protected bool returningBack;
    [SerializeField] Sprite ball;

    protected float maxDistance;
    protected float currentDistance;

    private bool facingLeft;

    void Start()
    {
        setUp();
        dj = GetComponent<DistanceJoint2D>();
        lr = GetComponent<LineRenderer>();
        lr.SetPosition(0, apex.transform.position);
        lr.SetPosition(1, transform.position);
        dj.connectedAnchor = apex.transform.position;
        dj.enabled = true;
        lr.enabled = true;
        returningBack = false;
        maxDistance = dj.distance;
    }

    new protected void Update()
    {
        if (returningBack) {
            setDir(apex.transform.position - transform.position);
        }
        if (!apex.hitPlayer || returningBack) {
            lookAtDirection();
            fire();
        }
        if (apex.hitPlayer && !returningBack) {
            transform.position = player.transform.position;
            player.slowFor(0.1f);
            pull = apex.transform.position - player.transform.position;
            playerRb2D.AddForce(pull, ForceMode2D.Impulse);
            currentDistance = Vector2.Distance(player.transform.position, apex.transform.position);
            if (player.GetComponent<PlayerHealth>().hurt || player.GetComponent<PlayerHealth>().dead) {
                DestroyProjectile();
            }
        }
        attached();
    }

	new protected void setUp() {
		Invoke("returnBack", lifeTime);
		col = GetComponent<Collider2D>();
		rb2D = GetComponent<Rigidbody2D>();
		sr = GetComponent<SpriteRenderer>();
	}

	new protected void OnTriggerEnter2D(Collider2D other) {
		if (other.gameObject.tag == "Hunter" && !returningBack) {
            player = other.gameObject.GetComponent<PlayerController>();
            apex.hitPlayer = true;
            gameObject.transform.SetParent(other.gameObject.transform);
            playerRb2D = player.GetComponent<Rigidbody2D>();
            dj.distance = 150f;
            maxDistance = dj.distance;
            sr.sprite = ball;
		}
        if (other.gameObject.tag == "Ground" && !returningBack) {
            returnBack();
        }
        if ((other.gameObject.tag == "Monster" && returningBack) || apex == null) {
            dj.enabled = false;
            lr.enabled = false;
            DestroyProjectile();
        }
	}

    protected void attached() {
        lr.SetPosition(0, apex.transform.position);
        lr.SetPosition(1, transform.position);
        dj.connectedAnchor = apex.transform.position;
        currentDistance = Vector2.Distance(apex.transform.position, transform.position);
        if (currentDistance >= maxDistance) {
            returnBack();
            returningBack = true;
        }
    }

    protected void returnBack() {
        gameObject.transform.SetParent(apex.transform);
        returningBack = true;
    }

	public void lookAtDirection() {
        if (spawnPosition.x > transform.position.x && facingLeft) {
			flip();
		}
		else if (spawnPosition.x < transform.position.x && !facingLeft) {
			flip();
		}
	}

	public void flip() {
		facingLeft = !facingLeft;
		transform.localScale = new Vector3(transform.localScale.x, -transform.localScale.y, transform.localScale.z);
	}

}
