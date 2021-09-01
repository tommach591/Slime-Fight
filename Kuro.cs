using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Kuro : NPC
{
    protected Player_Unlocks pu;

    void Start()
    {
        setUp();
        pu = player.GetComponent<Player_Unlocks>();
        if (!pu.rescuedKuro) {
            gameObject.SetActive(false);
        }
        //dialogue.function.onClick.AddListener(openStory);
        dialogue.function.transform.GetChild(0).GetComponent<Text>().text = "Shop";
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
        /*
        if (story.storyOn && Input.GetKeyDown(KeyCode.Escape)) {
            story.toggleStory();
            player.talking = false;
        }
        */
    }

    public override void talk() {
        if (!dialogue.dialogueOn && !player.talking) {
            player.talking = true;
            pickRandomQuote();
            dialogue.toggleDialogue();
        }
    }
}
