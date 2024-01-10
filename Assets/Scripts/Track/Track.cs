using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Track : MonoBehaviour
{
    public int finishLaps; //목표 랩수
    public Trackpoint[] trackpoints;
    public Finishline finishline;
    public GameObject trackpointParent;

    public Text resultUIText;
    public LapController playerLabController;

    private int finishedCarNum = 0; //완주한 차량 개수
    //private int finalPlayerRank; //플레이어 최종순위
    private float finalFinishTime; //플레이어의 완주 시간
    private int destroyedEnemyNum = 0; //현재 파괴된 차량 개수
    private string resultStr; //게임 결과

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
            StartCoroutine(EndGame(Result.Allkill));
        }
    }

    public void UpdateFinishedCarNum() //완주한 car(labcontroller)가 호출
    {
        finishedCarNum++;
    }

    public IEnumerator EndGame(Result result) //결과창 뜨고 약 3초후 메인화면으로 데이터와 함께 넘겨주기
    {
        float uiChangeDelay = 3.0f;
        resultUIText.enabled = true; //결과창 ui 텍스트 활성화
        switch (result)
        {
            case Result.Finish:
                finalFinishTime = playerLabController.currentLapTime;
                resultStr = $"{finishedCarNum}위";

                resultUIText.text = "Finish!";
                yield return new WaitForSeconds(uiChangeDelay);

                resultUIText.text = resultStr;
                break;
            case Result.Allkill:
                finalFinishTime = playerLabController.currentLapTime;
                resultStr = "All Kill!";

                resultUIText.text = resultStr;
                break;
            case Result.Retire:
                finalFinishTime = -1.0f; //-1은 리타이어 했다는 의미
                resultStr = "Retire";

                resultUIText.text = resultStr;
                break;
        }

        //기록 저장
        Record record = new Record
        {
            dateTime = DateTime.Now.ToString(),
            result = resultStr,
            labTime = finalFinishTime,
            destroyedCarNum = destroyedEnemyNum
        };
        DataManager.I.SaveRecordData(record);

        //몇 초 후 메인씬으로 돌아가기
        yield return new WaitForSeconds(uiChangeDelay);
        SceneManager.LoadScene("MainScene");
    }
}
