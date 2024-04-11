using System.Linq;
using System.Runtime.Remoting.Messaging;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item Definition", menuName = "Scriptable Object/Item Definition")]
public class ItemDefinition : ScriptableObject
{
    /*
     *  아이템 Scriptable Object(스크립터블 오브젝트) 추가 가이드
     *
     *  1. Assets > Create > Scripable Object > Item Definition 을 눌러 새 스크립터블 오브젝트를 생성한다.
     *  2. 추가할 아이템을 구분할 수 있는 enum 값을 스크립트에 추가한다.
     *  3. inspector 뷰에서 생성된 새 스크립터블 오브젝트에 정보를 넣어준다
     *  4. ItemController 프리팹 ItemController 컴포넌트 내에 ItemList에 해당 아이템 스크립터블 오브젝트를 추가한다.
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
        Booster,
        Pistol
    }
    
    [Header("Item Info")]
    public string itemName;         // 아이템 이름
    public Sprite itemIcon;         // UI에서 출력될 아이템 아이콘
    public Item item;               // 아이템 구분 값(고유 ID)
    public GameObject ItemPrefab;   // 실제로 작동할 아이템 프리팹
    
    [Header("Item Property")]
    public bool isEquipable;        // 아이템의 장착(스폰) 가능 여부 (장착: true)
    public bool isDisposable;       // 아이템 사용의 일회성 여부 (1회용: true)
}