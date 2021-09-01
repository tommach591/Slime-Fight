using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

	[SerializeField] public GameObject player;
	[SerializeField] OnCameraScreen[] invisibleWalls;
	[SerializeField] OnCameraScreen[] floor;

    private GameObject playerCamera;

    private int numberOfFloors;
    private int numberOfFloorsSeen;
    private bool floorSeen;
    private float yStop;

    private int numberOfWalls;
    private int numberOfWallsSeen;
    private bool wallSeen;
    private float xStop;

    private bool onLeft;
    private bool onRight;

    public float xOffset = 0;
    public float yOffset = 0;

    // Start is called before the first frame update
    void Start()
    {
        numberOfWalls = invisibleWalls.Length;
        numberOfFloors = floor.Length;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        refreshCam();
        checkWalls();
        checkFloors();
        if (wallSeen && ((onLeft && playerCamera.transform.position.x < xStop) || (onRight && playerCamera.transform.position.x > xStop))) {
            /*
            if (floorSeen && playerCamera.transform.position.y < yStop) {
                transform.position = new Vector3(xStop, yStop, transform.position.z);
            }
            else {
                transform.position = new Vector3(xStop, player.transform.position.y, transform.position.z);
            }
            */
            transform.position = new Vector3(xStop + xOffset, playerCamera.transform.position.y + yOffset, transform.position.z);
        }
        /*
        else if (floorSeen && playerCamera.transform.position.y < yStop) {
            if (wallSeen && ((onLeft && playerCamera.transform.position.x < xStop) || (onRight && playerCamera.transform.position.x > xStop))) {
                transform.position = new Vector3(xStop, yStop, transform.position.z);
            }
            else {
                transform.position = new Vector3(playerCamera.transform.position.x, yStop, transform.position.z);
            }
        }*/
        else {
            transform.position = new Vector3(playerCamera.transform.position.x + xOffset, playerCamera.transform.position.y + yOffset, transform.position.z);
        }
    }

    public void refreshCam() {
        if (playerCamera == null) {
            playerCamera = player.transform.Find("CameraPoint").gameObject;
        }
    }

    void checkWalls() {
        numberOfWallsSeen = 0;
        onLeft = false;
        onRight = false;
        for (int i = 0; i < numberOfWalls; i++) {
            if (invisibleWalls[i].inView) {
                numberOfWallsSeen++;
                if (transform.position.x > invisibleWalls[i].transform.position.x) {
                    onLeft = true;
                }
                if (transform.position.x < invisibleWalls[i].transform.position.x) {
                    onRight = true;
                }
            }
        }
        if (numberOfWallsSeen != 0) {
            wallSeen = true;
        }
        else {
            wallSeen = false;
        }
        if (wallSeen) {
            xStop = transform.position.x;
        }
    }

    void checkFloors() {
        numberOfFloorsSeen = 0;
        for (int i = 0; i < numberOfFloors; i++) {
            if (floor[i].inView) {
                numberOfFloorsSeen++;
            }
        }
        if (numberOfFloorsSeen != 0) {
            floorSeen = true;
        }
        else {
            floorSeen = false;
        }
        if (floorSeen) {
            yStop = transform.position.y;
        }
    }
}
