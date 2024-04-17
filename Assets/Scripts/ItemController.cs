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

    public float delayToSwitchItem = 0.5f;
    private float timer = 0.0f;

    public void Start()
    {
        InitSelectedItem();
        InitItemController();
    }

    public void InitSelectedItem()
    {
        if(DataManager.I.selectedItemInfos == null || DataManager.I.selectedItemInfos.Length == 0 || DataManager.I.selectedItemInfos[0] == null)
        {
            return;
        }

        foreach(var item in itemList)// 기본 아이템인 pistol 배정
        {
            if(item.name == "Pistol")
            {
                selectedItem[0] = item;
            }
        }

        for(int i = 0; i < DataManager.I.selectedItemInfos.Length; i++)// 선택 아이템들 배정
        {
            for(int j = 0; j < itemList.Length; j++)
            {
                if (DataManager.I.selectedItemInfos[i].name == itemList[j].name)
                {
                    selectedItem[i+1] = itemList[j];
                }
            }
        }
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
                itemObjectSlot[i].transform.position = Vector3.zero;
            }
        }

        if (selectedItem[0])
        {
            Grabbers[0].GrabGrabbable(itemObjectSlot[0].GetComponent<Grabbable>(), true);
            //itemObjectSlot[0].transform.position = Grabbers[0].transform.position + Vector3.down;
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
        Debug.Log($"Clean the current slot. slotNum: {currentSlot}");
    }
    
    /// <summary>
    /// Add the "item" in the "index"-th item slot when called.
    /// </summary>
    /// <param name="item"></param>
    /// <param name="index"></param>
    public void AddItemToSlot(Grabbable item, int index)
    {
        var raceItemDefinition = item.itemDefinition;
        
        if (selectedItem[index].item == ItemDefinition.Item.None)
        {
            GameManager.I.Hud.ChangeSlotIcon(index, raceItemDefinition.itemIcon);
            selectedItem[currentSlot] = raceItemDefinition;
            itemObjectSlot[currentSlot] = item.gameObject;
            Debug.Log($"Add item:{raceItemDefinition.itemName} to {index}-th slot");
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
                GameObject tempItem;
                if (selectedItem[currentSlot].item == ItemDefinition.Item.Pistol)//pistol은 아이템 교체 후 다시 돌아오면, pistol이 투명화되는 버그 존재. 그것의 임시방편
                {
                    tempItem = Instantiate(selectedItem[currentSlot].ItemPrefab); //이건 아예 새로 아이템 생성하는거라, 탄창 수 등 현재 상태 반영못함. pistol은 어차피 탄창 무한이므로 노상관
                    tempItem.gameObject.transform.position = Vector3.zero;
                }
                else
                {
                    tempItem = Instantiate(grabber.HeldGrabbable.gameObject);
                    tempItem.gameObject.transform.position = Vector3.zero;

                    //tempItem과 같이 딸려온 grabModel 삭제
                    // GameObject copyedGrabModel = tempItem.GetComponent<Grabbable>().GetGrabPoint()?.GetChild(0)?.gameObject;
                    Grabbable grabbableComponent = tempItem.GetComponent<Grabbable>();
                    if (grabbableComponent.GrabPoints.Count > 0)// Uzi_copy, RPG일 경우
                    {
                        var grabPoint = grabbableComponent.GrabPoints[0];
                        for (int i = 0; i < grabPoint.childCount; i++)
                        {
                            if (grabPoint.GetChild(i).name == "ModelsRight")
                            {
                                Destroy(grabPoint.GetChild(i).gameObject);
                                break;
                            }
                        }
                    }
                    else// Booster, Repiar일 경우
                    {
                        for (int i = 0; i < tempItem.transform.childCount; i++)
                        {
                            if (tempItem.transform.GetChild(i).name == "ModelsRight")
                            {
                                Destroy(tempItem.transform.GetChild(i).gameObject);
                                break;
                            }
                        }
                    }
                }

                itemObjectSlot[currentSlot] = tempItem;
                Destroy(grabber.HeldGrabbable?.gameObject);

                grabber.hasSwitchedSlot = true;
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

        if (selectedItem[currentSlot].isEquipable)
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

    public void Update()
    {
        timer += Time.deltaTime;
        if(timer >= delayToSwitchItem)
        {
            if (InputBridge.Instance.LeftThumbNear)
            {
                SwitchItem(currentSlot - 1);
                timer = 0.0f;
            }
            else if (InputBridge.Instance.RightThumbNear)
            {
                SwitchItem(currentSlot + 1);
                timer = 0.0f;
            }
            
        }
    }

/*#if UNITY_EDITOR
    // for Item Slot Debug on Desktop
    public void OnGUI()
    {
        if (GUI.Button(new Rect(100, 150, 100, 30), "Slot Left"))
            SwitchItem(currentSlot - 1);
        
        if (GUI.Button(new Rect(200, 150, 100, 30), "Slot Right"))
            SwitchItem(currentSlot + 1);
    }
#endif*/
}
