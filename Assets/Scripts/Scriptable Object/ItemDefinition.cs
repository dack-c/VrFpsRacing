using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Scriptable Object/Item")]
public class ItemDefinition : ScriptableObject
{
    public enum Item
    {
        Test1, 
        Test2,
        Test3
    }

    public string itemName;
    public Sprite itemIcon;
    public Item item;
}