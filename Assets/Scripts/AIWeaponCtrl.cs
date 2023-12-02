using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIWeaponCtrl : MonoBehaviour
{
    public Transform ai;
    private Vector3 player_pos;
    private Vector3 ai_pos;
    private Vector3 ToTarget;
    private float angle;
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
        ai.GetComponent<Transform>().rotation = Quaternion.LookRotation(ToTarget);
    }
}
