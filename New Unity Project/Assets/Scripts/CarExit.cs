using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BNG;

public class CarExit : MonoBehaviour
{
    public GameObject xrRig;
    public GameObject playerController;
    public GameObject vehicle;
    public GameObject enterCube;
    public GameObject ExitDestination;
    public GameObject trackingSpace;

    public GameObject rightHandModel;
    public GameObject leftHandModel;

    Quaternion initialRotation;
    Vector3 initialPosition;


    public float playerHeightExit = -0.025f;
    public bool intheCar = false;

    

    void Start()
    {
        initialRotation = trackingSpace.transform.localRotation; // store the initial rotation of the tracking space
        initialPosition = trackingSpace.transform.localPosition; //  store the initial position of the tracking space
    }


    void Update()
    {

        if (playerController.transform.position.y < -75f && intheCar == true || intheCar == true && InputBridge.Instance.XButtonDown)
        {
            playerController.transform.parent = xrRig.transform;

            playerController.transform.position = ExitDestination.transform.position; //transport player out of the car

            playerController.GetComponent<PlayerGravity>().enabled = true; //enable gravity

            playerController.GetComponent<CharacterController>().enabled = true;

            playerController.GetComponent<BNGPlayerController>().CharacterControllerYOffset = playerHeightExit;

            playerController.GetComponent<PlayerTeleport>().enabled = true;

            playerController.GetComponent<LocomotionManager>().enabled = true;

            playerController.GetComponent<SmoothLocomotion>().enabled = true;

            playerController.GetComponent<PlayerRotation>().enabled = true;

            //reenable hand collision so you can punch things again
            rightHandModel.GetComponent<HandCollision>().EnableCollisionOnPoint = true;
            rightHandModel.GetComponent<HandCollision>().EnableCollisionOnFist = true;
            leftHandModel.GetComponent<HandCollision>().EnableCollisionOnPoint = true;
            leftHandModel.GetComponent<HandCollision>().EnableCollisionOnFist = true;

            trackingSpace.transform.localRotation = initialRotation; // reset tracking space rotation
            trackingSpace.transform.localPosition = initialPosition; // reset tracking space position

            vehicle.GetComponent<VehicleControl>().activeControl = false; // disable car

            enterCube.SetActive(true); //enable the enter cube 

            intheCar = false;
        }

    }
}
