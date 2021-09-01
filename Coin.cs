using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    public int value;
    private Rigidbody2D rb2D;

    void Awake() {
        rb2D = GetComponent<Rigidbody2D>();
        boing();
    }

    void OnCollisionEnter2D(Collision2D collision) {
		if (collision.gameObject.tag == "Hunter") {
            collision.gameObject.GetComponent<Player_Stats>().coins += value;
            Destroy(gameObject);
        }
	}

    public void boing() {
        rb2D.AddForce(new Vector2(0f, 1f) * rb2D.mass * 1500f);
    }
}
