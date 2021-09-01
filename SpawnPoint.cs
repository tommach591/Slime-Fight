using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{

    private PlayerController player;
    private CameraController cam;
    private Monster[] monsters;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerController>();
        player.transform.position = new Vector3(transform.position.x, transform.position.y, player.transform.position.z);

        cam = FindObjectOfType<CameraController>();
        cam.player = player.gameObject;

        monsters = FindObjectsOfType<Monster>();
        for (int i = 0; i < monsters.Length; i++) {
            monsters[i].player = player.gameObject;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
