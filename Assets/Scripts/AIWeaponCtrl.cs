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
        public float aimIssue = 2.0f; //사격시 조준 오차범위
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
        float dist=RW.MaxRange*set.aimingRange;
       
        
        
        timer += Time.deltaTime;

        //타겟 조준
        if(timer>=1)
        {
            status.ToTarget = aiC.status.targets[aiC.status.targetNum].position - ai_pos;
            status.ToTarget.y += 0.5f;
            status.realDist = status.ToTarget.magnitude;
            Vector3 random = new Vector3(Random.Range(0.0f,set.aimIssue)-set.aimIssue/2, Random.Range(0.0f,set.aimIssue)-set.aimIssue/2,Random.Range(0.0f, set.aimIssue) - set.aimIssue / 2);
            status.ToTarget += random;
            if (dist<RW.MaxRange*set.aimingRange)
            {
                ai.GetComponent<Transform>().rotation = Quaternion.LookRotation(status.ToTarget);
            }    
        }
        Debug.DrawRay(transform.position, status.ToTarget, Color.yellow);
        
        //사격
        if(dist<=RW.MaxRange*set.fireRange)
        {
            
            if (timer >= 1&&comp.StartSign)
            {
                
                timer = 0;
                RW.Shoot();
                //Debug.Log("Shoot");
            }
        }
        
    }
}
