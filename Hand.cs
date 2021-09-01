using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : Projectile
{
    public LineRenderer lr;
    public DistanceJoint2D dj;

    public GameObject attached;
    public GameObject hit;
    public bool dragPlayer = false;
    public bool hitTarget = false;

    void Start()
    {
        setUp();
        lr = GetComponent<LineRenderer>();
        dj = GetComponent<DistanceJoint2D>();
        dj.enabled = false;
    }

    new protected void Update()
    {
        if (!hitTarget) {
            fire();
        }
        dragging();
    }

	new protected void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.tag == "Hunter") {
            hitTarget = true;
            hit = other.gameObject;
        }
	}

    public void dragging() {
        if (dragPlayer) {
            lr.SetPosition(0, attached.transform.position);
            if (hitTarget) {
                lr.SetPosition(1, hit.transform.position);
            }
            else {
                lr.SetPosition(1, transform.position);
            }
            dj.connectedAnchor = attached.transform.position;
            lr.enabled = true;
            dj.enabled = true;
        }
        else {
            dj.enabled = false;
            lr.enabled = false;
        }
        if (dj.enabled) {
            lr.SetPosition(1, transform.position);
        }
    }

}
