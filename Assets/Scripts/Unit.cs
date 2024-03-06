using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : RootBehaiour
{    
    [SerializeField]
    Ability originStatus;        // 기본 수치.
    
    private Ability increaseStatus { get; set; }  // 증가 수치.
    protected Ability finalStatus { get; set; }     // 최종 수치.

    public float maxHp => finalStatus.hp;
    public float power => finalStatus.power;
    public float speed => finalStatus.speed;
    public float cooltime => finalStatus.cooltime;
    public bool isAlive => hp > 0;

    protected float hp;       // 체력
    protected int level;      // 레벨.
    protected int exp;        // 경험치.
    protected int killCount;  // 킬 카운트.

    protected Animator anim;

    protected void Start()
    {
        anim = GetComponent<Animator>();
        hp = int.MaxValue;
        level = 1;
        exp = 0;
        killCount = 0;                

        UpdateStatus();
    }

    protected override void OnPauseGame(bool isPause)
    {
        base.OnPauseGame(isPause);
        anim.speed = isPause ? 0f : 1f;
    }
    protected virtual void UpdateStatus()
    {
        // 실제 적용 스테이터스 계산.
        finalStatus = originStatus + increaseStatus;
        hp = Mathf.Clamp(hp, 0, maxHp);
    }

    protected void ResetIncrease()
    {
        increaseStatus = new Ability();
    }
    protected void AddIncrease(Ability ability)
    {
        increaseStatus += ability;
    }


    // 무기가 사용자의 Ability를 적용시키는 함수.
    public WeaponStatus ApplyAbility(WeaponStatus target)
    {
        return target * finalStatus;
    }
    public virtual void TakeDamage(float power, float knockback = 0f)
    {
        if (!isAlive)
            return;

        if(Random.value < 0.15f)
        {
            power *= 2;
        }

        hp = Mathf.Clamp(hp - power, 0, maxHp);
        if (hp <= 0)
            Dead();
        else
            Hit(knockback);
    }

    protected virtual void Dead()
    {
    }
    protected virtual void Hit(float knockback)
    {

    }
}
