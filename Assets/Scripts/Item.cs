using UnityEditorInternal.Profiling.Memory.Experimental;
using UnityEngine;

[System.Serializable]
public abstract class Item
{
    public string itemName;     // 아이템 이름.
    public string id;           // 고유 아이디.
    public string description;  // 설명.
    public Sprite iconSprite;   // 아이콘 스프라이트.

    [HideInInspector]
    public int level;           // 레벨.

    public Item()
    {
        itemName = string.Empty;
        id = string.Empty;
        description = string.Empty;
        iconSprite = null;
        level = 0;
    }
}

[System.Serializable]
public class PassiveItem : Item
{
    public Ability ability;     // 능력치.

    public PassiveItem() : base()
    {
        ability = new Ability();
    }

}

[System.Serializable]
public class WeaponItem : Item
{
    [Header("Weapon")]
    public Sprite handSprite;                   // 장비 스프라이트.
    public WeaponObject weaponPrefab;           // 무기 프리팹.

    [SerializeField] WeaponStatus origin;       // 기본 능력치.
    [SerializeField] WeaponStatus grow;         // 레벨별 성장치.

    public WeaponStatus status => origin + (grow * (level - 1));

    public WeaponItem() : base()
    {
        handSprite = null;
        weaponPrefab = null;
        origin = new WeaponStatus();
        grow = new WeaponStatus();
    }
}
