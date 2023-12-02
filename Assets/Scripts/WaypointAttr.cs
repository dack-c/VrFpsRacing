using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointAttr : MonoBehaviour
{
    // Start is called before the first frame update
    [System.Serializable]
    public class waypoint
    {
        public bool corner = false;
        public bool befCorner = false;
        public Vector3 waypoint_pos;
    }

    
    public waypoint ways;

    private void Start()
    {
        ways.waypoint_pos = this.gameObject.transform.position;
    }
}
