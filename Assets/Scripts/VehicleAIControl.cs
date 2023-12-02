using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleAIControl : MonoBehaviour //AI������ �� ��ũ��Ʈ�� ���̸� ��.
{
    public VehicleControl vehicleControl; // �� ������ �� AI������ VehicleControl.cs�� �־�� ��.

    //AI������ �ʱ� ��Ʈ�� ���µ�
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

    //VehicleContol.cs�� keyboardControlMode�� false���� �� �Լ��� �����۵���
    public void ChangeVehicleControlState(float steerFloat, float accelFloat, bool brakeBool, bool shiftBool)
    {
        vehicleControl.steerFloat = steerFloat; //�ڵ��� ȸ�� ����(-1~1. ���� ������ ���� ȸ��, ���� ����� ���� ȸ��)
        vehicleControl.accelFloat = accelFloat; //������ ���� ��(0~1)
        vehicleControl.brakeBool = brakeBool; //�극��ũ�� ��Ҵ��� ����
        vehicleControl.shiftBool = shiftBool; //�� 2������ �ø����� ����(=�ν��� ����)
    }
}
