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

    public int currentWaypoint;
    
    public float waypointRange;
    public float currentAngle;
    private float accelFloat;
    private float steerFloat;
    public float fullAccelRange;
    private bool brakeBool=false;
    private bool shiftBool = false;
    public float handling=1;
    public float cornerBrake = 2;



    // Start is called before the first frame update
    void Start()
    {
        waypoints = waypointCtrl.waypoints;
        currentWaypoint = 0;
    }

    // Update is called once per frame
    void Update()
    {
        float waypointDist = Vector3.Distance(waypoints[currentWaypoint].waypoint_pos, transform.position);
        if (waypointDist<waypointRange)
        {
            currentWaypoint++;
            if(currentWaypoint==waypoints.Count) { currentWaypoint = 0; }
        }

        if (waypointDist < fullAccelRange)
        {
            accelFloat = waypointDist / fullAccelRange * 1.0f;
        }
        else
        {
            accelFloat = 1.0f;
        }

        

        Vector3 fwd = transform.TransformDirection(Vector3.forward);
        Vector3 toTarget = waypoints[currentWaypoint].waypoint_pos - transform.position;
        float angle = Vector3.Angle(fwd, toTarget);
        currentAngle = Vector3.SignedAngle(fwd, toTarget,Vector3.up)/180*handling;
        if (waypoints[currentWaypoint].befCorner)
        {
            brakeBool = true;
        }
        else if(angle>=45)
        {
            brakeBool = true;
        }
        else
        {
            brakeBool = false;
        }
        if (waypoints[currentWaypoint].corner)
        {
            accelFloat /= cornerBrake;
        }

        controller.GetComponent<VehicleAIControl>().ChangeVehicleControlState(currentAngle, accelFloat,brakeBool,shiftBool);
        //controller.GetComponent<VehicleAIControl>().initialSteerFloat = currentAngle;
        //controller.GetComponent<VehicleAIControl>().initialAccelFloat = accelFloat;
        //controller.GetComponent<VehicleAIControl>().initialBrakeBool = brakeBool;
        //controller.GetComponent<VehicleAIControl>().initialShiftBool = shiftBool;

        Debug.DrawRay(transform.position, toTarget,Color.white);
    }

}
