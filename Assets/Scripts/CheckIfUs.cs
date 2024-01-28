using BNG;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckIfUs : MonoBehaviour
{

    public RaycastWeapon RW;

    public bool isAimingSelf()
    {

        RaycastHit hit;
        if (Physics.Raycast(RW.MuzzlePointTransform.position, RW.MuzzlePointTransform.forward,out hit,RW.MaxRange,RW.ValidLayers, QueryTriggerInteraction.Ignore))
        {
            Transform hitTransform = hit.collider.transform;
            Damageable d = null;
            while (d == null && hitTransform != null)
            {
                d = hitTransform.GetComponent<Damageable>();
                hitTransform = hitTransform.parent;
            }
            Transform parentTransform = transform.parent;
            //if (d != null)
            //{
            //    Debug.Log(parentTransform.name + " and " + d.transform.name);
            //}
            if (d != null)
            {
                if (parentTransform == d.transform)
                {
                    return true;
                }
            }
        }

        return false;
    }
}
