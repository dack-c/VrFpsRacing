using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BNG;
public class CarEnter : MonoBehaviour
{
    public GameObject playerController;
    public GameObject vehicle;
    public GameObject carDestination;
    public GameObject enterCube;

    public GameObject rightHandModel;
    public GameObject leftHandModel;

    Quaternion seatRotation; // rotation of player once in the car
    Vector3 seatPosition; //  position of player in the car

    public float carPlayerHeight = -0.35f;

    

    public void EnterCar()
    {
        seatRotation = carDestination.transform.rotation;
        seatPosition = carDestination.transform.position;


        playerController.transform.position = seatPosition; //  set position of player
        playerController.transform.rotation = seatRotation; //  set rotation of the player

        playerController.GetComponent<BNGPlayerController>().CharacterControllerYOffset = carPlayerHeight; // set height of player once in the car

        playerController.GetComponent<CharacterController>().enabled = false; //  disable character contoller

        playerController.GetComponent<PlayerTeleport>().enabled = false; //  disable player teleport

        playerController.GetComponent<LocomotionManager>().enabled = false; //  disable player locomotion manager

        playerController.GetComponent<SmoothLocomotion>().enabled = false; //  disable player smooth locomotion

        playerController.GetComponent<PlayerGravity>().enabled = false; // disable player gravity

        playerController.GetComponent<PlayerRotation>().enabled = false; //  disable player rotation

        //disable hand collision or suffer beating the car around while you are in it
        rightHandModel.GetComponent<HandCollision>().EnableCollisionOnPoint = false;
        rightHandModel.GetComponent<HandCollision>().EnableCollisionOnFist = false;
        leftHandModel.GetComponent<HandCollision>().EnableCollisionOnPoint = false;
        leftHandModel.GetComponent<HandCollision>().EnableCollisionOnFist = false;

        playerController.transform.parent = vehicle.transform; //set player contoller parent from XRrig to the vehicle

       // vehicle.GetComponentInChildren<CarExit>().intheCar = true; // set bool on CarExit script to true

        vehicle.GetComponent<VehicleControl>().activeControl = true;

        carDestination.GetComponent<CarExit>().intheCar = true;

        enterCube.SetActive(false); //  disable the enter cube

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            EnterCar();
        }
    }
}
