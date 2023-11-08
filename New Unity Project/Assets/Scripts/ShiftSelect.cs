using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShiftSelect : MonoBehaviour
{
    public GameObject stickShifter;
    public GameObject VehicleControl; //edit
    public int currentGear;

    public void Standard()
    {
        stickShifter.SetActive(true);
        VehicleControl.GetComponent<VehicleControl>().carSetting.automaticGear = false;
    }

    public void Automatic()
    {
        currentGear = VehicleControl.GetComponent<VehicleControl>().currentGear;

        stickShifter.SetActive(false);
        if (currentGear < 1)
        {
            VehicleControl.GetComponent<VehicleControl>().ShiftUp();
        }
        VehicleControl.GetComponent<VehicleControl>().carSetting.automaticGear = true;
    }
}
