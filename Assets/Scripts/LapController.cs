using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LapController : MonoBehaviour
{
    public int laps = 1;
    public int trackpointIndex = -1;
    public LapValue lapValue;
    public bool isStarted = false;
    public bool isFinished = false;
    public float lapTime = 0.0f;
    public float finishLapTime;

    public Transform Transform;
    public Track CurrentTrack;
    
    public void UpdateLapValue()
    {
        if (isStarted)
        {
            lapTime += Time.deltaTime;
            var passedTrackpoint = CurrentTrack.trackpoints[trackpointIndex].transform.position;
            lapValue = new LapValue
            {
                lap = laps,
                trackpointIndex = trackpointIndex,
                trackpointDistance = Vector3.Distance(Transform.position, passedTrackpoint)
            };
        }
    }

    public void ProcessTrackpoint(Trackpoint trackpoint)
    {
        if (trackpointIndex == trackpoint.index - 1 || trackpointIndex == trackpoint.index + 1)
            trackpointIndex = trackpoint.index;
    }

    public void ProcessFinishline()
    {
        if (trackpointIndex == CurrentTrack.trackpoints.Length - 1)
        {
            laps++;
            if (laps > CurrentTrack.finishLaps && !isFinished)
            {
                isFinished = true;
                finishLapTime = lapTime;
            }
        }
    }

    public struct LapValue
    {
        public int lap;
        public int trackpointIndex;
        public float trackpointDistance;

        public int CompareTo(LapValue other)
        {
            // 1 when this is ahead of other, -1 when this is behind other
            if (lap != other.lap)
                return lap > other.lap ? 1 : -1;

            if (trackpointIndex == other.trackpointIndex)
                return trackpointDistance > other.trackpointDistance ? 1 : -1;

            return trackpointIndex > other.trackpointIndex ? 1 : -1;
        }
    }
}
