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
    public class stat //스테이터스. 지정 불필요
    {
        public int currentWaypoint; //현재 추적중인 웨이포인트
        public float currentAngle; //현재 웨이포인트와 자신의 각도
        public float accelFloat; //가속치
        public bool brakeBool = false; //브레이크
        public bool shiftBool = false; //기어
        public List<Transform> targets;
        public int targetNum=-1;
        public float realDist = 0;
        public Vector3 aimingTarget;
    }
    public stat status;

    [System.Serializable]
    public class setting//세팅. 임의로 조절 가능
    {
        public float waypointRange = 13; //다음 웨이포인트로 향하기 위한 설정값
        public float fullAccelRange = 70; //최대 가속을 위해 웨이포인트가 떨어져 있어야 하는 거리
        public float handling = 4; //방향전환 가중치
        public float cornerBrake = 5; //코너 감속
        public float safeRange = 3; //안전거리
        public bool avoidCol = false; //안전거리 준수 설정
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


        //다음 웨이포인트 탐색
        float waypointDist = Vector3.Distance(waypoints[status.currentWaypoint].waypoint_pos, transform.position);
        if (waypointDist<set.waypointRange)
        {
            status.currentWaypoint++;
            if(status.currentWaypoint==waypoints.Count) { status.currentWaypoint = 0; }
        }

        //엑셀러레이터 강도 조절
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

        //상대 자동차와의 간격 조절
        if(status.realDist <set.safeRange&&set.avoidCol)
        {
            toTarget += status.aimingTarget *1.2f * -1;
        }

        //핸들링 각도 계산
        float angle = Vector3.Angle(fwd, toTarget);
        status.currentAngle = Vector3.SignedAngle(fwd, toTarget,Vector3.up)/180*set.handling;

        //브레이크 조건
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

        //코너 감속
        //if (waypoints[status.currentWaypoint].corner)
        //{
            status.accelFloat /= set.cornerBrake;
        //}

        //추진
        controller.GetComponent<VehicleAIControl>().ChangeVehicleControlState(status.currentAngle, status.accelFloat,status.brakeBool,status.shiftBool);
        //controller.GetComponent<VehicleAIControl>().initialSteerFloat = currentAngle;
        //controller.GetComponent<VehicleAIControl>().initialAccelFloat = accelFloat;
        //controller.GetComponent<VehicleAIControl>().initialBrakeBool = brakeBool;
        //controller.GetComponent<VehicleAIControl>().initialShiftBool = shiftBool;

        //디버그
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
