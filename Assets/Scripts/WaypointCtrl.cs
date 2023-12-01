using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointCtrl : MonoBehaviour
{

    public List<Transform> waypoints;

    private void Awake()
    {
        foreach (Transform tr in gameObject.GetComponentInChildren<Transform>())
        {
            waypoints.Add(tr);
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
