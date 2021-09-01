using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarHammer : Hammer
{
    // Start is called before the first frame update
    void Start()
    {
        setUp();
    }

    // Update is called once per frame
    void Update()
    {

    }

	public override void attack() {
		base.attack();
	}

}
