using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : RootBehaiour
{    
    [SerializeField]
    Ability originStatus;        // �⺻ ��ġ.
    
    private Ability increaseStatus { get; set; }  // ���� ��ġ.
    protected Ability finalStatus { get; set; }     // ���� ��ġ.

    public float maxHp => finalStatus.hp;
    public float power => finalStatus.power;
    public float speed => finalStatus.speed;
    public float cooltime => finalStatus.cooltime;
    public bool isAlive => hp > 0;

    protected float hp;       // ü��
    protected int level;      // ����.
    protected int exp;        // ����ġ.
    protected int killCount;  // ų ī��Ʈ.

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
        // ���� ���� �������ͽ� ���.
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


    // ���Ⱑ ������� Ability�� �����Ű�� �Լ�.
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
