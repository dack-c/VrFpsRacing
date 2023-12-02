using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointCtrl : MonoBehaviour
{

    public List<WaypointAttr.waypoint> waypoints;
    public Component[] components;

    private void Awake()
    {
        components = GetComponentsInChildren<WaypointAttr>();
        for (int i = 0; i < components.Length; i++)
        {
            waypoints.Add(components[i].GetComponent<WaypointAttr>().ways);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
