using UnityEditorInternal.Profiling.Memory.Experimental;
using UnityEngine;

[System.Serializable]
public abstract class Item
{
    public string itemName;     // ������ �̸�.
    public string id;           // ���� ���̵�.
    public string description;  // ����.
    public Sprite iconSprite;   // ������ ��������Ʈ.

    [HideInInspector]
    public int level;           // ����.

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
    public Ability ability;     // �ɷ�ġ.

    public PassiveItem() : base()
    {
        ability = new Ability();
    }

}

[System.Serializable]
public class WeaponItem : Item
{
    [Header("Weapon")]
    public Sprite handSprite;                   // ��� ��������Ʈ.
    public WeaponObject weaponPrefab;           // ���� ������.

    [SerializeField] WeaponStatus origin;       // �⺻ �ɷ�ġ.
    [SerializeField] WeaponStatus grow;         // ������ ����ġ.

    public WeaponStatus status => origin + (grow * (level - 1));

    public WeaponItem() : base()
    {
        handSprite = null;
        weaponPrefab = null;
        origin = new WeaponStatus();
        grow = new WeaponStatus();
    }
}
