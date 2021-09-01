using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dummy : Monster
{
    protected bool checkingDamage = false;
    protected int currentHealth;

    void Awake()
    {
        setUp();
    }

    void Update()
    {
        checkConditions();
        Physics2D.IgnoreCollision(player.GetComponent<BoxCollider2D>(), col);
        if (!checkingDamage) {
            StartCoroutine(checkIfHeal());
        }
        if (health <= 100000) {
            heal();
        }
    }

    protected IEnumerator checkIfHeal() {
        checkingDamage = true;
        currentHealth = health;
        yield return new WaitForSeconds(10f);
        if (currentHealth == health) {
            heal();
        }
        checkingDamage = false;
    }

    protected void heal() {
		health = maxHealth;
		healthBar.setHealthBar(health, maxHealth);
    }
}
