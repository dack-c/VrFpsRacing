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
    public GameUI Hud;
    public List<ItemDefinition> SelectedItem;

    // For now, the index number of the selected item slot is temporarily written in 'GameManager',
    // but after that, an 'ItemController' is created to manage the item and managed there.
    public int currentItemSlotIndex = 0; 

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
}
