using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Mission : MonoBehaviour
{
    [SerializeField] public Monster target;
    [SerializeField] public string destination;

    [SerializeField] private Monster reset;
    private string noDestination = "None";

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void setMission(Monster t, string d) {
        target = t;
        destination = d;
    }

    public void resetMission() {
        target = reset;
        destination = noDestination;
    }

    public bool isEmpty() {
        if (destination == noDestination) {
            return true;
        }
        else {
            return false;
        }
    }

}
