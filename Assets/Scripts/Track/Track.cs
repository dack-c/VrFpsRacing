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
        Debug.Log($"initialized Trackpoints");
        for (int i = 0; i < trackpoints.Length; i++)
        {
            trackpoints[i].index = i;
        }
    }
}
