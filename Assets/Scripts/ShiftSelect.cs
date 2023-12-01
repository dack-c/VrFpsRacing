using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShiftSelect : MonoBehaviour
{
    public GameObject VehicleControl; //edit
    public int currentGear;

    public void Standard()
    {
        VehicleControl.GetComponent<VehicleControl>().carSetting.automaticGear = false;
    }

    public void Automatic()
    {
        currentGear = VehicleControl.GetComponent<VehicleControl>().currentGear;
        VehicleControl.GetComponent<VehicleControl>().NeutralGear = false;

        if (currentGear < 1)
        {
            VehicleControl.GetComponent<VehicleControl>().ShiftUp();
        }
        VehicleControl.GetComponent<VehicleControl>().carSetting.automaticGear = true;
    }
}
