using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Platform : MonoBehaviour
{
    private PlatformEffector2D pe2d;
    private CompositeCollider2D cc2d;
    private BoxCollider2D player;
    private TilemapCollider2D col;
    public float waitTime;
    public float switchTime;
    public bool ignoreOn;
    public int pressed;

    // Start is called before the first frame update
    void Start()
    {
        pe2d = GetComponent<PlatformEffector2D>();
        cc2d = GetComponent<CompositeCollider2D>();
        player = FindObjectOfType<PlayerController>().gameObject.GetComponent<BoxCollider2D>();
        col = GetComponent<TilemapCollider2D>();
        ignoreOn = false;
        pressed = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (waitTime <= 0) {
            pressed = 0;
        }
        else {
            waitTime -= Time.deltaTime;
        }
        if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S)) {
            if (pressed >= 1) {
                Physics2D.IgnoreCollision(col, player, true);
                Physics2D.IgnoreCollision(cc2d, player, true);
                ignoreOn = true;
                pressed = 0;
            }
            else {
                pressed++;
                waitTime = 0.4f;
            }
        }

        if (!ignoreOn) {
            switchTime = 0.5f;
        }
        if (ignoreOn) {
            if (switchTime <= 0) {
                Physics2D.IgnoreCollision(col, player, false);
                Physics2D.IgnoreCollision(cc2d, player, false);
                ignoreOn = false;
                switchTime = 0.5f;
            }
            else {
                switchTime -= Time.deltaTime;
            }
        }
    }
}
