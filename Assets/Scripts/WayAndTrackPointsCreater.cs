using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EasyRoads3Dv3;

public class WayAndTrackPointsCreater : MonoBehaviour
{
    private ERRoadNetwork roadNetwork;

    public GameObject waypointPrefab; // 웨이포인트로 사용할 프리팹
    public MeshCollider trackCollider; // 트랙의 메시 콜라이더
    public GameObject wayPointParent; //웨이포인트의 부모 오브젝트
    public int wayPointInterval = 8;

    public GameObject trackpointPrefab; // 트렉포인트로 사용할 프리팹
    public GameObject trackPointParent; //트랙포인트의 부모 오브젝트
    public GameObject finishLineObj;
    public int trackPointInterval = 1;

    private List<Transform> waypoints = new List<Transform>(); // 웨이포인트 리스트
    private List<Vector3> rightEdgeForWaypoints = new List<Vector3>();
    private List<Transform> trackpoints = new List<Transform>();

    // Start is called before the first frame update
    void Awake()
    {
        /*EasyRoad 프로버전에서만 사용가능한 스크립트
        roadNetwork = new ERRoadNetwork();
        ERRoad erRoad = roadNetwork.GetRoadByName(gameObject.name);
        erRoad.GetMarkerPositions();
        Debug.Log(erRoad.GetMarkerCount());*/
        if(wayPointInterval % 2 == 1)//홀수면 짝수로 바꾸기
        {
            wayPointInterval++;
        }

        Vector3[] colliderVertices = trackCollider.sharedMesh.vertices; // 메시 콜라이더의 모든 버텍스를 가져옴

        for (int i = 0; i < colliderVertices.Length; i += wayPointInterval)//waypoint 생성
        {
            Vector3 centerPoint = (colliderVertices[i] + colliderVertices[i + 1]) / 2.0f;
            GameObject waypointObj = Instantiate(waypointPrefab, centerPoint, Quaternion.identity);
            waypoints.Add(waypointObj.transform);
            rightEdgeForWaypoints.Add(colliderVertices[i]);
            waypointObj.transform.parent = wayPointParent.transform;
        }

        for(int i = 0; i < waypoints.Count-trackPointInterval; i += trackPointInterval) //waypoint위치에 trackpoint생성
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
