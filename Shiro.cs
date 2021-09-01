using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shiro : NPC
{
    [SerializeField] Story story;
    [SerializeField] PlayerStory playerStory;

    // Start is called before the first frame update
    void Start()
    {
        setUp();
        playerStory = player.GetComponent<PlayerStory>();
        dialogue.function.onClick.AddListener(openStory);
        dialogue.function.transform.GetChild(0).GetComponent<Text>().text = "Story";
        dialogue.chat.onClick.AddListener(pickRandomQuote);
        story.next.onClick.AddListener(story.nextQuote);
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
        if (story.storyOn && Input.GetKeyDown(KeyCode.Escape)) {
            story.toggleStory();
            player.talking = false;
        }
    }

    public void finishedStory() {
        story.toggleStory();
        player.talking = false;
    }

    public override void talk() {
        if (!dialogue.dialogueOn && !player.talking) {
            player.talking = true;
            pickRandomQuote();
            dialogue.toggleDialogue();
        }
    }

    public void openStory() {
        dialogue.toggleDialogue();
        story.toggleStory();
        if (playerStory.isCompleted()) {
            playerStory.nextStory();
            story.startChapter();
        }
        else if (!playerStory.isAccepted()) {
            story.startChapter();
        }
    }
    
    public void acceptMission() {
        playerStory.acceptedMission();
    }

    public void completeMission() {
        playerStory.completedMission();
    }

    public void nextChapter() {
        playerStory.nextStory();
    } 

    public void nextTarget() {
        playerStory.target = story.getTarget();
    }

    public PlayerStory getPlayerStory() {
        return playerStory;
    }
}
