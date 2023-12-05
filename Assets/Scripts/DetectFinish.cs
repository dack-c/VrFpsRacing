using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectFinish : MonoBehaviour
{
    GameObject obj;
    bool finished = false;
    private void Start()
    {
        obj = GameObject.Find("Finishline");
        
    }
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.name=="Finishline" && finished==false){
            Debug.Log("player finished");
            obj.GetComponent<GameEnd>().FinishEnter("Mycar");
            obj.GetComponent<GameEnd>().rank= obj.GetComponent<GameEnd>().finished;
            finished = true;
        }
    }
}
