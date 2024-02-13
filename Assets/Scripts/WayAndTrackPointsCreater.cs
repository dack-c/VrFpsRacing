using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EasyRoads3Dv3;
using System.Drawing;
using System;
using UnityEditor.Rendering.LookDev;

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

    public float trackPointCreationAngle = 70f; //이 각도 이상으로 꺾어지는 코너면 트랙포인트 생성

    public GameObject carsParent;

    // Start is called before the first frame update
    void Awake()
    {

        
        //1. 웨이포인트 생성

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
                //waypointObj.transform.parent = wayPointParent.transform;
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


        //2. 경주 시작점에 맞춰 waypoints 순서 재정렬

        //2.1) finishLine기준으로 가장 멀리 있는 차 구하기
        float longestDistance = 0f;
        Transform frontmostCar = null;
        for (int i = 0; i < carsParent.transform.childCount; i++)
        {
            Transform car = carsParent.transform.GetChild(i);
            float curDistance = Vector3.Distance(finishLineObj.transform.position, car.position);

            if (curDistance > longestDistance) // 위에서 잡은 기준으로 거리 재기
            {
                frontmostCar = car;
                longestDistance = curDistance;
            }
        }

        //2.2) 가장 멀리있는 차의 앞에 있는 웨이포인트 중에 가장 가까운 웨이포인트 찾기
        int closestWaypointIndex = FindClosestAndForwardWaypointIndexFrom(frontmostCar);

        // 2.3) 이 웨이포인트를 기준으로 waypoints와 rightEdgeForWaypoints를 재정렬
        List<Transform> tempList = waypoints.GetRange(closestWaypointIndex, waypoints.Count - closestWaypointIndex);
        waypoints.RemoveRange(closestWaypointIndex, waypoints.Count - closestWaypointIndex);
        tempList.AddRange(waypoints);
        waypoints = tempList;

        List<Vector3> tempList2 = rightEdgeForWaypoints.GetRange(closestWaypointIndex, rightEdgeForWaypoints.Count - closestWaypointIndex);
        rightEdgeForWaypoints.RemoveRange(closestWaypointIndex, rightEdgeForWaypoints.Count - closestWaypointIndex);
        tempList2.AddRange(rightEdgeForWaypoints);
        rightEdgeForWaypoints = tempList2;

        // 2.4) 재정렬된 waypoint들을 부모에 귀속
        foreach (Transform t in waypoints)
        {
            t.SetParent(wayPointParent.transform);
        }


        // 3. 자동 코너 지정
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
                int midN = i + 1;
                if (i + 1 >= waypoints.Count)
                {
                    midN -= waypoints.Count;
                }

                float angle = CaculateWaypointAngle(i, false);
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


        //4. 트랙포인트 생성 로직
        float reverseAngleSum = 0f;
        for(int i = 0; i < waypoints.Count - 1; i++)
        {
            float angle = CaculateWaypointAngle(i, true);
            float reverseAngle; //꺾는 각도
            if(angle >= 0) //각도가 양수라면
            {
                reverseAngle = 180 - angle;
            }
            else
            {
                reverseAngle = -180 - angle;
            }
            reverseAngleSum += reverseAngle;
            if(Mathf.Abs(reverseAngleSum) >= trackPointCreationAngle)//트랙포인트 생성
            {
                GameObject trackpointObj = Instantiate(trackpointPrefab, waypoints[i+1].position, Quaternion.identity);
                trackpointObj.transform.LookAt(rightEdgeForWaypoints[i+1]);
                trackpointObj.transform.Rotate(0, 90, 0);
                trackpointObj.transform.Translate(0, 3.5f, 0);
                trackpoints.Add(trackpointObj.transform);
                trackpointObj.transform.parent = trackPointParent.transform;
                reverseAngleSum = 0f;
            }
        }

        /*for (int i = 0; i < waypoints.Count-trackPointInterval; i += trackPointInterval) //waypoint위치에 trackpoint생성
        {
            GameObject trackpointObj = Instantiate(trackpointPrefab, waypoints[i].position, Quaternion.identity);
            trackpointObj.transform.LookAt(rightEdgeForWaypoints[i]);
            trackpointObj.transform.Rotate(0, 90, 0);
            trackpointObj.transform.Translate(0, 3.5f, 0);
            trackpoints.Add(trackpointObj.transform);
            trackpointObj.transform.parent = trackPointParent.transform;
        }*/
    }

    private int FindClosestAndForwardWaypointIndexFrom(Transform origin)
    {
        float closestDistance = 10000f;
        int closestIndex = 0;
        for(int i = 0; i < waypoints.Count; i++)
        {
            Vector3 directionToWaypoint = waypoints[i].position - origin.position;
            float angle = Vector3.Angle(origin.forward, directionToWaypoint);
            if(angle < 90)//이 웨이포인트가 origin의 앞에 있을 경우
            {
                float distance = Vector3.Distance(origin.position, waypoints[i].position);
                if(distance < closestDistance)
                {
                    closestIndex = i;
                    closestDistance = distance;
                }
            }
        }
        return closestIndex;
    }

    /*private int FindClosestAndForwardWaypointIndexFrom(Transform origin)// origin 앞에 있는 웨이포인트들 중에서 가장 가까운 웨이포인트 찾기 
    {
        int result = 0;
        BinarySearchForClosestAndForwardWaypoint(origin, 0, waypoints.Count-1, ref result);
        return result;
    }

    private void BinarySearchForClosestAndForwardWaypoint(Transform origin, int left, int right, ref int result)
    {
        if(left > right)
        {
            return;
        }

        int center = (left + right) / 2;
        Vector3 directionToWaypoint = waypoints[center].position - origin.position;
        float angle = Vector3.Angle(origin.forward, directionToWaypoint);
        if (angle < 90)//이 웨이포인트가 origin의 앞에 있을 경우
        {
            result = center;
            BinarySearchForClosestAndForwardWaypoint(origin, left, center-1, ref result); //앞에 있으면서 더 가까운 웨이포인트들이 있는지 탐색
        }
        else
        {
            BinarySearchForClosestAndForwardWaypoint(origin, center+1, right, ref result); //일단 뒤에 있으므로 앞에 있는 웨이포인트들을 향해 탐색
        }
    }*/

    private float CaculateWaypointAngle(int frontIndex, bool isReturnSigned)//이 index의 다음 index와, 그 다음 index 사이의 각도를 구하는 함수
    {
        int frontN = frontIndex;
        int midN = frontIndex + 1;
        int backN = frontIndex + 2;
        if (frontIndex + 1 >= waypoints.Count)
        {
            midN -= waypoints.Count;
        }
        if (frontIndex + 2 >= waypoints.Count)
        {
            backN -= waypoints.Count;
        }

        Vector3 front = waypoints[frontN].position;
        Vector3 mid = waypoints[midN].position;
        Vector3 back = waypoints[backN].position;

        front.y = 0;
        mid.y = 0;
        back.y = 0;
        Vector3 line1 = mid - front;
        Vector3 line2 = mid - back;

        float angle;
        if(isReturnSigned)
        {
            angle = Vector3.SignedAngle(line1, line2, line1);
        }
        else
        {
            angle = Vector3.Angle(line1, line2);
        }
        

        return angle;
    }

    public static float GetAngleSigned(Vector3 vStart, Vector3 vEnd) //두 벡터 사이의 각도: -180~180
    {
        Vector3 v = vEnd - vStart;

        return Mathf.Atan2(v.y, v.x) * Mathf.Rad2Deg;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
