using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Mel : NPC
{
    public QuestBoard questBoard;

    // Start is called before the first frame update
    void Start()
    {
        setUp();
        dialogue.function.onClick.AddListener(openQuestBoard);
        dialogue.function.transform.GetChild(0).GetComponent<Text>().text = "Quest Board";
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
        if (questBoard.questBoardOn && Input.GetKeyDown(KeyCode.Escape)) {
            questBoard.toggleQuestBoard();
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

    public void openQuestBoard() {
        dialogue.toggleDialogue();
        questBoard.toggleQuestBoard();
    }
}
