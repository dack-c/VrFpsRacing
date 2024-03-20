using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Scriptable Object/Item")]
public class ItemDefinition : ScriptableObject
{
    public enum Item
    {
        None, 
        Test1, 
        Test2,
        Test3
    }

    public string itemName;
    public Sprite itemIcon;         // UI에서 출력될 아이템 아이콘
    public Item item;               // 아이템 구분 값(고유 ID)
    public bool isEquipable;        // 아이템의 장착(스폰) 가능 여부
    public GameObject ItemPrefab;   // 실제로 작동할 아이템 프리팹

    public bool Use()
    {
        if (false == isEquipable)
            return true;

        switch (item)
        {
            // 
            case Item.None:
                return true;
            // 
            case Item.Test1:
            case Item.Test2:
            case Item.Test3:
                Debug.Log("Use Test Item");
                return false;
        }
        return false;
    }
}