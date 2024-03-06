using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : Singleton<ItemManager>
{
    [SerializeField] PassiveItem[] passiveInfos;        // 패시브 아이템 정보.
    [SerializeField] WeaponItem[] weaponInfos;          // 무기 아이템 정보.


    public Item GetItem(string id, int forceLevel = 1)
    {
        // 두 종류의 아이템을 리스트에 담은 뒤 섞는다.
        List<Item> itemList = new List<Item>();
        itemList.AddRange(passiveInfos);
        itemList.AddRange(weaponInfos);

        Item item = itemList.Find(item => item.id == id);
        item.level = forceLevel;
        return item;
    }
    public Item[] GetRandomItem(List<Item> inventory)
    {
        int count = 3;

        // 두 종류의 아이템을 리스트에 담은 뒤 섞는다.
        List<Item> itemList = new List<Item>();
        itemList.AddRange(passiveInfos);
        itemList.AddRange(weaponInfos);
        for(int i= 0; i<itemList.Count * 10; i++)
        {
            int ran1 = Random.Range(0, itemList.Count);
            int ran2 = Random.Range(0, itemList.Count);

            Item temp = itemList[ran1];
            itemList[ran1] = itemList[ran2];
            itemList[ran2] = temp;
        }

        // count(=기본 3)개 만큼 뽑아서 전달.
        Item[] selected = new Item[count];
        for (int i = 0; i < count; i++)
        {
            // 인벤토리 내부에 해당 아이템이 존재하는지 확인.
            Item target = itemList[i].Copy();
            Item have = inventory.Find(item => item.id == target.id);
            target.level = (have == null) ? 1 : have.level + 1;
            selected[i] = target;
        }

        return selected;
    }

    public WeaponObject GetWeaponPrefab(string id)
    {
        WeaponItem weapon = System.Array.Find(weaponInfos, info => info.id == id);
        //weapon.weaponPrefab

        return weapon.weaponPrefab;
    }

}
