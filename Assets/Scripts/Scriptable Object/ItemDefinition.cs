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
    public Sprite itemIcon;
    public Item item;
    public GameObject ItemPrefab; 

    public bool Use()
    {
        switch (item)
        {
            case Item.None:
                return true;
            case Item.Test1:
            case Item.Test2:
            case Item.Test3:
                Debug.Log("Use Test Item");
                return false;
        }
        return false;
    }
}