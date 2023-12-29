using BNG;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AI : MonoBehaviour
{
    public WaypointCtrl waypointCtrl;
    public List<WaypointAttr.waypoint> waypoints;
    public GameObject controller;
    public CompeterCtrl competerCtrl;

    [System.Serializable]
    public class stat //�������ͽ�. ���� ���ʿ�
    {
        public int currentWaypoint; //���� �������� ��������Ʈ
        public float currentAngle; //���� ��������Ʈ�� �ڽ��� ����
        public float accelFloat; //����ġ
        public bool brakeBool = false; //�극��ũ
        public bool shiftBool = false; //���
        public List<Transform> targets;
        public int targetNum=-1;
        public float realDist = 0;
        public Vector3 aimingTarget;
    }
    public stat status;

    [System.Serializable]
    public class setting//����. ���Ƿ� ���� ����
    {
        public float waypointRange = 13; //���� ��������Ʈ�� ���ϱ� ���� ������
        public float fullAccelRange = 70; //�ִ� ������ ���� ��������Ʈ�� ������ �־�� �ϴ� �Ÿ�
        public float handling = 4; //������ȯ ����ġ
        public float cornerBrake = 5; //�ڳ� ����
        public float safeRange = 3; //�����Ÿ�
        public bool avoidCol = false; //�����Ÿ� �ؼ� ����
    }
    public setting set;


    // Start is called before the first frame update
    void Start()
    {
        waypoints = waypointCtrl.waypoints;
        status.currentWaypoint = 0;
        status.targets = competerCtrl.location;
    }

    // Update is called once per frame
    void Update()
    {
        if(status.targets.Count!=competerCtrl.location.Count)
        {
            status.targets = competerCtrl.location;
        }


        //���� ��������Ʈ Ž��
        float waypointDist = Vector3.Distance(waypoints[status.currentWaypoint].waypoint_pos, transform.position);
        if (waypointDist<set.waypointRange)
        {
            status.currentWaypoint++;
            if(status.currentWaypoint==waypoints.Count) { status.currentWaypoint = 0; }
        }

        //������������ ���� ����
        if (waypointDist < set.fullAccelRange)
        {
            status.accelFloat = waypointDist / set.fullAccelRange * 1.0f;
        }
        else
        {
            status.accelFloat = 1.0f;
        }

        

        Vector3 fwd = transform.TransformDirection(Vector3.forward);
        Vector3 toTarget = waypoints[status.currentWaypoint].waypoint_pos - transform.position;

        //��� �ڵ������� ���� ����
        if(status.realDist <set.safeRange&&set.avoidCol)
        {
            toTarget += status.aimingTarget *1.2f * -1;
        }

        //�ڵ鸵 ���� ���
        float angle = Vector3.Angle(fwd, toTarget);
        status.currentAngle = Vector3.SignedAngle(fwd, toTarget,Vector3.up)/180*set.handling;

        //�극��ũ ����
        if(!competerCtrl.StartSign)
        {
            status.brakeBool = true;
        }
        else if(controller.GetComponent<VehicleControl>().speed<15.0f)
        {
            status.brakeBool = false;
        }
        else if (waypoints[status.currentWaypoint].befCorner)
        {
            status.brakeBool = true;
        }
        else if(angle>=45)
        {
            status.brakeBool = true;
        }
        else
        {
            status.brakeBool = false;
        }

        //�ڳ� ����
        //if (waypoints[status.currentWaypoint].corner)
        //{
            status.accelFloat /= set.cornerBrake;
        //}

        //����
        controller.GetComponent<VehicleAIControl>().ChangeVehicleControlState(status.currentAngle, status.accelFloat,status.brakeBool,status.shiftBool);
        //controller.GetComponent<VehicleAIControl>().initialSteerFloat = currentAngle;
        //controller.GetComponent<VehicleAIControl>().initialAccelFloat = accelFloat;
        //controller.GetComponent<VehicleAIControl>().initialBrakeBool = brakeBool;
        //controller.GetComponent<VehicleAIControl>().initialShiftBool = shiftBool;

        //�����
        Debug.DrawRay(transform.position, toTarget,Color.white);


        float dist = 0;
        if(status.targetNum!=-1)
        {
            dist = status.realDist;
        }
        for (int i = 0; i < status.targets.Count; i++)
        {
            Vector3 instTarget = status.targets[i].position - controller.transform.position;
            float curDist = instTarget.magnitude;
            
            if (status.targetNum == -1 && (status.targets[i].name != gameObject.name) && status.targets[i].gameObject.activeSelf)
            {
                status.targetNum = i;
                dist = curDist;
            }
            else if (dist > curDist && (status.targets[i].name!=gameObject.name) && status.targets[i].gameObject.activeSelf)
            {
                dist = curDist;
                status.targetNum = i;
            }
        }
        if(status.targetNum>=status.targets.Count)
        {
            status.targetNum = -1;
        }
        
        status.aimingTarget = status.targets[status.targetNum].position-controller.transform.position;
        status.realDist = status.aimingTarget.magnitude;

    }

   

}
