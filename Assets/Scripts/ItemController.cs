using BNG;
using System.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;

public class ItemController : MonoBehaviour
{
    public List<Grabber> Grabbers = new List<Grabber>();
    public ItemDefinition[] itemList;   // List of all items used in the game
    public ItemDefinition noItem;       // Dummy object for Null Item
    public ItemDefinition[] selectedItem = new ItemDefinition[4];       // inventory
    public GameObject[] itemObjectSlot = new GameObject[4];
    
    public int currentSlot = 0;

    public void Start()
    {
        InitItemController();
    }

    public void InitItemController()
    {
        for (int i = 0; i < selectedItem.Length; i++)
        {
            if (selectedItem[i].item == ItemDefinition.Item.None)
                itemObjectSlot[i] = null;
            else
            {
                itemObjectSlot[i] = Instantiate(selectedItem[i].ItemPrefab);
            }
        }
    }

    /// <summary>
    /// Call to remove an item from the currently selected item slot.
    /// </summary>
    public void CleanCurrentSlot()
    {
        selectedItem[currentSlot] = noItem;
        itemObjectSlot[currentSlot] = null;
        GameManager.I.Hud.ChangeSlotIcon(currentSlot, null);
    }
    
    /// <summary>
    /// Add the "item" in the "index"-th item slot when called.
    /// </summary>
    /// <param name="item"></param>
    /// <param name="index"></param>
    public void AddItemToSlot(GameObject item, int index)
    {
        RaceItem raceItem = item.GetComponent<RaceItem>();
        if (selectedItem[index].item == ItemDefinition.Item.None)
        {
            GameManager.I.Hud.ChangeSlotIcon(index, raceItem.itemDefinition.itemIcon);
            selectedItem[currentSlot] = raceItem.itemDefinition;
            itemObjectSlot[currentSlot] = item;
        }
        else
            Debug.Log($"There is already an item in the index-th item slot:{index}");
    }

    /// <summary>
    /// Call when you get items that are dropped in the world space.
    /// First check whether the current slot is empty, then check whether there are other empty slots in order from index 0.
    /// If an empty slot is found, the index value of the corresponding slot is returned.
    /// If there is no empty slot, return -1.
    /// </summary>
    /// <returns></returns>
    public int CheckEmptyIndex()
    {
        // check the current slot
        if (selectedItem[currentSlot].item == ItemDefinition.Item.None)
        {
            return currentSlot;
        }

        // check the other empty slot
        for (int i = 0; i < selectedItem.Length; i++)
        {
            if (selectedItem[i].item == ItemDefinition.Item.None)
                return i;
            else
                continue;
        }
        
        // no empty slot
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
        foreach (var grabber in Grabbers)
        {
            if (!grabber.HeldGrabbable)
            {
                continue;
            }

            if (grabber.HeldGrabbable.isRaceItem)
            {
                GameObject tempItem = Instantiate(grabber.HeldGrabbable.gameObject);
                itemObjectSlot[currentSlot] = tempItem;
                Destroy(grabber.HeldGrabbable?.gameObject);
                grabber.HeldGrabbable = null;
                grabber.DidDrop();
                break;
            }
        }

        currentSlot = index;

        if (currentSlot < 0)
            currentSlot = selectedItem.Length - 1;
        else if (currentSlot >= selectedItem.Length)
            currentSlot = 0;

        GameManager.I.Hud.SwitchSelectedSlot();

        if (itemObjectSlot[currentSlot].GetComponent<RaceItem>().isEquipable)
        {
            // 이곳에 플레이어에게 아이템을 장착시키는 함수 작성
            foreach (var grabber in Grabbers)
            {
                if (!grabber.HeldGrabbable)
                {
                    if (selectedItem[currentSlot].item != ItemDefinition.Item.None) {
                        itemObjectSlot[currentSlot].transform.position = grabber.transform.position;
                        grabber.GrabGrabbable(itemObjectSlot[currentSlot].GetComponent<Grabbable>(), true);
                        break;
                    }
                }
            }
        }
    }

#if UNITY_EDITOR
    // for Item Slot Debug on Desktop
    public void OnGUI()
    {
        if (GUI.Button(new Rect(100, 150, 100, 30), "Slot Left"))
            SwitchItem(currentSlot - 1);
        
        if (GUI.Button(new Rect(200, 150, 100, 30), "Slot Right"))
            SwitchItem(currentSlot + 1);
    }
#endif
}
