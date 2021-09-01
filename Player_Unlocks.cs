using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Unlocks : MonoBehaviour
{
    public bool rescuedKuro = false;

    public Dictionary<int, bool> unlockables = new Dictionary<int, bool>() {
        {0, cutlass},
        {1, jollyAxe},
        {2, crystalBow},
        {3, fishGun},
        {4, rainbowWing},
        {5, blackjack}
    };

    public static bool cutlass = false;
    public static bool jollyAxe = false;
    public static bool crystalBow = false;
    public static bool fishGun = false;
    public static bool rainbowWing = false;
    public static bool blackjack = false;

    public Dictionary<int, bool> hunted = new Dictionary<int, bool>() {
        {0, jolly},
        {1, bee},
        {2, tomato},
        {3, apexJolly}
    };

    public static bool jolly = false;
    public static bool bee = false;
    public static bool tomato = false;
    public static bool apexJolly = false;

    void Update() {
        if (rescuedKuro && !unlockables[0]) {
            for (int i = 0; i < 6; i++) {
                unlockables[i] = true;
            }
        }
    }
}
