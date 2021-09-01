using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Mission : MonoBehaviour
{
    [SerializeField] Player_Mission player;
    [SerializeField] Monster monster;
    [SerializeField] Text monsterName;
    [SerializeField] Text locationName;
    [SerializeField] string destination;
    [SerializeField] Image icon;
    private SpriteRenderer monsterImage;
    private Button button;

    Dictionary<string, string> maps = new Dictionary<string, string>() {
        {"TownyTown", "Towny Town"},
        {"GrassyGrass", "Grassy Grass"},
        {"WateryWater", "Watery Water"},
        {"SandySand", "Sandy Sand"}
    };

    // Start is called before the first frame update
    void Start()
    {
        try {
            player = FindObjectOfType<Player_Mission>();
            monsterImage = monster.transform.GetComponent<SpriteRenderer>();
            icon.sprite = monsterImage.sprite;
            monsterName.text = monster.name;
            locationName.text = maps[destination];
            button = GetComponent<Button>();
            button.onClick.AddListener(delegate { player.setMission(monster, destination); });
        }
        catch {
            gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void changeMap(string s) {
        SceneManager.LoadScene(s);
    }
}
