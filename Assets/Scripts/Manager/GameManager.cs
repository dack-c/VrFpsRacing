using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager I { get; private set; }

    public GameObject Player;
    public LapController[] Players;
    public Track CurrentTrack;
    public CompeterCtrl CompeterCtrl; //ai차량에 출발신호를 주기 위해(startSign = true)

    // Start is called before the first frame update
    void Awake()
    {
        if (I)
        {
            Destroy(gameObject);
            return;
        }
        I = this;
        DontDestroyOnLoad(gameObject);
    } 
    
    // Update is called once per frame
    void Update()
    {
        
    }
}
