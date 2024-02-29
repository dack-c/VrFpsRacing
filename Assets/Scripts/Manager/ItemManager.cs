using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class ItemManager : MonoBehaviour
{
    [SerializeField] TMP_Text Item1;
    [SerializeField] TMP_Text Item2;
    [SerializeField] TMP_Text Item3;
    GameObject object1;
    GameObject object2;
    GameObject object3;

    string Selected_Item;
    string[] Item_Pot = new string[3];
    private void Start()
    {
        object1 = GameObject.Find("Item 1");
        object2 = GameObject.Find("Item 2");
        object3 = GameObject.Find("Item 3");
        // Item 1, 2, 3의 이름을 가진 object를 scene에서 찾기
        for (int i = 0; i < 3; i++)
        {
            Item_Pot[i] = ""; // 배열 초기화
        }
        for (int i = 0; i < 3; i++)
        {
            Debug.Log(Item_Pot[i]);
        }
        Set_Item();
    }
    public void ButtonClick()
    {
        GameObject clickObject = EventSystem.current.currentSelectedGameObject; // 현재 선택된 오브젝트를 저장
        Selected_Item = clickObject.GetComponentInChildren<TMP_Text>().text; // 저장된 오브젝트의 이름을 가지고 온다
        for (int i = 0; i < 3; i++)
        {
            if (Item_Pot[i] != "" && Item_Pot[i] == Selected_Item) // 선택된 오브젝트 (아이템)을 현재 item_pot와 비교한다. 
            {
                break;
            }
            if (Item_Pot[i] == "") // 아이템이 들어가 있지 않다면 아이템을 넣는다. 
            {
                Item_Pot[i] = Selected_Item;
                if (i == 0)
                {
                    object1.GetComponent<SelectItems>().isItemIn = true;
                }
                else if (i == 1)
                {
                    object2.GetComponent<SelectItems>().isItemIn = true;
                }
                else if (i == 2)
                {
                    object3.GetComponent<SelectItems>().isItemIn = true;
                }
                break;
            }
        }
        Set_Item();
        Debug.Log(Selected_Item);
    }

    private void Set_Item() // 아이템 text를 바꿔주기
    {
        Item1.text = Item_Pot[0];
        Item2.text = Item_Pot[1];
        Item3.text = Item_Pot[2];
    }
}