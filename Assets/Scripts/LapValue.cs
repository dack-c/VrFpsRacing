using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LapValue
{
    public int lap;
    public int trackpointIndex;
    public float trackpointDistance;

    public int CompareTo(LapValue other)
    {
        // 1 when this is ahead of other, -1 when this is behind other
        if (lap != other.lap)
            return lap > other.lap ? -1 : 1;

        if (trackpointIndex == other.trackpointIndex)
            return trackpointDistance > other.trackpointDistance ? -1 : 1;

        return trackpointIndex > other.trackpointIndex ? -1 : 1;
    }
}
