using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDStats : MonoBehaviour
{
    private Player_Stats ps;
    [SerializeField] Text AtkNum;
    [SerializeField] Text DefNum;
    [SerializeField] Text CritRateNum;
    [SerializeField] Text CritDmgNum;

    // Start is called before the first frame update
    void Start()
    {
        ps = FindObjectOfType<Player_Stats>();
    }

    // Update is called once per frame
    void Update()
    {
        AtkNum.text = ps.getAttack().ToString();
        DefNum.text = ps.getDefense().ToString();
        CritRateNum.text = ps.getCritChance().ToString("F2") + "%";
        CritDmgNum.text = ps.getCritDamage().ToString("F2") + "x";
    }
}
