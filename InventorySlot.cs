using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    [SerializeField] Item item;
    [SerializeField] public Player_Equips equips;
    private Image icon;
    private SpriteRenderer itemImage;
    private Button button;

    public bool unlocked;
    private bool isSetUp = false;
    Color canClick = new Color(1f, 1f, 1f, 1f);
    Color cantClick = new Color(0.5f, 0.5f, 0.5f, 5f);
    
    // Start is called before the first frame update
    void Start()
    {
        if (item != null) {
            button = GetComponent<Button>();

            icon = transform.GetChild(0).GetComponent<Image>();
            if (item.gameObject.tag == "Weapon") {
                itemImage = item.transform.GetComponent<SpriteRenderer>();
            }
            else {
                itemImage = item.transform.GetChild(0).GetComponent<SpriteRenderer>();
            }
            icon.sprite = itemImage.sprite;
            icon.color = new Color(icon.color.r, icon.color.g, icon.color.b, 1f);
            icon.enabled = true;
            
            equips = FindObjectOfType<Player_Equips>();
            
            icon.color = cantClick;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!isSetUp && unlocked) {
            icon.color = canClick;
            setupButton();
        }
    }

    private void setupButton() {
        if (item.gameObject.tag == "Hair") {
            button.onClick.AddListener(delegate { equips.changeHair(item.GetComponent<Hair>()); });
        }
        if (item.gameObject.tag == "Face") {
            button.onClick.AddListener(delegate { equips.changeFace(item.GetComponent<Face>()); });
        }
        if (item.gameObject.tag == "Hat") {
            button.onClick.AddListener(delegate { equips.changeHat(item.GetComponent<Hat>()); });
        }
        if (item.gameObject.tag == "Weapon") {
            button.onClick.AddListener(delegate { equips.changeWeapon(item.GetComponent<Weapon>()); });
        }
        isSetUp = true;
    }
}
