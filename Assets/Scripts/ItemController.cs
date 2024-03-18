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
        selectedItem[currentSlot].Use();
    }

    public void ExpiredItem()
    {
        selectedItem[currentSlot] = noItem;
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
                return i;
            }
        }

        return -1;
    }

    public void SwitchItem(int index)
    {
        currentSlot = index;

        if (currentSlot < 0)
            currentSlot = selectedItem.Length - 1;
        else if (currentSlot >= selectedItem.Length)
            currentSlot = 0;

        GameManager.I.Hud.SwitchSelectedSlot();
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
