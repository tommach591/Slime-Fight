using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    [SerializeField] CameraController cam;
    [SerializeField] GameObject hairInventory;
    [SerializeField] GameObject faceInventory;
    [SerializeField] GameObject hatInventory;
    [SerializeField] GameObject weaponInventory;

    private GameObject lastInventory;

    [SerializeField] private PlayerController player;

    [SerializeField] Button hairButton;
    [SerializeField] Button faceButton;
    [SerializeField] Button hatButton;
    [SerializeField] Button weaponButton;

    private InventorySlot[] hairSlots;
    private InventorySlot[] faceSlots;
    private InventorySlot[] hatSlots;
    private InventorySlot[] weaponSlots;
    
    public bool inventoryOn;

    private float startTime;
    private float elapsed;
    private float duration = 0.75f;
    private bool transitionIn;
    private bool transitionOut;
    public bool doneTransition;

    private float endingx = 12f;
    private float startingx = 0f;

    // Start is called before the first frame update
    void Start()
    {
        hairSlots = hairInventory.transform.GetChild(0).GetComponentsInChildren<InventorySlot>();
        faceSlots = faceInventory.transform.GetChild(0).GetComponentsInChildren<InventorySlot>();
        hatSlots = hatInventory.transform.GetChild(0).GetComponentsInChildren<InventorySlot>();
        weaponSlots = weaponInventory.transform.GetChild(0).GetComponentsInChildren<InventorySlot>();

        hairButton.onClick.AddListener(toHairInventory);
        faceButton.onClick.AddListener(toFaceInventory);
        hatButton.onClick.AddListener(toHatInventory);
        weaponButton.onClick.AddListener(toWeaponInventory);

        if (hairInventory.activeSelf) {
            hairInventory.SetActive(false);
            faceInventory.SetActive(false);
            hatInventory.SetActive(false);
            weaponInventory.SetActive(false);

            hairButton.gameObject.SetActive(false);
            faceButton.gameObject.SetActive(false);
            hatButton.gameObject.SetActive(false);
            weaponButton.gameObject.SetActive(false);
        }
        
        inventoryOn = false;
        doneTransition = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (transitionIn) {
            doneTransition = false;
            elapsed = (Time.time - startTime) / duration;
            Camera.main.orthographicSize = Mathf.SmoothStep(27f, 15f, elapsed);
            cam.xOffset = Mathf.SmoothStep(startingx, endingx, elapsed);
            if (elapsed > duration) {
                transitionIn = false;
                doneTransition = true;
            }
            cam.xOffset = endingx;
        }
        else if (transitionOut) {
            doneTransition = false;
            elapsed = (Time.time - startTime) / duration;
            Camera.main.orthographicSize = Mathf.SmoothStep(15f, 27f, elapsed);
            cam.xOffset = Mathf.SmoothStep(endingx, startingx, elapsed);
            if (elapsed > duration) {
                transitionOut = false;
                doneTransition = true;
            }
            cam.xOffset = startingx;
        }
    }

    public void toggleMenu() {
        startTime = Time.time;
        elapsed = 0f;
        if (!inventoryOn) {
            if (lastInventory == null) {
                hairInventory.SetActive(true);
            }
            else {
                lastInventory.SetActive(true);
            }

            hairButton.gameObject.SetActive(true);
            faceButton.gameObject.SetActive(true);
            hatButton.gameObject.SetActive(true);
            weaponButton.gameObject.SetActive(true);

            inventoryOn = true;
            transitionIn = true;             
        }
        else if (inventoryOn) {
            hairInventory.SetActive(false);
            faceInventory.SetActive(false);
            hatInventory.SetActive(false);
            weaponInventory.SetActive(false);

            hairButton.gameObject.SetActive(false);
            faceButton.gameObject.SetActive(false);
            hatButton.gameObject.SetActive(false);
            weaponButton.gameObject.SetActive(false);

            inventoryOn = false;
            transitionOut = true;
        }
    }

    public void toHairInventory() {
        hairInventory.SetActive(true);
        lastInventory = hairInventory;
        faceInventory.SetActive(false);
        hatInventory.SetActive(false);
        weaponInventory.SetActive(false);
    }

    public void toFaceInventory() {
        hairInventory.SetActive(false);
        faceInventory.SetActive(true);
        lastInventory = faceInventory;
        hatInventory.SetActive(false);
        weaponInventory.SetActive(false);
    }

    public void toHatInventory() {
        hairInventory.SetActive(false);
        faceInventory.SetActive(false);
        hatInventory.SetActive(true);
        lastInventory = hatInventory;
        weaponInventory.SetActive(false);
    }

    public void toWeaponInventory() {
        hairInventory.SetActive(false);
        faceInventory.SetActive(false);
        hatInventory.SetActive(false);
        weaponInventory.SetActive(true);
        lastInventory = weaponInventory;
    }

}
