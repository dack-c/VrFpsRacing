using BNG;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Slide �� �ڽ� ������Ʈ�� kinmatic�鵵 �� �θ� �°� �����Ű�� ����
public class UpdateChildPhysics : MonoBehaviour
{
    public Grabbable grabbable;

    public bool isHeldInPre; //�̹� ����������, ���� ����� ����
    // Start is called before the first frame update
    void Start()
    {
        grabbable = GetComponent<Grabbable>();
        isHeldInPre = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(grabbable.BeingHeld && !isHeldInPre)
        {
            isHeldInPre = true;
            UpdateAllchildKinematic(true);
        }
        else if(!grabbable.BeingHeld && isHeldInPre)
        {
            isHeldInPre = false;
            UpdateAllchildKinematic(false);
        }
    }

    private void UpdateAllchildKinematic(bool kinematicSetting)
    {
        foreach (Transform child in transform)
        {
            Rigidbody rb = child.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = kinematicSetting;
            }
        }
    }
}
