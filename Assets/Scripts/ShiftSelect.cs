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
        // start LapController
        if(GameManager.I != null)
        {
            GameManager.I.Player.GetComponent<LapController>().isStarted = true;
            for (int i = 0; i < GameManager.I.Players.Length; i++)
                GameManager.I.Players[i].isStarted = true;
            GameManager.I.CompeterCtrl.StartSign = true;
        }

        currentGear = VehicleControl.GetComponent<VehicleControl>().currentGear;
        VehicleControl.GetComponent<VehicleControl>().NeutralGear = false;

        stickShifter.SetActive(false);
        if (currentGear < 1)
        {
            VehicleControl.GetComponent<VehicleControl>().ShiftUp();
        }
        VehicleControl.GetComponent<VehicleControl>().carSetting.automaticGear = true;
    }
}
