using System.Collections;
using UnityEngine;

public abstract class WeaponObject : RootBehaiour
{
    private WeaponStatus status;

    public float power => status.power;                                         // ���ݷ�.
    public int projectileCount => Mathf.RoundToInt(status.projectileCount);     // ����ü ����.
    public float cooltime => status.cooltime;                                   // ��Ÿ��.
    public float continueTime => status.continueTime;                           // ���ӽð�.
    public float knockback => status.knockback;
    public int penetrate => Mathf.RoundToInt(status.penetrate);

    protected void Start()
    {
        Initialize();
        StartCoroutine(IEUpdate());
    }

    private IEnumerator IEUpdate()
    {
        while(true)
        {
            float time = cooltime;
            while (time > 0.0f)
            {
                if(!isPauseObject)
                    time -= Time.deltaTime;
                yield return null;
            }
            yield return StartCoroutine(IEAttack());
        }
    }

    protected abstract void Initialize();
    protected abstract IEnumerator IEAttack();
    
    public void UpdateWeapon(Ability ownerAbility)
    {
        // item�� ������ ���� �⺻ ��ġ + ���� ��ġ�� ���� ���� ��ü�� �ɷ�ġ�� ���.
        // ���⿡ ������� �����Ƽ�� ���� ���� �ɷ�ġ�� ���Ѵ�.        
        // status = itemInfo.status * ownerAbility;
    }
}
