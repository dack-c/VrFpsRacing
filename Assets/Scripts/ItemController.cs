using System.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;

public class ItemController : MonoBehaviour
{
    public ItemDefinition[] itemList;   // List of all items used in the game
    public ItemDefinition noItem;       // Dummy object for Null Item
    public ItemDefinition[] selectedItem = new ItemDefinition[4];       // inventory

    public int currentSlot = 0;

    public void Start()
    {
        InitItemController();
    }

    public void InitItemController()
    {
        for (int i = 0; i < selectedItem.Length; i++)
        {
            if (!selectedItem[i])
                selectedItem[i] = noItem;
        }
    }

    /// <summary>
    /// Call when using an item in the current slot.
    /// </summary>
    public void UseItem()
    {
        if (selectedItem[currentSlot].Use())
            CleanCurrentSlot();
    }

    /// <summary>
    /// Called when dropping an item in the currently selected item slot into the world space.
    /// </summary>
    public void DropItem()
    {
        // 이곳에 월드에 아이템을 스폰하는 함수 작성

        CleanCurrentSlot();
    }

    /// <summary>
    /// Call to remove an item from the currently selected item slot.
    /// </summary>
    public void CleanCurrentSlot()
    {
        selectedItem[currentSlot] = noItem;
        GameManager.I.Hud.ChangeSlotIcon(currentSlot, null);
    }

    /// <summary>
    /// Call when you get items that are dropped in the world space.
    /// Returns the index number of the slot in which the item was obtained. 
    /// And returns -1 if the item was not obtained because the inventory was full.
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    public int GetDropedItem(ItemDefinition item)
    {
        for (int i = 0; i < selectedItem.Length; i++)
        {
            if (selectedItem[i] == noItem)
                continue;
            else
            {
                selectedItem[i] = item;
                GameManager.I.Hud.ChangeSlotIcon(i, item.itemIcon);

                // 이곳에 플레이어에게 아이템을 장착시키는 함수 작성

                return i;
            }
        }

        return -1;
    }

    /// <summary>
    /// Use to select the index-th item slot
    /// </summary>
    /// <param name="index"></param>
    public void SwitchItem(int index)
    {
        // return if there is no change in the existing slot
        if (currentSlot == index)
            return;

        // 이곳에 플레이어가 장착한 아이템을 장착 해제하는 함수 작성

        currentSlot = index;

        if (currentSlot < 0)
            currentSlot = selectedItem.Length - 1;
        else if (currentSlot >= selectedItem.Length)
            currentSlot = 0;

        GameManager.I.Hud.SwitchSelectedSlot();

        if (selectedItem[currentSlot].isEquipable)
        {
            // 이곳에 플레이어에게 아이템을 장착시키는 함수 작성
        }
    }

#if UNITY_EDITOR
    // for Item Slot Debug
    public void OnGUI()
    {
        if (GUI.Button(new Rect(100, 150, 100, 30), "Slop Left"))
        {
            SwitchItem(currentSlot - 1);
        }
        if (GUI.Button(new Rect(200, 150, 100, 30), "Slot Right"))
        {
            SwitchItem(currentSlot + 1);
        }
        if (GUI.Button(new Rect(300, 150, 100, 30), "Drop Item"))
        {
            DropItem();
        }
        if (GUI.Button(new Rect(400, 150, 100, 30), "Use Item"))
        {
            UseItem();
        }
    }
#endif
}
