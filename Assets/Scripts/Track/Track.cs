using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Track : MonoBehaviour
{
    public int finishLaps;
    public Trackpoint[] trackpoints;
    public Finishline finishline;

    private void Awake()
    {
        InitTrackpoints();
    }

    private void InitTrackpoints()
    {
        for (int i = 0; i < trackpoints.Length; i++)
        {
            trackpoints[i].index = i;
        }
    }
}
