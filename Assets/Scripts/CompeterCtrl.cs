using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompeterCtrl : MonoBehaviour
{

    public List<Transform> location;
    public bool StartSign = false;

    // Start is called before the first frame update
    void Start()
    {
        foreach(Transform t in gameObject.GetComponentInChildren<Transform>())
        {
            location.Add(t);
        }
    }

    public void startOff()
    {
        StartSign = true;
    }

    public void checkerFlag()
    {
        StartSign = false;
    }
}
