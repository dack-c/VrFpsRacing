using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompeterCtrl : MonoBehaviour
{

    public List<Transform> location;
    public bool StartSign = false;

    // Start is called before the first frame update
    void Awake()
    {
        foreach(Transform t in gameObject.GetComponentInChildren<Transform>())
        {
            if (t.gameObject.activeSelf)
            {
                location.Add(t);
            }
        }
    }

    private void Update()
    {
        checkPlayers();
    }

    public void startOff()
    {
        StartSign = true;
    }

    public void checkerFlag()
    {
        StartSign = false;
    }

    public void checkPlayers()
    {
        /*foreach(Transform t in location)
        {
            if(!t.gameObject.activeSelf)
            {
                location.Remove(t);
            }
        }*/
        //위 로직 사용시, foreach문을 돌리고 있는 중에 list안에 있는 요소들이 삭제가 되면 예외 발생.
        //참고 자료: https://ruen346.tistory.com/109#google_vignette

        for (int i = location.Count - 1; i >= 0; i--)
        {
            if (!location[i].gameObject.activeSelf)
            {
                location.RemoveAt(i);
            }
        }
    }
}
