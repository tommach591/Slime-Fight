using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    [SerializeField] public PlayerController player;
    protected Player_Stats ps;
    [SerializeField] protected string[] quotes;
    public Dialogue dialogue;
    private int currentQuote;
    private int lastQuote;

    protected void setUp() {
        player = FindObjectOfType<PlayerController>();
        ps = player.GetComponent<Player_Stats>();
        Physics2D.IgnoreCollision(player.GetComponent<BoxCollider2D>(), GetComponent<BoxCollider2D>());
    }

    public void OnMouseOver()
    {   
        if (Input.GetKeyDown(KeyCode.Mouse0)) {
            talk();
        }
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Hunter" && (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W)))
        {
            talk();
        }
    }

    public virtual void talk() {

    }

	protected void chatting() {
		if (player.talking) {
			player.stopped = true;
			player.cantAttack = true;
		}
		else if (!player.getAttacking() && !ps.guardOn) {
			player.stopped = false;
			player.cantAttack = false;
		}
	}

    protected void pickRandomQuote() {
        currentQuote = (int)Random.Range(0f, quotes.Length);
        while (currentQuote == lastQuote && quotes.Length != 1) {
            currentQuote = (int)Random.Range(0f, quotes.Length);
        }
        dialogue.setQuote(quotes[currentQuote]);
        lastQuote = currentQuote;
    }

}
