using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public class GameEnd : MonoBehaviour
{
    public struct Labtime
    {
        public float time;
        public string name;
    }
    public int finished; //몇명이 들어왔는지
    public int rank;
    public float Limittime;
    public bool enter;
    public float ltime;
    public Labtime[] labtime;
    
    private void Start()
    {
        finished = 0;
        Limittime = 60;
        ltime = Limittime;
        enter = false;
        rank = -1;
        labtime = new Labtime[4];
    }
    private void Update()
    {
        
        if(enter)
        {
            Limittime = Limittime - Time.deltaTime;
            if (ltime - Limittime > 1)
            {
                Debug.Log(ltime);
                ltime--;
            }
                
        }
        if (Limittime < 0) Endgame();
        if (finished == 3) Endgame(); //플레이어가 들어오거나 죽으면 바로 끝남. 이 코드 잘못됨
    }
    public void FinishEnter(string name)
    {
        labtime[finished].time = Time.time;
        labtime[finished].name=name;
        finished++;
        enter = true;

    }
    public void Endgame()
    {
        //rank가 -1 이면 리타이어, 4 이상이면 패배
        Debug.Log("game end \nrank:"+rank);
        Debug.Log(labtime[0].name + "  " + labtime[0].time);
        Debug.Log(labtime[1].name + "  " + labtime[1].time);
        Debug.Log(labtime[2].name + "  " + labtime[2].time);

    }

}
