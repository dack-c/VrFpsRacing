using UnityEngine;

public class Track : MonoBehaviour
{
    public int finishLaps;
    public Trackpoint[] trackpoints;
    public Finishline finishline;
    public GameObject trackpointParent;

    private void Start()
    {
        InitTrackpoints();
    }

    private void InitTrackpoints()
    {
        Debug.Log($"initialized Trackpoints");
        trackpoints = trackpointParent.transform.GetComponentsInChildren<Trackpoint>();
        /*for(int i = 0; i < trackpointParent.transform.childCount; i++)
        {
            trackpoints.Add(trackpointParent.transform.GetChild(i).GetComponent<Trackpoint>());
        }*/

        for (int i = 0; i < trackpoints.Length; i++)
        {
            trackpoints[i].index = i;
        }
    }
}
