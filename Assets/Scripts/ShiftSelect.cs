using System.Threading.Tasks;
using UnityEngine;

public class ShiftSelect : MonoBehaviour
{
    public GameObject stickShifter;
    public GameObject VehicleControl; //edit
    public int currentGear;
    public GameObject ControlButton;

    public async void StartCar()
    {
        // Disable Start buttons, and HUD countdown UI output
        ControlButton.SetActive(false);
        GameManager.I.Hud.startCountdownUI.GetComponent<RaceCountdown>().StartCountdown();

        // Wait for 5 seconds for the countdown to start
        await Task.Delay(5000);

        // start LapController
        if (GameManager.I != null)
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
