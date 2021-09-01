using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartPistol : Gun
{
    // Start is called before the first frame update
    void Start()
    {
        setUp();
        spawn = transform.Find("Spawn").gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	public override void attack() {
		base.attack();
	}
}
