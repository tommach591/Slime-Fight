using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopBalance : MonoBehaviour
{
    private Player_Stats ps;
    [SerializeField] private Text balance;
    [SerializeField] private Vector3 offset;

    [SerializeField] private ChaChing money;
    private ChaChing moneyCopy;

    private int lastCoin;

    // Start is called before the first frame update
    void Start()
    {
        ps = FindObjectOfType<Player_Stats>();
    }

    // Update is called once per frame
    void Update()
    {
        try {
            if (ps.coins > lastCoin) {
                moneyCopy = Instantiate(money, Vector3.zero, Quaternion.identity);
                moneyCopy.transform.SetParent(gameObject.transform);
                moneyCopy.setUp(ps.coins - lastCoin, offset);
            }
        }
        catch {

        }
        balance.text = ps.coins.ToString();
        lastCoin = ps.coins;
    }
}
