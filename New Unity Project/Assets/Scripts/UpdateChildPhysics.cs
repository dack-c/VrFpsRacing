using BNG;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Slide 등 자식 오브젝트의 kinmatic들도 다 부모에 맞게 변경시키기 위함
public class UpdateChildPhysics : MonoBehaviour
{
    public Grabbable grabbable;

    public bool isHeldInPre; //이미 변경했으면, 로직 재수행 방지
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
