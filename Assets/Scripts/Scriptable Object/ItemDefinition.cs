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

    public void Use()
    {
        switch (item)
        {
            case Item.None:
                break;
            case Item.Test1:
            case Item.Test2:
            case Item.Test3:
                Debug.Log("Use Test Item");
                break;
        }
    }
}