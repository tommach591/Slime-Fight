using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStory : MonoBehaviour
{
    public enum Status {
        NotAccepted = 0,
        Accepted = 1,
        Completed = 2
    }

    public Dictionary<int, Status> chapters = new Dictionary<int, Status>() {
        {0, Status.NotAccepted},
        {1, Status.NotAccepted},
        {2, Status.NotAccepted}
    };

    public int currentStory = 0;
    public int currentQuote;
    public int lastStory;
    public Monster target;

    void Start()
    {
        
    }

    void Update()
    {

    }

    public void nextStory() {
        if (currentStory != lastStory) {
            currentStory++;
        }
    }

    public void acceptedMission() {
        if (!isAccepted()) {
            chapters[currentStory] = Status.Accepted;
        }
    }

    public bool isAccepted() {
        if (chapters[currentStory] != Status.Accepted) {
            return false;
        }
        else {
            return true;
        }
    }

    public void completedMission() {
        if (!isCompleted()) {
            chapters[currentStory] = Status.Completed;
        }
    }

    public bool isCompleted() {
        if (chapters[currentStory] != Status.Completed) {
            return false;
        }
        else {
            return true;
        }
    }
}
