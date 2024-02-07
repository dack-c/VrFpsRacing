using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MirrorCameraMovement : MonoBehaviour //플레이어가 거울을 바라보는 위치에 따라 미러카메라의 위치 및 회전 변경
{
    public Transform playerTarget;
    public Transform mirror;

    public float rotateRatio = 1f; //회전을 몇 배로 할 것인지.

    private void Start()
    {
        if (mirror == null)
        {
            mirror = transform.parent;
        }
    }

    void Update()
    {
        //카메라 위치변경
        Vector3 localPlayer = mirror.InverseTransformPoint(playerTarget.position);
        //transform.position = mirror.TransformPoint(new Vector3(localPlayer.x, localPlayer.y, -localPlayer.z));

        //카메라 회전변경
        Vector3 lookTarget = mirror.TransformPoint(new Vector3(-localPlayer.x*rotateRatio, localPlayer.y, localPlayer.z));
        transform.LookAt(lookTarget);
    }
}
