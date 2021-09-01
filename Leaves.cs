using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Leaves : MonsterProjectile
{
    [SerializeField] protected Sprite[] leaves;

    void Awake()
    {
        setUp();
        int choice = Random.Range(0, leaves.Length);
        sr.sprite = leaves[choice];
    }

    // Update is called once per frame
    new protected void Update()
    {
        fire();
    }
}
