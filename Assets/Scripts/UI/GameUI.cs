using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    public GameObject Player;
    public LapController LapController;
    public Text rankingText;
    public Text playerCountText;
    public Text raceTimeText;
    public Text finishTimeText;
    public Text hpText;

    private static List<LapController> playerLaps = new List<LapController>();

    void Start()
    {
        InitGameUI();
    }

    private void InitGameUI()
    {
        Player = GameManager.I.Player;
        LapController = Player.GetComponent<LapController>();
        playerCountText.text = $"{GameManager.I.Players.Length}";
        for (int i = 0; i < GameManager.I.Players.Length; i++)
        {
            playerLaps.Add(GameManager.I.Players[i]);
        }
    }

    void Update()
    {
        UpdateRanking();
        UpdateHP();

        if (LapController.isStarted) UpdateRaceTime();
        if (LapController.isFinished) SetFinishTime();
    }
    
    private void UpdateRanking()
    {
        SortRanking();
        var playerRank = playerLaps.IndexOf(LapController) + 1;

        rankingText.text = $"{playerRank}";
    }

    private void SortRanking()
    {
        for (int i = 0; i < playerLaps.Count; ++i)
            playerLaps[i].UpdateLapValue();

        playerLaps.Sort((a, b) => a.lapValue.CompareTo(b.lapValue));
    }
    
    private void UpdateRaceTime()
    {
        var lapTimes = LapController.currentLapTime;
        SetTimeText(finishTimeText, lapTimes);
    }

    private void SetFinishTime()
    {
        var lapTimes = LapController.finishLapTime[0];
        SetTimeText(raceTimeText, lapTimes);
    }

    public void SetTimeText(Text text, float time)
    {
        text.text = $"{(int)(time / 60):00}:{time % 60:00.00}";
    }

    private void UpdateHP()
    {
        hpText.text = $"100"; // Write down the player's HP here
    }
}
