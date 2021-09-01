using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Equips : MonoBehaviour
{
    [SerializeField] private GameObject player;
    private GameObject head;
    private GameObject hair;
    private GameObject face;
    private GameObject hat;
    private GameObject weapon;

    // Start is called before the first frame update
    void Start()
    {
        head = transform.Find("Head").gameObject;
        hair = head.transform.Find("Hair").gameObject;
        face = head.transform.Find("Face").gameObject;
        hat = head.transform.Find("Hat").gameObject;
        weapon = player.transform.Find("Weapon").gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void changeHair(Hair newHair) {
        if (hair.transform.GetChild(0).gameObject.GetComponent<Hair>() != newHair) {
            Destroy(hair.transform.GetChild(0).gameObject);
            Hair myNewHair = Instantiate(newHair, hair.transform);
        }
    }

    public void changeFace(Face newFace) {
        if (face.transform.GetChild(0).gameObject.GetComponent<Face>() != newFace) {
            Destroy(face.transform.GetChild(0).gameObject);
            Face myNewFace = Instantiate(newFace, face.transform);
        }
    }

    public void changeHat(Hat newHat) {
        if (hat.transform.GetChild(0).gameObject.GetComponent<Hat>() != newHat) {
            Destroy(hat.transform.GetChild(0).gameObject);
            Hat myNewHat = Instantiate(newHat, hat.transform);
        }
    }

    public void changeWeapon(Weapon newWeapon) {
        if (weapon.transform.GetChild(0).gameObject.GetComponent<Weapon>() != newWeapon) {
            Destroy(weapon.transform.GetChild(0).gameObject);
            Weapon myNewWeapon = Instantiate(newWeapon, weapon.transform);
            player.GetComponent<PlayerController>().changePrimaryWeapon(myNewWeapon);
        }
    }
}
