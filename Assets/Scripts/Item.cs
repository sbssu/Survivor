using UnityEditorInternal.Profiling.Memory.Experimental;
using UnityEngine;

[System.Serializable]
public abstract class Item
{
    public string itemName;     // ������ �̸�.
    public string id;           // ���� ���̵�.
    public string description;  // ����.
    public Sprite iconSprite;   // ������ ��������Ʈ.
    public int level;

    public virtual Item Copy() { return null; }

    public abstract Status GetStatus();
}

[System.Serializable]
public class PassiveItem : Item
{
    public Ability status; // �⺻ �ɷ�ġ.

    public override Status GetStatus()
    {
        return status;
    }
    public override Item Copy()
    {
        PassiveItem newItem = new PassiveItem();
        newItem.itemName = itemName;
        newItem.id = id;
        newItem.description = description;
        newItem.iconSprite = iconSprite;
        newItem.status = status;
        newItem.level = level;
        return newItem;
    }
}

[System.Serializable]
public class WeaponItem : Item
{
    [Header("Weapon")]
    public Sprite handSprite;                   // ��� ��������Ʈ.
    public WeaponObject weaponPrefab;                 // ���� ������.

    [SerializeField] WeaponStatus origin;       // �ɷ�ġ.
    [SerializeField] WeaponStatus grow;         // ����ġ.

    public override Status GetStatus()
    {
        return origin + (grow * (level - 1));
    }

    public override Item Copy()
    {
        WeaponItem newItem = new WeaponItem();
        newItem.itemName = itemName;
        newItem.id = id;
        newItem.description = description;
        newItem.iconSprite = iconSprite;
        newItem.handSprite = handSprite;
        newItem.origin = origin;
        newItem.grow = grow;
        newItem.level = level;
        return newItem;
    }
}
