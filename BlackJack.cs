using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackJack : Tome
{
	override public IEnumerator attackComboOne() {
		attacking = true;

        pc.slowFor(0.25f);
		pah.playAnim(5);
        castMagic(0.75f);
		yield return new WaitForSeconds(0.25f);
        if (!pc.isGrounded()) {
            pah.playAnim(2);
        }
        else {
		    pah.playAnim(0);
        }
		yield return new WaitForSeconds(delayBtwAttacks);
		attacking = false;
	}

	override public IEnumerator attackComboTwo() {
		attacking = true;

        pc.slowFor(0.25f);
		pah.playAnim(5);
		yield return new WaitForSeconds(0.25f);
        castMagic(0.75f);
        castMagic(0.75f);
        if (!pc.isGrounded()) {
            pah.playAnim(2);
        }
        else {
		    pah.playAnim(0);
        }
		yield return new WaitForSeconds(delayBtwAttacks);
		attacking = false;
	}

	override public IEnumerator attackComboThree() {
		attacking = true;
		cooldown = true;

        pc.slowFor(0.25f);
		pah.playAnim(5);
		yield return new WaitForSeconds(0.25f);
        castMagic(0.75f);
        castMagic(0.75f);
        castMagic(0.75f);
        if (!pc.isGrounded()) {
            pah.playAnim(2);
        }
        else {
		    pah.playAnim(0);
        }
		yield return new WaitForSeconds(delayBtwAttacks);
		attacking = false;
	}
}
