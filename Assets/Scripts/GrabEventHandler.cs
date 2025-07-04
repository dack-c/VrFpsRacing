using BNG;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GrabEventHandler : MonoBehaviour
{
    // Start is called before the first frame update
    public Grabbable grabbable;

    public UnityEvent onBeingGrabbed;
    public UnityEvent onBeingReleased;

    private bool preGrabState = false;

    void Start()
    {
        if(grabbable == null)
        {
            grabbable = gameObject.GetComponent<Grabbable>();
        }
        preGrabState = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(grabbable.BeingHeld && preGrabState == false)
        {
            preGrabState = true;
            onBeingGrabbed.Invoke();
        }
        else if(!(grabbable.BeingHeld) && preGrabState == true)
        {
            preGrabState = false;
            onBeingReleased.Invoke();
        }
    }
}
