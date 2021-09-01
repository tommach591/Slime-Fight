using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MapSetup : MonoBehaviour
{

    private PlayerController player;
    private Player_Equips pe;
    private PlayerStory playerStory;
    private Player_Mission playerMission;
    private CameraController cam;
    private Monster monster;
    private Monster copy;

    private NPC[] npcs;

    [SerializeField] GameObject playerSpawn;
    [SerializeField] GameObject bossSpawn;

    private bool hunting;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerController>();
        player.transform.position = new Vector3(playerSpawn.transform.position.x, playerSpawn.transform.position.y, player.transform.position.z);

        playerStory = player.GetComponent<PlayerStory>();
        playerMission = player.GetComponent<Player_Mission>();

        cam = FindObjectOfType<CameraController>();
        cam.player = player.gameObject;

        try {
            npcs = FindObjectsOfType<NPC>();
            for (int i = 0; i < npcs.Length; i++) {
                npcs[i].player = player;
            }
            
        } catch {}

        try {
            monster = playerMission.target;
            copy = Instantiate(monster, new Vector3(bossSpawn.transform.position.x, bossSpawn.transform.position.y, player.transform.position.z), Quaternion.identity).GetComponent<Monster>();
            copy.player = player.gameObject;
            hunting = true;
            if (player.talking) {
                player.talking = false;
                player.stoppedOff();
                player.cantAttack = false;
            }
        }
        catch { hunting = false; }
    }

    // Update is called once per frame
    void Update()
    {
        if (copy == null && hunting) {
            copy = FindObjectOfType<Monster>();
            if (copy == null) {
                Invoke("targetSlayed", 5f);
            }
        }
    }

    void targetSlayed() {
        if (playerStory.target != null && playerStory.target.monsterType == playerMission.target.monsterType) {
            playerStory.completedMission();
        }
        player.GetComponent<PlayerHealth>().heal();
        playerMission.resetMission();
        SceneManager.LoadScene("TownyTown");
    }
}
