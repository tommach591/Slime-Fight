using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cloud : MonoBehaviour
{

    [SerializeField] float x_despawn;
    [SerializeField] float x_spawn;
    [SerializeField] Vector3 speed;

    private Vector3 respawn;

    // Start is called before the first frame update
    void Start()
    {
        respawn = new Vector3(x_spawn, transform.position.y, transform.position.z);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += speed;
        if (transform.position.x <= x_despawn) {
            transform.position = respawn;
        }
    }
}
