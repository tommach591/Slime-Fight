using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CurrentMission : MonoBehaviour
{

    [SerializeField] Player_Mission player;
    [SerializeField] Text monsterName;
    [SerializeField] Text locationName;
    [SerializeField] Image icon;
    private SpriteRenderer monsterImage;

    private MonsterLore lore;
    [SerializeField] Text story;
    [SerializeField] Text prime;
    [SerializeField] Text crit;

    Dictionary<string, string> maps = new Dictionary<string, string>() {
        {"None", "None"},
        {"TownyTown", "Towny Town"},
        {"GrassyGrass", "Grassy Grass"},
        {"WateryWater", "Watery Water"},
        {"SandySand", "Sandy Sand"}
    };

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<Player_Mission>();
        lore = GetComponent<MonsterLore>();
    }

    // Update is called once per frame
    void Update()
    {
        updateCurrent();
    }

    public void updateCurrent() {
        monsterName.text = player.target.name;
        locationName.text = maps[player.destination];
        monsterImage = player.target.transform.GetComponent<SpriteRenderer>();
        icon.sprite = monsterImage.sprite;
        story.text = lore.story[player.target.name];
        prime.text = lore.prime[player.target.name];
        crit.text = lore.crit[player.target.name];
    }
}
