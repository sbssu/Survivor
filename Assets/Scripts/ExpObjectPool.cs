using UnityEngine;

public class ExpObjectPool : Singleton<ExpObjectPool>
{
    [SerializeField] ExpObject prefab;

    public ExpObject GetRandomExpObject()
    {
        // TYPE�� ��� ���� ������ ������ �ϳ��� ��Ҹ� ����.
        var types = System.Enum.GetValues(typeof(ExpObject.TYPE)) as ExpObject.TYPE[];
        ExpObject.TYPE type = types[Random.Range(0, types.Length)];

        ExpObject newExpObject = Instantiate(prefab);
        newExpObject.Setup(type);
        return newExpObject;
    }        
}
