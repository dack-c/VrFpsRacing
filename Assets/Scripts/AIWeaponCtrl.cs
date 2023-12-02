using BNG;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIWeaponCtrl : MonoBehaviour
{
    public Transform ai;
    public RaycastWeapon RW;
    private Vector3 player_pos;
    private Vector3 ai_pos;
    private Vector3 ToTarget;
    private float angle;
    public float aimingRange=1.2f;
    public float fireRange = 1.0f;
    private float timer = 0;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        player_pos = GameObject.FindWithTag("Player").GetComponent<Transform>().position;
        ai_pos = ai.position;

        ToTarget = player_pos- ai_pos;
        float dist = ToTarget.magnitude;
        if(dist<=RW.MaxRange*aimingRange)
        {
            ai.GetComponent<Transform>().rotation = Quaternion.LookRotation(ToTarget);
        }
        if(dist<=RW.MaxRange*fireRange)
        {
            timer += Time.deltaTime;
            if (timer >= 1)
            {
                timer = 0;
                RW.Shoot();
            }
        }
        
    }
}
