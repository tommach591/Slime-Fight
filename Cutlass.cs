using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cutlass : Sword
{
    // Start is called before the first frame update
    void Start()
    {
        setUp();
		comboDelay = 0.5f;
    }

    // Update is called once per frame
    void Update()
    {
      resetCombo();
      if (base.cooldown) {
        StartCoroutine(attackCooldown());
      }
    }

	public override void attack() {
		if (!ph.dead) {
			if ((Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S)) && !cooldown) {
				ps.perfectGuardUp();
			}
			if ((Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S)) && !cooldown) {
				pc.stoppedOn();
				ps.guardUp();
				pah.playAnim(10);
			}
			else if ((Input.GetKeyUp(KeyCode.DownArrow) || Input.GetKeyUp(KeyCode.S)) && !cooldown) {
				pc.stoppedOff();
				ps.guardDown();
				ps.perfectGuardDown();
				pah.playAnim(0);
				cooldown = true;
			}
			else if ((Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.Mouse0))) {
				pc.slowFor(0.25f);

				if (lastCombo != 0f && (Time.time - lastCombo) < comboDelay && !cooldown && !attacking) {
					if (combo == 1) {
						StartCoroutine(attackComboTwo());
						combo = 0;
						lastCombo = 0f;
					}
				}
				else if (!cooldown && !attacking) {
					StartCoroutine(attackComboThree());
					combo = 1;
					lastCombo = Time.time;
				}
			}
		}
	}
}
