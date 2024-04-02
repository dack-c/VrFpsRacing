using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item Definition", menuName = "Scriptable Object/Item Definition")]
public class ItemDefinition : ScriptableObject
{
    /*
     *  아이템 Scriptable Object(스크립터블 오브젝트) 추가 가이드
     *
     *  1. Assets > Create > Scripable Object > Item 을 눌러 새 스크립터블 오브젝트를 생성한다.
     *  2. 추가할 아이템을 구분할 수 있는 enum 값을 스크립트에 추가한다.
     *  3. inspector 뷰에서 생성된 새 스크립터블 오브젝트에 정보를 넣어준다(아이템 이름, 아이콘, enum 값, 장착 가능 여부, 아이템 프리팹)
     *  4. 이 스크립트 Use() 함수 Switch 문 내에 해당하는 아이템의 enum값에 대한 케이스를 추가하여 ItemPrefab을 통하여 아이템을 사용할 수 있는 기능을 넣어준다.
     *  5. ItemController 프리팹 ItemController 컴포넌트 내에 itemList에 해당 아이템 스크립터블 오브젝트를 추가한다.
     */

    public enum Item
    {
        None, 
        Test1, 
        Test2,
        Test3,
        Rpg9,
        MachineGun,
        RepairKit,
        Booster
    }

    public string itemName;
    public Sprite itemIcon;         // UI에서 출력될 아이템 아이콘
    public Item item;               // 아이템 구분 값(고유 ID)
    public GameObject ItemPrefab;   // 실제로 작동할 아이템 프리팹
}