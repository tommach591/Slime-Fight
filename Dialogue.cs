using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dialogue : MonoBehaviour
{
    [SerializeField] private PlayerController player;
    [SerializeField] private Text quote;
    [SerializeField] private Text npcName;
    [SerializeField] public Button function;
    [SerializeField] public Button chat;
    public bool dialogueOn;

    private GameObject chatbox;

    // Start is called before the first frame update
    void Start()
    {
        npcName.text = transform.parent.parent.name;
        chatbox = gameObject.transform.GetChild(0).gameObject;
        dialogueOn = false;
        chatbox.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void toggleDialogue() {
        if (!dialogueOn) {
            chatbox.SetActive(true);
            dialogueOn = true;
        }
        else if (dialogueOn) {
            chatbox.SetActive(false);
            dialogueOn = false;
        }
    }

    public void setQuote(string s) {
        quote.text = s;
    }

}
