using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoostItemController : MonoBehaviour
{
    public GameObject player;
    public float boostDuration = 5f;
    public float boostPower = 300f;

    private VehicleControl vehicleControl; //플레이어가 탄 차량의 VehicleControl.cs
    private bool isUsed = false;

    // Start is called before the first frame update
    void Start()
    {
        if(player == null)
        {
            player = GameObject.FindWithTag("Player");
        }
    }

    public void StartBoost()// boostDuration만큼 부스트 작동
    {
        if(!isUsed)
        {
            isUsed = true;
            GameManager.I.PlayerItemController.CleanCurrentSlot();
            gameObject.GetComponent<MeshRenderer>().enabled = false;
            StartCoroutine(StartBoostCoroutine());
        }
    }

    private IEnumerator StartBoostCoroutine()
    {
        vehicleControl = player.transform.parent.GetComponent<VehicleControl>();
        if (vehicleControl != null)
        {
            float originalShiftPower = vehicleControl.carSetting.shiftPower;
            vehicleControl.carSetting.shiftPower = boostPower;//shiftPower가 boost의 power랑 같음
            vehicleControl.shift = true;
            yield return new WaitForSeconds(boostDuration);
            vehicleControl.carSetting.shiftPower = originalShiftPower;
            vehicleControl.shift = false;

            
            Destroy(gameObject);
        }
    }
}
