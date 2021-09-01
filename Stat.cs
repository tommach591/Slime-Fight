using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Stat : MonoBehaviour
{
    private Player_Stats ps;
    [SerializeField] Button purchase;
    [SerializeField] Text costText;
    [SerializeField] Text statNumber;
    private string stat;

    private int cost;

    // Start is called before the first frame update
    void Start()
    {
        ps = FindObjectOfType<Player_Stats>();
        stat = gameObject.name;
        purchase.onClick.AddListener(makePurchase);
        updateStat();
        updateShop();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void updateShop() {
        switch(stat) {
            case "Attack":
                cost = ps.getAttack() + 1 - 1;
                break;
            case "Health":
                cost = ps.getMaxHealth() + 1 - 20;
                break;
            case "Defense":
                cost = ps.getDefense() + 1;
                break;
            case "CritRate":
                cost = (int)((ps.getCritChance() * 2) + 1 - 10);
                break;
            case "CritDmg":
                cost = (int)((ps.getCritDamage() * 20) + 1 - 30);
                break;
            default:
                break;
        }
        costText.text = cost.ToString();
    }

    void updateStat() {
        switch(stat) {
            case "Attack":
                statNumber.text = ps.getAttack().ToString();
                break;
            case "Health":
                statNumber.text = ps.getMaxHealth().ToString();
                break;
            case "Defense":
                statNumber.text = ps.getDefense().ToString();
                break;
            case "CritRate":
                statNumber.text = ps.getCritChance().ToString("F2");
                break;
            case "CritDmg":
                statNumber.text = ps.getCritDamage().ToString("F2");
                break;
            default:
                break;
        }
    }

    void makePurchase() {
        if (ps.coins >= cost) {
            switch(stat) {
                case "Attack":
                    ps.incAttack();
                    break;
                case "Health":
                    ps.incMaxHealth();
                    break;
                case "Defense":
                    ps.incDefense();
                    break;
                case "CritRate":
                    ps.incCritChance();
                    break;
                case "CritDmg":
                    ps.incCritDamage();
                    break;
                default:
                    break;
            }
            ps.coins -= cost;
            updateStat();
            updateShop();
        }
    }
}
