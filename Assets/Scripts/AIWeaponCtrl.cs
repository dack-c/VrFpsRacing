using BNG;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class AIWeaponCtrl : MonoBehaviour
{

    
    public Transform ai;
    public RaycastWeapon RW;
    public CompeterCtrl comp;
    public AI aiC;
    public CheckIfUs CIU;

    [System.Serializable]
    public class stat
    {
        public Vector3 ToTarget;//조준 위치
        public float realDist;//타겟과의 거리
        public int targetNum=-1;//타겟 번호
    }
    public stat status;

    [System.Serializable]
    public class setting
    {
        public float aimingRange=1.2f;//조준 시작 거리. 무기 사정거리 * aimingRange
        public float fireRange = 1.0f;//사격 시작 거리. 무기 사정거리 * fireRange
        public float aimIssue = 5.0f; //사격시 조준 오차범위
    }
    public setting set;

    private Vector3 ai_pos;
    
    private float angle;
    
    private float timer = 0;
    
    
    
    


    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        ai_pos = ai.position;
        
        
        
        timer += Time.deltaTime;

        //타겟 조준
        if(timer>=1)
        {
            status.ToTarget = aiC.status.targets[aiC.status.targetNum].position - ai_pos;
            status.ToTarget.y += 0.5f;
            status.realDist = status.ToTarget.magnitude;
            //set.aimIssue *= (aiC.status.realDist / 10);
            Vector3 random = new Vector3(Random.Range(0.0f,set.aimIssue)-set.aimIssue/2, Random.Range(0.0f,set.aimIssue)-set.aimIssue/2,Random.Range(0.0f, set.aimIssue) - set.aimIssue / 2);
            status.ToTarget += random;
            if (aiC.status.realDist<RW.MaxRange*set.aimingRange)
            {
                ai.GetComponent<Transform>().rotation = Quaternion.LookRotation(status.ToTarget);
            }    
        }
        Debug.DrawRay(transform.position, status.ToTarget, Color.yellow);

        //사격
        
        if(aiC.status.realDist <= RW.MaxRange*set.fireRange)
        {
            if (timer >= 1&&comp.StartSign)
            {
                bool isParent = false;
                isParent = CIU.isAimingSelf();//자기 자신을 조준 중인지 확인

                if (!isParent)
                {
                    RW.Shoot();
                    timer = 0;
                }
                else
                {
                    //Debug.Log(transform.parent.name + "이 자신을 겨눴음");
                }
                //Debug.Log("Shoot");
            }
        }
        
    }
}
