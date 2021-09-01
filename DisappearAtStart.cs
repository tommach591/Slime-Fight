using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisappearAtStart : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<SpriteRenderer>().color = new Color(0f, 0f, 0f, 0f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
