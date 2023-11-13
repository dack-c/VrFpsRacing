using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BNG;
public class MyVRCarController : MonoBehaviour
{
    public float steerFloat;
    public float accelFloat;
    public bool brakeBool;
    public bool shiftBool;

    public GameObject VehicleControl;
    public JoystickVehicleControl joyStickShifter;

    public int vehicleGear;

    public float VibrateFrequency = 0.3f;
    public float VibrateAmplitude = 0.1f;
    public float VibrateDuration = 0.1f;

    public Grabber rightGrabber;
    public Grabber leftGrabber;

    public Grabbable joystickGrabbable;

    void Update()
    {
        accelFloat = InputBridge.Instance.RightTrigger + -InputBridge.Instance.LeftTrigger;

        if (InputBridge.Instance.AButton)
        {
            brakeBool = true;
        }

        else
        {
            brakeBool = false;
        }

        if (InputBridge.Instance.BButton)
        {
            shiftBool = true;
        }

        else
        {
            shiftBool = false;
        }


        VehicleControl.GetComponent<VehicleControl>().steerFloat = steerFloat;
        VehicleControl.GetComponent<VehicleControl>().accelFloat = accelFloat;
        VehicleControl.GetComponent<VehicleControl>().brakeBool = brakeBool;
        VehicleControl.GetComponent<VehicleControl>().shiftBool = shiftBool;

        vehicleGear = VehicleControl.GetComponent<VehicleControl>().currentGear;
    }

    public void CarSteering(float onValueChange)
    {
        steerFloat = -onValueChange;
    }

    public void Shift(Vector2 LeverVector)
    {
        if (LeverVector.y > 0.4f && LeverVector.x <= -0.4f)
        {
            joyStickShifter.ReturnToCenterSpeed = 0;
            VehicleControl.GetComponent<VehicleControl>().NeutralGear = false;
            if (vehicleGear > 1)
            {
                VehicleControl.GetComponent<VehicleControl>().currentGear = 2;
                VehicleControl.GetComponent<VehicleControl>().ShiftDown();
                DoHaptics();
            }

            else if (vehicleGear < 1)
            {
                VehicleControl.GetComponent<VehicleControl>().currentGear = 1;
                DoHaptics();

            }



        }

        if (LeverVector.y < -0.4f && LeverVector.x <= -0.4f)
        {
            joyStickShifter.ReturnToCenterSpeed = 0;
            VehicleControl.GetComponent<VehicleControl>().NeutralGear = false;
            if (vehicleGear > 2)
            {
                VehicleControl.GetComponent<VehicleControl>().currentGear = 3;
                VehicleControl.GetComponent<VehicleControl>().ShiftDown();
                DoHaptics();
            }

            else if (vehicleGear < 2)
            {
                VehicleControl.GetComponent<VehicleControl>().currentGear = 1;
                VehicleControl.GetComponent<VehicleControl>().ShiftUp();
                DoHaptics();
            }

        }


        if (LeverVector.y > 0.4f && LeverVector.x > -0.4f && LeverVector.x < 0.4f)
        {
            joyStickShifter.ReturnToCenterSpeed = 0;
            VehicleControl.GetComponent<VehicleControl>().NeutralGear = false;
            if (vehicleGear > 3)
            {
                VehicleControl.GetComponent<VehicleControl>().currentGear = 4;
                VehicleControl.GetComponent<VehicleControl>().ShiftDown();
                DoHaptics();
            }

            else if (vehicleGear < 3)
            {
                VehicleControl.GetComponent<VehicleControl>().currentGear = 2;
                VehicleControl.GetComponent<VehicleControl>().ShiftUp();
                DoHaptics();
            }
        }

        if (LeverVector.y < -0.4f && LeverVector.x > -0.4f && LeverVector.x < 0.4f)
        {
            joyStickShifter.ReturnToCenterSpeed = 0;
            VehicleControl.GetComponent<VehicleControl>().NeutralGear = false;
            if (vehicleGear > 4)
            {
                VehicleControl.GetComponent<VehicleControl>().currentGear = 5;
                VehicleControl.GetComponent<VehicleControl>().ShiftDown();
                DoHaptics();
            }

            else if (vehicleGear < 4)
            {
                VehicleControl.GetComponent<VehicleControl>().currentGear = 3;
                VehicleControl.GetComponent<VehicleControl>().ShiftUp();
                DoHaptics();
            }

        }

        if (LeverVector.y > 0.4f && LeverVector.x > 0.4f)
        {

            joyStickShifter.ReturnToCenterSpeed = 0;
            VehicleControl.GetComponent<VehicleControl>().NeutralGear = false;
            if (vehicleGear < 5)
            {
                VehicleControl.GetComponent<VehicleControl>().currentGear = 4;
                VehicleControl.GetComponent<VehicleControl>().ShiftUp();
                DoHaptics();

            }
        }

        if (LeverVector.y < 0.4f && LeverVector.x > 0.4f)
        {
            joyStickShifter.ReturnToCenterSpeed = 0;
            VehicleControl.GetComponent<VehicleControl>().currentGear = 0;
            VehicleControl.GetComponent<VehicleControl>().ShiftDown();
            DoHaptics();
        }


        else if (LeverVector.y > -0.4 && LeverVector.y < 0.4 && LeverVector.x > -0.4 && LeverVector.x < 0.4)
        {
            DoHaptics();
            joyStickShifter.ReturnToCenterSpeed = 10;
            VehicleControl.GetComponent<VehicleControl>().NeutralGear = true;

        }

    }

    void DoHaptics()
    {
        if (rightGrabber.HeldGrabbable == joystickGrabbable)
        {
            InputBridge.Instance.VibrateController(VibrateFrequency, VibrateAmplitude, VibrateDuration, rightGrabber.HandSide);
        }

        else if (leftGrabber.HeldGrabbable == joystickGrabbable)
        {
            InputBridge.Instance.VibrateController(VibrateFrequency, VibrateAmplitude, VibrateDuration, leftGrabber.HandSide);
        }
    }
}
