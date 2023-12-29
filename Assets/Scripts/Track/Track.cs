using UnityEngine;

public class Track : MonoBehaviour
{
    public int finishLaps; //목표 랩수
    public Trackpoint[] trackpoints;
    public Finishline finishline;
    public GameObject trackpointParent;

    private int finishedCarNum = 0; //완주한 차량 개수
    private int finalPlayerRank; //플레이어 최종순위
    private float finalFinishTime; //플레이어의 완주 시간
    private int destroyedEnemyNum = 0;

    public enum Result //게임 결과
    {
        Finish, //n위로 완주
        Allkill, //모든 차 폭파시킴
        Retire //사망
    }
    private void Start()
    {
        InitTrackpoints();
        finishedCarNum = 0;
        destroyedEnemyNum = 0;
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

    public void CheckDestroyedEnemyNum() //폭파된 차량이 호출
    {
        destroyedEnemyNum++;
        if(destroyedEnemyNum == GameManager.I.Players.Length - 1) //모든 적 차량 파괴 시
        {
            EndGame(Result.Allkill);
        }
    }

    public void UpdateFinishedCarNum() //완주한 car(labcontroller)가 호출
    {
        finishedCarNum++;
    }

    //구현 중
    public void EndGame(Result result) //결과창 뜨고 약 3초후 메인화면으로 데이터와 함께 넘겨주기
    {
        switch(result)
        {
            case Result.Finish:
                finalPlayerRank = finishedCarNum;
                finalFinishTime = Time.time;
                break;
            case Result.Allkill:
                finalFinishTime = Time.time;
                break;
            case Result.Retire:
                break;
        }
    }
}
