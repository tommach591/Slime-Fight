using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Lailah : NPC
{
    public Inventory inventory;

    // Start is called before the first frame update
    void Start()
    {
        setUp();
        dialogue.function.onClick.AddListener(openInventory);
        dialogue.function.transform.GetChild(0).GetComponent<Text>().text = "Inventory";
        dialogue.chat.onClick.AddListener(pickRandomQuote);
    }

    // Update is called once per frame
    void Update()
    {
        chatting();
        closeMenu();
    }

    private void closeMenu() {
        if (dialogue.dialogueOn && Input.GetKeyDown(KeyCode.Escape) && inventory.doneTransition) {
            dialogue.toggleDialogue();
            player.talking = false;
        }
        if (inventory.inventoryOn && Input.GetKeyDown(KeyCode.Escape) && inventory.doneTransition) {
            inventory.toggleMenu();
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

    public void openInventory() {
        dialogue.toggleDialogue();
        inventory.toggleMenu();
    }

}
