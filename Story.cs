using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Story : MonoBehaviour
{
    [SerializeField] public Sprite[] expressions;
    private string[] currentChapter;
    private string[] currentChapterExpressions;

    private Shiro shiro;
    private PlayerStory playerStory;
    [SerializeField] private Image portrait;
    [SerializeField] private Text quote;
    [SerializeField] public Button next;

    public Dictionary<string, int> shiroExpression = new Dictionary<string, int>() {
        {"Idle", 0},
        {"Smile", 1},
        {"Worried", 2},
        {"Angry", 3}
    };

    public string[][] chapter = 
    {
        chapterZero, 
        chapterOne,
        chapterTwo
    };

    public string[][] chapterExpressions = 
    {
        chapterZeroExpressions, 
        chapterOneExpressions,
        chapterTwoExpressions
    };

    [SerializeField] public Monster[] chapterTargets;

    [SerializeField] GameObject storyDialogue;
    public bool storyOn;

    // Start is called before the first frame update
    void Start()
    {
        if (storyDialogue.activeSelf) {
            storyDialogue.SetActive(false);
        }

        storyOn = false;
        shiro = transform.parent.parent.GetComponent<Shiro>();
        playerStory = shiro.getPlayerStory();

        currentChapter = chapter[playerStory.currentStory];
        currentChapterExpressions = chapterExpressions[playerStory.currentStory];
        updateDialgoue();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void toggleStory() {
        if (!storyOn) {
            storyDialogue.SetActive(true);
            storyOn = true;
        }
        else if (storyOn) {
            storyDialogue.SetActive(false);
            storyOn = false;
        }
    }

    public void startChapter() {
        currentChapter = chapter[playerStory.currentStory];
        currentChapterExpressions = chapterExpressions[playerStory.currentStory];
        playerStory.currentQuote = 0;
        updateDialgoue();
    }

    public void nextQuote() {
        if (playerStory.currentQuote == currentChapter.Length - 1) {
            shiro.finishedStory();
            shiro.acceptMission();
            shiro.nextTarget();
        }
        else {
            playerStory.currentQuote++;
            updateDialgoue();
        }
    }

    public void updateDialgoue() {
        portrait.sprite = expressions[shiroExpression[currentChapterExpressions[playerStory.currentQuote]]];
        quote.text = currentChapter[playerStory.currentQuote];
    }

    public Monster getTarget() {
        return chapterTargets[playerStory.currentStory];
    }


/* Zero ***********************************************************************************************************************/

public static string[] chapterZero =
{@"Hello Hunter!",
@"I'm Shiro nice to meet you! Welcome to our village!",
@"We are currently building our town and we hired you to protect us from nearby monsters.",
@"There's some Jollys nearby, why don't you go out and slay one?"};

public static string[] chapterZeroExpressions = 
{
    "Idle",
    "Smile",
    "Idle",
    "Idle"
};

/* One ***********************************************************************************************************************/

public static string[] chapterOne =
{@"Nice job hunting that Jolly!",
@"Apparently another Jolly watched you slay it's family member.",
@"It was so upset that it transformed into a Bee!",
@"Now that it transformed, it's destroying our farms!",
@"Can you please take care of the Bee?"};

public static string[] chapterOneExpressions = 
{
    "Smile",
    "Idle",
    "Worried",
    "Angry",
    "Worried"
};

/* New ***********************************************************************************************************************/

public static string[] chapterTwo =
{@"Wow you took care of Bee?",
@"Thank you so much!"};

public static string[] chapterTwoExpressions = 
{
    "Smile",
    "Smile"
};

}
