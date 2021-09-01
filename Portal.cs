using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour
{
    [SerializeField] Player_Mission player;

    Dictionary<string, int> maps = new Dictionary<string, int>() {
        {"TownyTown", 0},
        {"GrassyGrass", 1},
        {"WateryWater", 2},
        {"SandySand", 3}
    };

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<Player_Mission>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerStay2D(Collider2D other)
    {
        if ((Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W)) && maps.ContainsKey(player.destination)) {
            player.GetComponent<PlayerController>().talking = false;
            changeMap(player.destination);
        }
    }

    void changeMap(string s) {
        SceneManager.LoadScene(s);
    }
}
