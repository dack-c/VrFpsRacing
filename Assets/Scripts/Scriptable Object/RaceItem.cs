using System.Linq;
using UnityEngine;

public class RaceItem : MonoBehaviour
{
    public ItemDefinition itemDefinition;               // 아이템 구분 값(고유 ID)
    public bool isEquipable;                            // 아이템의 장착(스폰) 가능 여부 (장착: true)
    public bool isDisposable;                           // 아이템 사용의 일회성 여부 (1회용: true)
}