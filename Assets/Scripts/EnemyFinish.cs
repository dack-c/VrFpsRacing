using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFinish : MonoBehaviour
{
    GameObject obj;
    bool finished = false;
    private void Start()
    {
        obj = GameObject.Find("Finishline");

    }
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.name == "Finishline" && finished==false)
        {
            Debug.Log("enemyfinished");
            obj.GetComponent<GameEnd>().FinishEnter(this.gameObject.name);
            finished = true;
        }
    }
}
