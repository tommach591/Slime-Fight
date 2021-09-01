using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChaChing : MonoBehaviour
{
    private float lifeTime = 2f;
    private float speed = 50f;

    private Vector3 offset;

    // Update is called once per frame
    void Update()
    {
        this.transform.position = new Vector3(transform.parent.Find("Coin").position.x, transform.parent.Find("Coin").position.y - speed, transform.parent.Find("Coin").position.z) + offset;
        if (speed >= 0f) {
            speed -= (speed / 100);
        }
    }

    public void setUp(int x, Vector3 o) {
        this.GetComponent<Text>().text = "+" + x.ToString();
        offset = o;
        Invoke("DestroyMe", lifeTime);
    }

    public void DestroyMe()
    {
        Destroy(gameObject);
    }
}
