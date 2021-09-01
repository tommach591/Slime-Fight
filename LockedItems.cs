using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockedItems : MonoBehaviour
{
    [SerializeField] public InventorySlot[] slots;
    [SerializeField] public Player_Unlocks pu;

    // Start is called before the first frame update
    void Start()
    {
        pu = FindObjectOfType<Player_Unlocks>();
        checkWeaponRowTwo();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void unlock(int i, bool key) {
        slots[i].unlocked = key;
    }

    public void checkWeaponRowTwo() {
        for (int i = 0; i < 6; i++) {
            unlock(i, pu.unlockables[i]);
        }
    }
}
