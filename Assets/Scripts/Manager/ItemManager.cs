using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class ItemManager : MonoBehaviour
{
    /*[SerializeField] TMP_Text Item1;
    [SerializeField] TMP_Text Item2;
    [SerializeField] TMP_Text Item3;
    GameObject object1;
    GameObject object2;
    GameObject object3;

    string Selected_Item;*/
    string[] selectedItems = new string[3];

    public TMP_Text itemNameInRightPanel;
    public TMP_Text itemDiscriptionInRightPanel;

    public GameObject selectedItemsParent;
    public string itemEmptyText = "None"; //아이템이 선택되지 않았을 때 보이는 텍스트

    public Color checkedColor = Color.red; //selectedItemButton이 선택됬을 때 색깔
    public Color uncheckedColor = Color.white;

    public GameObject saveNotiObj;
    public float notiDuration = 1f;

    public GameObject mainMenuScreenObj;
    public GameObject itemMenuScreenObj;

    private List<GameObject> selectedItemHolders;

    private void OnEnable()
    {
        /*object1 = GameObject.Find("Item 1");
        object2 = GameObject.Find("Item 2");
        object3 = GameObject.Find("Item 3");
        // Item 1, 2, 3의 이름을 가진 object를 scene에서 찾기
        for (int i = 0; i < 3; i++)
        {
            selectedItems[i] = ""; // 배열 초기화
        }
        for (int i = 0; i < 3; i++)
        {
            Debug.Log(selectedItems[i]);
        }
        Set_Item();*/
        saveNotiObj.SetActive(false);

        selectedItemHolders = new List<GameObject>();
        for(int i = 0; i < selectedItemsParent.transform.childCount; i++)//선택된 아이템 뷰의 text와 색상 초기화
        {
            selectedItemHolders.Add(selectedItemsParent.transform.GetChild(i).gameObject);
            selectedItemHolders[i].GetComponent<TMP_Text>().text = itemEmptyText;

            ChangeBtnColor(uncheckedColor, selectedItemHolders[i].GetComponent<Button>());
        }

        if(DataManager.I.LoadSelectedItemData())//저장된 선택 아이템 설정이 존재하면 불러오기 
        {
            for(int i = 0; i < DataManager.maxItemNum; i++)
            {
                selectedItemHolders[i].GetComponent<TMP_Text>().text = DataManager.I.selectedItemInfos[i].name;
            }
        }
    }

    public void OnClickItemButton()// 버튼 클릭시 오른쪽 패널에 설명 뜨게 하기
    {
        GameObject clickedItemButton = EventSystem.current.currentSelectedGameObject; // 현재 선택된 오브젝트를 저장
        ItemData itemData = clickedItemButton.GetComponent<ItemData>();

        itemNameInRightPanel.text = itemData.name;
        itemDiscriptionInRightPanel.text = itemData.discription;
    }

    public void OnClickSelectButton()// 클릭시 선택된 아이템 뷰에 이름 표시되게 하기
    {
        foreach (GameObject selectedItemHolder in selectedItemHolders)
        {
            if(selectedItemHolder.GetComponent<TMP_Text>().text == itemNameInRightPanel.text)//이미 추가된 아이템이면
            {
                break;
            }
            else if(selectedItemHolder.GetComponent<TMP_Text>().text == itemEmptyText)//비어있는 아이템 홀더가 존재 시
            {
                selectedItemHolder.GetComponent<TMP_Text>().text = itemNameInRightPanel.text;
                break;
            }
        }
    }

    public void OnClickSelectedItemBtn()
    {
        GameObject clickedBtnObj = EventSystem.current.currentSelectedGameObject; //현재 선택된 버튼 가져오기
        ColorBlock clickedBtnColorBlock = clickedBtnObj.GetComponent<Button>().colors;

        if(clickedBtnObj.GetComponent<TMP_Text>().text == itemEmptyText)
        {
            return;
        }

        if(clickedBtnColorBlock.normalColor == checkedColor || clickedBtnColorBlock.selectedColor == checkedColor)//이미 선택된 상태라면
        {
            ChangeBtnColor(uncheckedColor, clickedBtnObj.GetComponent<Button>());
        }
        else
        {
            ChangeBtnColor(checkedColor, clickedBtnObj.GetComponent<Button>());
        }
    }

    public void OnClickDeselectBtn() //체크된 아이템들 다 선택해제 시키기
    {
        foreach(GameObject selectedItemHolder in selectedItemHolders)
        {
            ColorBlock selectedItemColorBlock = selectedItemHolder.GetComponent<Button>().colors;
            if(selectedItemColorBlock.normalColor == checkedColor || selectedItemColorBlock.selectedColor == checkedColor)//선택됬다면
            {
                ChangeBtnColor(uncheckedColor, selectedItemHolder.GetComponent<Button>());
                selectedItemHolder.GetComponent<TMP_Text>().text = itemEmptyText;
            }
        }
    }

    public void OnClickSaveBtn()
    {
        //저장할 데이터 생성
        SelectedItemInfo[] itemInfoToSave = new SelectedItemInfo[DataManager.maxItemNum];
        for(int i = 0; i < DataManager.maxItemNum; i++)
        {
            itemInfoToSave[i] = new SelectedItemInfo
            {
                name = selectedItemHolders[i].GetComponent<TMP_Text>().text
            };
        }

        //저장 및 결과 알림
        bool isSaveSuccessful = DataManager.I.SaveSelectedItemData(itemInfoToSave);
        StopAllCoroutines();
        if(isSaveSuccessful)
        {
            StartCoroutine(StartNoti("저장을 성공했습니다."));
        }
        else
        {
            StartCoroutine(StartNoti("오류로 인해 저장을 실패했습니다."));
        }
    }

    public void OnClickBackBtn()//보니까 UIScreen에서 각 Screen을 연결시켜주는 것 같아.
    {
        StopAllCoroutines();
        itemMenuScreenObj.SetActive(false);
        mainMenuScreenObj.SetActive(true);
        //DataManager의 아이템목록으로 selectedItems를 다시 바꾸고 나가기
    }

    private IEnumerator StartNoti(string notiText)
    {
        saveNotiObj.SetActive(true);
        saveNotiObj.GetComponent<TMP_Text>().text = notiText;
        yield return new WaitForSeconds(notiDuration);
        saveNotiObj.SetActive(false);
    }


    private void ChangeBtnColor(Color colorToChange, Button button)//현재 버튼 색깔을 변경
    {
        ColorBlock colorBlock = button.colors;
        colorBlock.normalColor = colorToChange;
        colorBlock.selectedColor = colorToChange;

        button.colors = colorBlock;
    }

   /* public void ButtonClick()//구형 버전: 신형석
    {
        GameObject clickObject = EventSystem.current.currentSelectedGameObject; // 현재 선택된 오브젝트를 저장
        Selected_Item = clickObject.GetComponentInChildren<TMP_Text>().text; // 저장된 오브젝트의 이름을 가지고 온다
        for (int i = 0; i < 3; i++)
        {
            if (selectedItems[i] != "" && selectedItems[i] == Selected_Item) // 선택된 오브젝트 (아이템)을 현재 item_pot와 비교한다. 
            {
                break;
            }
            if (selectedItems[i] == "") // 아이템이 들어가 있지 않다면 아이템을 넣는다. 
            {
                selectedItems[i] = Selected_Item;
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
        Item1.text = selectedItems[0];
        Item2.text = selectedItems[1];
        Item3.text = selectedItems[2];
    }*/
}