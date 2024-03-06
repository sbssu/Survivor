using System.Collections;
using UnityEngine;

public abstract class WeaponObject : RootBehaiour
{
    private WeaponStatus status;

    public float power => status.power;                                         // 공격력.
    public int projectileCount => Mathf.RoundToInt(status.projectileCount);     // 투사체 개수.
    public float cooltime => status.cooltime;                                   // 쿨타임.
    public float continueTime => status.continueTime;                           // 지속시간.
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
        // item의 레벨을 통해 기본 수치 + 성장 수치를 더해 무기 자체의 능력치를 계산.
        // 여기에 사용자의 어빌리티르 곱해 최종 능력치를 구한다.        
        // status = itemInfo.status * ownerAbility;
    }
}
