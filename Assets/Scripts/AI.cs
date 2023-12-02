using BNG;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AI : MonoBehaviour
{

    public WaypointCtrl waypointCtrl;
    public List<Transform> waypoints;
    public int currentWaypoint;
    public GameObject controller;
    public float waypointRange;
    public float currentAngle;
    private float accelFloat;
    private float steerFloat;
    public float fullAccelRange;
    private bool brakeBool=false;
    private bool shiftBool = false;
    private int currentGear;



    // Start is called before the first frame update
    void Start()
    {
        waypoints = waypointCtrl.waypoints;
        currentWaypoint = 0;
    }

    // Update is called once per frame
    void Update()
    {
        float waypointDist = Vector3.Distance(waypoints[currentWaypoint].position, transform.position);
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
        currentAngle = Vector3.SignedAngle(fwd, waypoints[currentWaypoint].position - transform.position,Vector3.up)/180;
        controller.GetComponent<VehicleAIControl>().initialSteerFloat = currentAngle;
        controller.GetComponent<VehicleAIControl>().initialAccelFloat = accelFloat;
        controller.GetComponent<VehicleAIControl>().initialBrakeBool = brakeBool;
        controller.GetComponent<VehicleAIControl>().initialShiftBool = shiftBool;

        Debug.DrawRay(transform.position, waypoints[currentWaypoint].position-transform.position,Color.white);
    }

}
