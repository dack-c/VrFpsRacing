using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LapController : MonoBehaviour
{
    public bool isPlayer = false;
    public int currentLaps = 1;
    public int currentTrackpointIndex = 0;

    public bool isStarted = false;
    public bool isFinished = false;

    public LapValue lapValue;

    public float currentLapTime = 0.0f;
    public List<float> finishLapTime = new List<float>();

    public Transform Transform;

    private void Start()
    {
        Transform = GetComponent<Transform>();
        for (int i = 0; i < GameManager.I.CurrentTrack.finishLaps; i++)
            finishLapTime.Add(0);
    }

    public void UpdateLapValue()
    {
        if (isStarted && !isFinished)
        {
            currentLapTime += Time.deltaTime;
            var passedTrackpoint = GameManager.I.CurrentTrack.trackpoints[currentTrackpointIndex].transform.position;
            lapValue = new LapValue
            {
                lap = currentLaps,
                trackpointIndex = currentTrackpointIndex,
                trackpointDistance = Vector3.Distance(Transform.position, passedTrackpoint)
            };
        }
    }

    public void ProcessTrackpoint(Trackpoint trackpoint)
    {
        if (!isStarted) return;
        if (currentTrackpointIndex == trackpoint.index - 1 || currentTrackpointIndex == trackpoint.index + 1)
        {
            Debug.Log($"{this} has collided with trackpoint{trackpoint.index}");
            currentTrackpointIndex = trackpoint.index;
        }
    }

    public void ProcessFinishline()// finishLine통과 시, Finishline obj에서 들어온 car(labcontroller)의 이 함수를 호출
    {
        if (!isStarted) return;
        if (currentTrackpointIndex == GameManager.I.CurrentTrack.trackpoints.Length - 1)
        {
            finishLapTime[currentLaps - 1] = currentLapTime;
            currentLaps++;
            Debug.Log($"Lap: {currentLaps}");
            if (currentLaps > GameManager.I.CurrentTrack.finishLaps && !isFinished)
            {
                Debug.Log($"");
                isFinished = true; //해당 car를 완주 상태로 변경
                GameManager.I.CurrentTrack.UpdateFinishedCarNum(); //완주한 차량 개수들 업데이트
                if (isPlayer)//들어온게 플레이어면 게임종료
                {
                    GameManager.I.CurrentTrack.EndGame(Track.Result.Finish);
                }
            }
            currentTrackpointIndex = -1;
        }
    }
}
