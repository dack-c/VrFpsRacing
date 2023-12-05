using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public bool WinVar = false;
    public bool LoseVar = false;
    public bool RetireVar = false;
    public TMP_Text HeaderTMP;
    public TMP_Text FirstPlaceTMP;
    public TMP_Text SecondPlaceTMP;
    public TMP_Text ThirdPlaceTMP;
    public TMP_Text XthPlaceTMP;
    GameObject obj;
    public GameObject leftPanel;
    void Start()
    {
        obj = GameObject.Find("Finishline");
    }
    void Update()
    {
        leftPanel.SetActive(true);
        Debug.Log(obj.GetComponent<GameEnd>().finished);

        FirstPlaceTMP.text = "1st : " + obj.GetComponent<GameEnd>().labtime[0].name + " / " + obj.GetComponent<GameEnd>().labtime[0].time;
        SecondPlaceTMP.text = "2nd : " + obj.GetComponent<GameEnd>().labtime[1].name + " / " + obj.GetComponent<GameEnd>().labtime[1].time;
        ThirdPlaceTMP.text = "3rd : " + obj.GetComponent<GameEnd>().labtime[2].name + " / " + obj.GetComponent<GameEnd>().labtime[2].time;
        if (obj.GetComponent<GameEnd>().rank == -1) //rank가 -1이면 리타이어
        {
            HeaderTMP.text = "Retire :(";
            leftPanel.SetActive(false);
        }
        else if (obj.GetComponent<GameEnd>().rank >= 4) //rank가 4 이상이면 패배
        {
            HeaderTMP.text = "Lose...";
            XthPlaceTMP.enabled = true;
            XthPlaceTMP.text = (obj.GetComponent<GameEnd>().rank).ToString() + "th : MyCar / ";
        }
        else // 위 조건 중 맞는게 없으면 승리
        {
            HeaderTMP.text = "Win!";
            XthPlaceTMP.enabled = false;
        }
    }
}
