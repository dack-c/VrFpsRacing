using BNG;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class AIWeaponCtrl : MonoBehaviour
{
    public Transform ai;
    public RaycastWeapon RW;
    private Vector3 player_pos;
    private Vector3 ai_pos;
    public Vector3 ToTarget;
    private float angle;
    public float aimingRange=1.2f;
    public float fireRange = 1.0f;
    private float timer = 0;
    public List<Transform> target;
    public CompeterCtrl comp;
    public int targetNum=-1;
    public float realDist;


    // Start is called before the first frame update
    void Start()
    {
        target = comp.location;
    }

    // Update is called once per frame
    void Update()
    {
        ai_pos = ai.position;
        float dist=RW.MaxRange*aimingRange;

        for(int i=0; i<target.Count; i++)
        {
            Vector3 instTarget = target[i].position - ai.position;
            float curDist = instTarget.magnitude;
            if(targetNum==-1)
            {
                targetNum = i;
                dist = curDist;
            }
            else if(dist>curDist&&curDist>2.5f)
            {
                dist = curDist;
                targetNum = i;
            }
        }
       
        
        
        timer += Time.deltaTime;

        if(timer>=1)
        {
            ToTarget = target[targetNum].position - ai_pos;
            ToTarget.y += 0.5f;
            realDist = ToTarget.magnitude;
            Vector3 random = new Vector3(Random.Range(0.0f,1.0f)-0.5f, Random.Range(0.0f,1.0f)-0.5f,Random.Range(0.0f,1.0f)-0.5f);
            ToTarget += random;
            
        }
        Debug.DrawRay(transform.position, ToTarget, Color.yellow);
        
        if (dist<RW.MaxRange*aimingRange)
        {
            ai.GetComponent<Transform>().rotation = Quaternion.LookRotation(ToTarget);
        }
        if(dist<=RW.MaxRange*fireRange)
        {
            
            if (timer >= 1)
            {
                
                timer = 0;
                RW.Shoot();
            }
        }
        
    }
}
