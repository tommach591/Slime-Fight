using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Animator_Handler : MonoBehaviour
{

	private Animator anim;
	private PlayerController pc;
	private PlayerHealth ph;
	private Player_Stats ps;
	[SerializeField] private string[] state;

    // Start is called before the first frame update
    void Start()
    {
		anim = GetComponent<Animator>();
		pc = GetComponent<PlayerController>();
		ph = GetComponent<PlayerHealth>();
		ps = GetComponent<Player_Stats>();
    }

	public bool isPlaying(int i) {
		if (anim.GetCurrentAnimatorStateInfo(0).IsName(state[i])) {
			return true;
		}
		else {
			return false;
		}
	}

	public void playAnim(int i) {
		if (!anim.GetCurrentAnimatorStateInfo(0).IsName(state[i])) {
			anim.Play(state[i], 0, 0);
		}
	}

	public void beIdle() {
		StartCoroutine(resetAnimation());
	}

	public IEnumerator resetAnimation() {
		if (ph.dead) {
			playAnim(12);
		}
		if (isPlaying(12) && !ph.dead) {
			playAnim(0);
		}
		if (ph.hurt && !pc.getCharging() && !isPlaying(12) && !ph.dead) {
			playAnim(11);
		}
		if (!ph.hurt && !ph.dead && isPlaying(11) && !isPlaying(12)) {
			playAnim(0);
		}
		if (!pc.isGrounded() && !pc.getCharging() && !pc.getAttacking() && !ps.guardOn && !ph.hurt && !ph.dead) {
			playAnim(2);
		}
		if (isPlaying(1) && (!Input.GetKey(KeyCode.LeftArrow) && !Input.GetKey(KeyCode.RightArrow) && !Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D) )) {
			playAnim(0);
		}
		if (isPlaying(2)) {
			yield return new WaitForSeconds(0.05f);
			if (pc.getGrounded() && !pc.getAttacking()) {
				playAnim(0);
			}
		}
	}
}
