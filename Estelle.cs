using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Estelle : NPC
{
    public StatShop statShop;

    // Start is called before the first frame update
    void Start()
    {
        setUp();
        dialogue.function.onClick.AddListener(openStatShop);
        dialogue.function.transform.GetChild(0).GetComponent<Text>().text = "Stats";
        dialogue.chat.onClick.AddListener(pickRandomQuote);
    }

    // Update is called once per frame
    void Update()
    {
        chatting();
        closeMenu();
    }

    private void closeMenu() {
        if (dialogue.dialogueOn && Input.GetKeyDown(KeyCode.Escape)) {
            dialogue.toggleDialogue();
            player.talking = false;
        } 
        if (statShop.statShopOn && Input.GetKeyDown(KeyCode.Escape)) {
            statShop.toggleStatShop();
            player.talking = false;
        }
    }

    public override void talk() {
        if (!dialogue.dialogueOn && !player.talking) {
            player.talking = true;
            pickRandomQuote();
            dialogue.toggleDialogue();
        }
    }

    public void openStatShop() {
        dialogue.toggleDialogue();
        statShop.toggleStatShop();
    }
}
