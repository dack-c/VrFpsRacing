using System.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;

public class ItemController : MonoBehaviour
{
    public ItemDefinition[] itemList;   // List of all items used in the game
    public ItemDefinition noItem;       // Dummy object for Null Item
    public ItemDefinition[] selectedItem = new ItemDefinition[4];       // inventory

    public int currentSlot = 0;

    public void Init()
    {
        for (int i = 0; i < selectedItem.Length; i++)
            selectedItem[i] = noItem;
    }

    public void UseItem()
    {
        if (selectedItem[currentSlot].Use())
            ExpiredItem();
    }

    public void ExpiredItem()
    {
        selectedItem[currentSlot] = noItem;
        GameManager.I.Hud.ChangeSlotIcon(currentSlot, null);
    }

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
                return i;
            }
        }

        return -1;
    }

    public void SwitchItem(int index)
    {
        // return if there is no change in the existing slot
        if (currentSlot == index)
            return;

        // 플레이어가 장착한 아이템을 장착 해제하는 함수

        currentSlot = index;

        if (currentSlot < 0)
            currentSlot = selectedItem.Length - 1;
        else if (currentSlot >= selectedItem.Length)
            currentSlot = 0;

        GameManager.I.Hud.SwitchSelectedSlot();

        if (selectedItem[currentSlot].isEquipable)
        {
            // 플레이어에게 장착시키는 함수
        }
    }

#if UNITY_EDITOR
    // for Item Slot Debug
    public void OnGUI()
    {
        if (GUI.Button(new Rect(100, 150, 100, 30), "item_Left"))
        {
            SwitchItem(currentSlot - 1);
        }
        if (GUI.Button(new Rect(200, 150, 100, 30), "item_Right"))
        {
            SwitchItem(currentSlot + 1);
        }
    }
#endif
}
