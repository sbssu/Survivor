using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ItemManager : Singleton<ItemManager>
{
    [SerializeField] PassiveItem[] passiveInfos;        // �нú� ������ ����.
    [SerializeField] WeaponItem[] weaponInfos;          // ���� ������ ����.

    public Item GetItem(string id, int forceLevel = 0)
    {
        // �� ������ �������� ����Ʈ�� ���� �� ���´�.
        List<Item> itemList = new List<Item>();
        itemList.AddRange(passiveInfos);
        itemList.AddRange(weaponInfos);

        Item item = itemList.Find(item => item.id == id);
        if(forceLevel > 0)
            item.level = forceLevel;
        return item;
    }
    public Item[] GetRandomItem(int count = 3)
    {
        // �� ������ �������� ����Ʈ�� ���� �� ���´�.
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

        // count(=�⺻ 3)�� ��ŭ �̾Ƽ� ����.
        return itemList.Take(count).ToArray();
    }
}
