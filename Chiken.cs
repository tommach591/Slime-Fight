using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chiken : Dummy
{
    protected Animator anim;
    protected NPC[] npcs;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        npcs = FindObjectsOfType<NPC>();
        for (int i = 0; i < npcs.Length; i++) {
            Physics2D.IgnoreCollision(npcs[i].GetComponent<BoxCollider2D>(), col);
        }
        immune = true;
    }

    // Update is called once per frame
    void Update()
    {
        lookAtPlayer();
        checkConditions();
        Physics2D.IgnoreCollision(player.GetComponent<BoxCollider2D>(), col);
        if (!checkingDamage) {
            StartCoroutine(checkIfHeal());
        }
        if (health <= 100000) {
            heal();
        }
        if (grounded && !anim.GetCurrentAnimatorStateInfo(0).IsName("chiken_idle")) {
            anim.Play("chiken_idle", 0, 0);
        }
        else if (!grounded && !anim.GetCurrentAnimatorStateInfo(0).IsName("chiken_air")) {
            anim.Play("chiken_air", 0, 0);
        }
    }
}
