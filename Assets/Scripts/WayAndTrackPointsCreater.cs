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

    public List<int> waypointAttrList = new List<int>();
    private int th = 1;
    public List<GameObject> waypointsList = new List<GameObject>();

    public bool manual = false;


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
        List<Vector3> centerPoints = new List<Vector3>();
        double distance = 0;

        /*for (int i = 0; i < colliderVertices.Length; i += wayPointInterval)//waypoint 생성
        {
            Vector3 centerPoint = (colliderVertices[i] + colliderVertices[i + 1]) / 2.0f;

                GameObject waypointObj = Instantiate(waypointPrefab, centerPoint, Quaternion.identity);
                waypoints.Add(waypointObj.transform);
                rightEdgeForWaypoints.Add(colliderVertices[i]);
                waypointObj.transform.parent = wayPointParent.transform;
                if (waypointAttrList.Contains(th) && manual)
                {
                    waypointObj.GetComponent<WaypointAttr>().ways.befCorner = true;
                    waypointsList.Add(waypointObj);
                }
                waypointObj.GetComponent<WaypointAttr>().ways.element = waypointObj.transform;
                waypointObj.GetComponent<WaypointAttr>().ways.th = th;
                th++;
        }
        */
        for (int i = 0; i < colliderVertices.Length; i += 2)//waypoint 생성
        {
            Vector3 centerPoint = (colliderVertices[i] + colliderVertices[i + 1]) / 2.0f;
            centerPoints.Add(centerPoint);
            if (centerPoints.Count >= 2)
            {
                Vector3 inst = centerPoints[centerPoints.Count-1] - centerPoints[centerPoints.Count - 2];
                inst.y = 0;
                distance += inst.magnitude;
            }

            if (i%wayPointInterval==0)
            {
                GameObject waypointObj = Instantiate(waypointPrefab, centerPoint, Quaternion.identity);
                waypoints.Add(waypointObj.transform);
                rightEdgeForWaypoints.Add(colliderVertices[i]);
                waypointObj.transform.parent = wayPointParent.transform;
                if (waypointAttrList.Contains(th)&&manual)//수동 코너 지정
                {
                    waypointObj.GetComponent<WaypointAttr>().ways.befCorner = true;
                    waypointsList.Add(waypointObj);
                }
                waypointObj.GetComponent<WaypointAttr>().ways.element = waypointObj.transform;
                waypointObj.GetComponent<WaypointAttr>().ways.th = th;
                waypointObj.GetComponent<WaypointAttr>().ways.length = distance;
                th++;
                distance = 0;
            }
        }
        if(distance!=0)
        {
            waypoints[0].GetComponent<WaypointAttr>().ways.length += distance;
            distance = 0;
        }

        //자동 코너 지정
        if (!manual)
        {
            //웨이포인트 2개 사이의 코너 감지
            for (int i = 0; i < waypoints.Count; i++)
            {
                Vector3 frontPoint;
                Vector3 backPoint = waypoints[i].position;
                if(i==0)
                {
                    frontPoint = waypoints[waypoints.Count-1].position;
                }
                else
                {
                    frontPoint = waypoints[i-1].position;
                }
                frontPoint.y = 0;
                backPoint.y = 0;
                double lineDist = (frontPoint-backPoint).magnitude;
                double curve = waypoints[i].GetComponent<WaypointAttr>().ways.length / lineDist;
                curve -= 1;
                curve /= lineDist;
                curve *= 100;
                if(curve<0.0001)
                {
                    curve = 0;
                }
                waypoints[i].GetComponent<WaypointAttr>().ways.lineDist = lineDist;
                waypoints[i].GetComponent<WaypointAttr>().ways.curve = curve;
                if(curve>0.1)
                {
                    waypoints[i].GetComponent <WaypointAttr>().ways.befCorner = true;
                }
            }
            
            //웨이포인트 3개의 각도 감지
            for(int i=0;i<waypoints.Count-1;i++)
            {
                int frontN = i;
                int midN=i+1;
                int backN = i+2;
                if (i + 1 >= waypoints.Count)
                {
                    midN -=waypoints.Count;
                }
                if (i + 2 >= waypoints.Count)
                {
                    backN -=waypoints.Count;
                }

                Vector3 front = waypoints[frontN].position;
                Vector3 mid=waypoints[midN].position;
                Vector3 back=waypoints[backN].position;
                
                front.y = 0;
                mid.y = 0;
                back.y = 0;
                Vector3 line1 = mid - front;
                Vector3 line2 = mid - back;
                float angle = Vector3.Angle(line1, line2);
                waypoints[midN].GetComponent<WaypointAttr>().ways.angle = angle;
                if (angle<=126)
                {
                    waypoints[frontN].GetComponent<WaypointAttr>().ways.befCorner = true;
                    waypoints[midN].GetComponent<WaypointAttr>().ways.corner = true;
                }
                else if(angle<=135)
                {
                    waypoints[midN].GetComponent<WaypointAttr>().ways.corner = true;
                }
            }
        }


        for (int i = 0; i < waypoints.Count-trackPointInterval; i += trackPointInterval) //waypoint위치에 trackpoint생성
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
