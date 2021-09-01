using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Stats : MonoBehaviour
{
	[SerializeField] int maxHealth;
	[SerializeField] int attack;
	[SerializeField] int defense;
	[SerializeField] float critChance;
	[SerializeField] float critDamage;
	public int coins;

	public bool guardOn = false;
	public bool perfectGuard = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	public int getMaxHealth() {
		return maxHealth;
	}

	public void setMaxHealth(int x) {
		maxHealth = x;
	}

	public int getAttack() {
		return attack;
	}

	public void setAttack(int x) {
		attack = x;
	}

	public int getDefense() {
		return defense;
	}

	public void setDefense(int x) {
		defense = x;
	}

	public float getCritChance() {
		return critChance;
	}

	public void setCritChance(float x) {
		critChance = x;
	}

	public float getCritDamage() {
		return critDamage;
	}

	public void setCritDamage(float x) {
		critDamage = x;
	}

	public void incMaxHealth() {
		maxHealth++;
		GetComponent<PlayerHealth>().heal();
	}

	public void incAttack() {
		attack++;
	}

	public void incDefense() {
		defense++;
	}

	public void incCritChance() {
		critChance += 0.5f;
	}

	public void incCritDamage() {
		critDamage += 0.05f;
	}

	public void guardUp() {
		if (!guardOn) {
			guardOn = true;
		}
	}

	public void guardDown() {
		if (guardOn) {
			guardOn = false;
		}
	}

	public void perfectGuardUp() {
		if (!perfectGuard) {
			StartCoroutine(perfectGuardFrame());
		}
	}

	public void perfectGuardDown() {
		if (perfectGuard) {
			perfectGuard = false;
		}
	}

	private IEnumerator perfectGuardFrame() {
		perfectGuard = true;
		yield return new WaitForSeconds(0.5f);
		perfectGuard = false;
	}
}
