using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RainbowWing : Claw
{
	override public IEnumerator upAttack() {
		attacking = true;
        upAttackOnCooldown = true;
		col.enabled = true;
        StartCoroutine(pc.avoidAllHitBoxFor(1.5f));

		pah.playAnim(4);
        rb2D.velocity = Vector2.zero;
		if (pc.getFacingLeft()) {
            target = new Vector2(-1f, 4f) * rb2D.mass * force;
		}
		else {
            target = new Vector2(1f, 4f) * rb2D.mass * force;
		}
        rb2D.AddForce(target, ForceMode2D.Impulse);
        pc.slowFor(0.27f);
        castMagic(0.25f);
        yield return new WaitForSeconds(0.05f);
        castMagic(0.25f);

		yield return new WaitForSeconds(0.27f);
		pah.playAnim(2);
		yield return new WaitForSeconds(delayBtwAttacks);
		attacking = false;
		col.enabled = false;

        yield return new WaitForSeconds(upAttackCooldown);
        upAttackOnCooldown = false;
	}

	override public IEnumerator downAttack() {
		attacking = true;
        downAttackOnCooldown = true;
		col.enabled = true;
        StartCoroutine(pc.avoidAllHitBoxFor(1.5f));

		pah.playAnim(5);
		if (pc.getFacingLeft()) {
            target = new Vector2(-5f, -5f) * rb2D.mass * force;
		}
		else {
            target = new Vector2(5f, -5f) * rb2D.mass * force;
		}
        rb2D.AddForce(target, ForceMode2D.Impulse);
        pc.slowFor(0.25f);
        castMagic(0.25f);
        yield return new WaitForSeconds(0.05f);
        castMagic(0.25f);

		yield return new WaitForSeconds(0.25f);
		pah.playAnim(2);
		yield return new WaitForSeconds(delayBtwAttacks);
		attacking = false;
		col.enabled = false;

        yield return new WaitForSeconds(downAttackCooldown);
        downAttackOnCooldown = false;
	}

	override public IEnumerator forwardAttack() {
		attacking = true;
		col.enabled = true;

		pah.playAnim(3);
		if (pc.getFacingLeft()) {
            target = new Vector2(-1.2f, 0f) * rb2D.mass * force;
		}
		else {
            target = new Vector2(1.2f, 0f) * rb2D.mass * force;
		}
        rb2D.AddForce(target, ForceMode2D.Impulse);
        pc.slowFor(0.27f);
		yield return new WaitForSeconds(0.20f);
        castMagic(0.01f);
        yield return new WaitForSeconds(0.05f);
        castMagic(0.01f);
		yield return new WaitForSeconds(0.02f);
		pah.playAnim(0);
		yield return new WaitForSeconds(delayBtwAttacks);
		attacking = false;
		col.enabled = false;
	}

	override public IEnumerator stabAttack() {
		attacking = true;
		col.enabled = true;
		pah.playAnim(3);
		if (pc.getFacingLeft()) {
            target = new Vector2(-2f, 0f) * rb2D.mass * force;
		}
		else {
            target = new Vector2(2f, 0f) * rb2D.mass * force;
		}
        pc.slowFor(0.27f);
		yield return new WaitForSeconds(0.20f);
        castMagic(0.01f);
        yield return new WaitForSeconds(0.05f);
        castMagic(0.01f);
		yield return new WaitForSeconds(0.02f);
		pah.playAnim(0);
		yield return new WaitForSeconds(delayBtwAttacks);
		attacking = false;
		col.enabled = false;
	}
}
