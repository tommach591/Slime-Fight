using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stardust : Magic
{
    [SerializeField] protected Sprite[] prism;
    [SerializeField] protected Sprite[] stardust;
    protected int pickOne;

    void Start()
    {
        setUp();
    }

	new protected void setUp() {
		StartCoroutine(fadeAway(lifeTime));
        sr = GetComponent<SpriteRenderer>();
		col = GetComponent<Collider2D>();
		rb2D = GetComponent<Rigidbody2D>();
        tmp = sr.color;
        pickOne = Random.Range(0, prism.Length);
        sr.sprite = prism[pickOne];
        srEfx[0].sprite = stardust[pickOne];
	}
    
}
