using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleAIControl : MonoBehaviour //AI차량에 이 스크립트를 붙이면 됨.
{
    public VehicleControl vehicleControl; // 이 변수에 그 AI차량의 VehicleControl.cs를 넣어야 함.

    //AI차량의 초기 컨트롤 상태들
    public float initialSteerFloat = 0f;
    public float initialAccelFloat = 0f;
    public bool initialBrakeBool = false;
    public bool initialShiftBool = false;

    void Start()
    {
        if(vehicleControl == null)
        {
            vehicleControl = gameObject.GetComponent<VehicleControl>();
        }
        ChangeVehicleControlState(initialSteerFloat, initialAccelFloat, initialBrakeBool, initialShiftBool);
    }

    void Update()
    {
        
    }

    //VehicleContol.cs의 keyboardControlMode가 false여야 이 함수가 정상작동함
    public void ChangeVehicleControlState(float steerFloat, float accelFloat, bool brakeBool, bool shiftBool)
    {
        vehicleControl.steerFloat = steerFloat; //핸들의 회전 각도(-1~1. 값이 음수면 좌측 회전, 값이 양수면 우측 회전)
        vehicleControl.accelFloat = accelFloat; //엑셀을 밟은 힘(0~1)
        vehicleControl.brakeBool = brakeBool; //브레이크를 밟았는지 여부
        vehicleControl.shiftBool = shiftBool; //기어를 2단으로 올릴건지 여부(=부스터 여부)
    }
}
