using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatShop : MonoBehaviour
{
    [SerializeField] GameObject shop;
    public bool statShopOn;

    // Start is called before the first frame update
    void Start()
    {
        if (shop.activeSelf) {
            shop.SetActive(false);
        }

        statShopOn = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void toggleStatShop() {
        if (!statShopOn) {
            shop.SetActive(true);
            statShopOn = true;
        }
        else if (statShopOn) {
            shop.SetActive(false);
            statShopOn = false;
        }
    }
}
