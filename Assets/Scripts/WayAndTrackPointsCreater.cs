using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EasyRoads3Dv3;

public class WayAndTrackPointsCreater : MonoBehaviour
{
    private ERRoadNetwork roadNetwork;

    public GameObject waypointPrefab; // ��������Ʈ�� ����� ������
    public MeshCollider trackCollider; // Ʈ���� �޽� �ݶ��̴�
    public GameObject wayPointParent; //��������Ʈ�� �θ� ������Ʈ
    public int wayPointInterval = 8;

    public GameObject trackpointPrefab; // Ʈ������Ʈ�� ����� ������
    public GameObject trackPointParent; //Ʈ������Ʈ�� �θ� ������Ʈ
    public GameObject finishLineObj;
    public int trackPointInterval = 1;

    private List<Transform> waypoints = new List<Transform>(); // ��������Ʈ ����Ʈ
    private List<Vector3> rightEdgeForWaypoints = new List<Vector3>();
    private List<Transform> trackpoints = new List<Transform>();

    // Start is called before the first frame update
    void Awake()
    {
        /*EasyRoad ���ι��������� ��밡���� ��ũ��Ʈ
        roadNetwork = new ERRoadNetwork();
        ERRoad erRoad = roadNetwork.GetRoadByName(gameObject.name);
        erRoad.GetMarkerPositions();
        Debug.Log(erRoad.GetMarkerCount());*/
        if(wayPointInterval % 2 == 1)//Ȧ���� ¦���� �ٲٱ�
        {
            wayPointInterval++;
        }

        Vector3[] colliderVertices = trackCollider.sharedMesh.vertices; // �޽� �ݶ��̴��� ��� ���ؽ��� ������

        for (int i = 0; i < colliderVertices.Length; i += wayPointInterval)//waypoint ����
        {
            Vector3 centerPoint = (colliderVertices[i] + colliderVertices[i + 1]) / 2.0f;
            GameObject waypointObj = Instantiate(waypointPrefab, centerPoint, Quaternion.identity);
            waypoints.Add(waypointObj.transform);
            rightEdgeForWaypoints.Add(colliderVertices[i]);
            waypointObj.transform.parent = wayPointParent.transform;
        }

        for(int i = 0; i < waypoints.Count-trackPointInterval; i += trackPointInterval) //waypoint��ġ�� trackpoint����
        {
            GameObject trackpointObj = Instantiate(trackpointPrefab, waypoints[i].position, Quaternion.identity);
            trackpointObj.transform.LookAt(rightEdgeForWaypoints[i]);
            trackpointObj.transform.Rotate(0, 90, 0);
            trackpointObj.transform.Translate(0, 3.5f, 0);
            trackpoints.Add(trackpointObj.transform);
            trackpointObj.transform.parent = trackPointParent.transform;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
