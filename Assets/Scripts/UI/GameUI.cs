using BNG;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    public Text rankingText;
    public Text playerCountText;
    public Text raceTimeText;
    public Text finishTimeText;
    public Text hpText;
    public RaceCountdown StartCountdownUI;
    public List<GameObject> ItemSlotUI;
    public Sprite EmptyItemSprite;

    public Damageable playerDamageable;

    private LapController LapController;
    private List<Image> itemIconDisplay;
    private List<Image> itemBorderDisplay;
    private static List<LapController> playerLaps = new List<LapController>();

    void Start()
    {
        InitGameUI();
    }

    private void InitGameUI()
    {
        LapController = GameManager.I.Player.GetComponent<LapController>();
        playerCountText.text = $"{GameManager.I.Players.Length}";
        for (int i = 0; i < GameManager.I.Players.Length; i++)
        {
            playerLaps.Add(GameManager.I.Players[i]);
        }
        for (int i = 0; i < ItemSlotUI.Count; i++)
        {
            itemIconDisplay.Add(ItemSlotUI[i].transform.GetChild(0).transform.GetChild(0).GetComponent<Image>());
            if (i < GameManager.I.PlayerItemController.selectedItem.Length)
                ChangeSlotIcon(i, GameManager.I.PlayerItemController.selectedItem[i].itemIcon);
            else
                ChangeSlotIcon(i, null);
            itemBorderDisplay.Add(ItemSlotUI[i].transform.GetChild(0).GetComponent<Image>());
        }
        SwitchSelectedSlot(0);
    }

    void Update()
    {
        UpdateRanking();
        UpdateHP();

        if (LapController.isStarted) UpdateRaceTime();
        if (LapController.isFinished) SetFinishTime();
    }


    // Update Game UI
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
        SetTimeText(raceTimeText, lapTimes);
    }

    private void SetFinishTime()
    {
        var lapTimes = LapController.finishLapTime[0];
        SetTimeText(finishTimeText, lapTimes);
    }

    public void SetTimeText(Text text, float time)
    {
        text.text = $"{(int)(time / 60):00}:{time % 60:00.00}";
    }

    private void UpdateHP()
    {
        // Write down the player's HP here
        hpText.text = $"{(int)(playerDamageable.Health)}";
    }


    // Update Item UI

    /// <summary>
    /// Functions to apply UI to selected item slots
    /// </summary>
    /// <param name="slotNum">index number of the selected item slot</param>
    public void SwitchSelectedSlot(int slotNum)
    {
        if (GameManager.I.PlayerItemController.currentSlot == slotNum)
            return;

        GameManager.I.PlayerItemController.currentSlot = slotNum;
        itemBorderDisplay[slotNum].enabled = true;
        for (int i = 0; i < itemBorderDisplay.Count; i++)
            if (i != slotNum)
                itemBorderDisplay[i].enabled = false;
    }

    public void SwitchSelectedSlot()
    {
        int _currentSlot = GameManager.I.PlayerItemController.currentSlot;
        for (int i = 0; i < itemIconDisplay.Count; i++)
        {
            if (i == _currentSlot)
                itemBorderDisplay[i].enabled = true;
            else
                itemBorderDisplay[i].enabled = false;
        }
    }

    /// <summary>
    /// Change the icon of the item slot.
    /// </summary>
    /// <param name="index">Index number of the item slot you want to change </param>
    /// <param name="itemIcon">Item icon to fit into the item slot. To set it to an empty icon, you can put null. </param>
    public void ChangeSlotIcon(int index, Sprite itemIcon)
    {
        if (itemIcon == null)
            itemIconDisplay[index].sprite = EmptyItemSprite;

        itemIconDisplay[index].sprite = itemIcon;
    }
}
