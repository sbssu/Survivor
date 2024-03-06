using System.Collections;
using UnityEngine;

public abstract class WeaponObject : RootBehaiour
{
    private Unit owner;                     // 소유자.
    private WeaponItem itemInfo;            // 아이템 정보.
    private WeaponStatus status;            // 능력치.

    public float power => status.power;                                         // 공격력.
    public int projectileCount => Mathf.RoundToInt(status.projectileCount);     // 투사체 개수.
    public float cooltime => status.cooltime;                                   // 쿨타임.
    public float continueTime => status.continueTime;                           // 지속시간.
    public float knockback => status.knockback;
    public int penetrate => Mathf.RoundToInt(status.penetrate);

    IEnumerator Start()
    {
        Initialize();
        while (owner.isAlive)
        {
            float time = cooltime;
            while (time > 0.0f)
            {
                if (!isPauseObject)
                    time -= Time.deltaTime;
                yield return null;
            }
            yield return StartCoroutine(IEAttack());
        }

        gameObject.SetActive(false);
    }

    public void Setup(Unit owner, WeaponItem itemInfo)
    {
        this.owner = owner;
        this.itemInfo = itemInfo;

        UpdateWaeponStatus();
    }
    public void UpdateWaeponStatus()
    {
        // item의 레벨을 통해 기본 수치 + 성장 수치를 더해 무기 자체의 능력치를 계산.
        // 여기에 사용자의 어빌리티르 곱해 최종 능력치를 구한다.        
        status = owner.ApplyAbility(itemInfo.status);
    }

    protected abstract void Initialize();
    protected abstract IEnumerator IEAttack();
}
