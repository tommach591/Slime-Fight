using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

public class Weapon : Item
{

	protected GameObject player;
	protected PlayerController pc;
	protected Player_Stats ps;
	protected PlayerHealth ph;
	protected Player_Animator_Handler pah;
	protected Collider2D col;
	protected Rigidbody2D rb2D;

	protected bool attacking = false;
	[SerializeField] protected float delayBtwAttacks;
	public bool chargeInProg = false;
	protected int chargeMultiplier = 1;

	[SerializeField] protected int weaponAttack = 0;
	[SerializeField] protected float damageMultiplier = 1;
	[SerializeField] protected int defenseUp = 0;
	[SerializeField] protected float critRateUp = 0f;
	[SerializeField] protected float critDamageUp = 0f;
	[SerializeField] protected int knockback = 0;

    protected Dictionary<int, float> knockbackLevel = new Dictionary<int, float>() {
        {0, 0f},
        {1, 25f},
		{2, 50f},
		{3, 100f},
		{4, 200f},
		{5, 400f}
    };

    void Start()
    {
		
    }

	public void setUp() {
		try {
			player = transform.parent.parent.gameObject;
			pc = player.GetComponent<PlayerController>();
			ps = player.GetComponent<Player_Stats>();
			ph = player.GetComponent<PlayerHealth>();
			pah = player.GetComponent<Player_Animator_Handler>();
			rb2D = player.GetComponent<Rigidbody2D>();
			col = GetComponent<Collider2D>();
			col.enabled = false;
		}
		catch {
		
		}
	}

	public int damageDealt() {
		return (int)((weaponAttack + (ps.getAttack() / 2)) * damageMultiplier);
	}

	public int critDamageDealt() {
		return (int)((weaponAttack + (ps.getAttack() / 2)) * damageMultiplier * (ps.getCritDamage() + critDamageUp));
	}

	public bool isCrit() {
		if (Random.Range(0f, 100f) <= (ps.getCritChance() + critRateUp)) {
			return true;
		}
		else {
			return false;
		}
	}

	public bool getAttacking() {
		return attacking;
	}

	public bool getCharging() {
		return chargeInProg;
	}

	public virtual void attack() {
		
	}

	public virtual void resetConditions() {
		
	}
}
