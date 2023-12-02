using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompeterCtrl : MonoBehaviour
{

    public List<Transform> location;
    // Start is called before the first frame update
    void Start()
    {
        foreach(Transform t in GetComponentInChildren<Transform>())
        {
            location.Add(t);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
