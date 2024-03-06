using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Unit : RootBehaiour
{    
    [SerializeField] 
    Ability origin;        // �⺻ ��ġ.

    // ���� �����Ƽ ��ġ.
    private Ability ability;
    public float maxHp => ability.hp;
    public float power => ability.power;
    public float speed => ability.speed;
    public float cooltime => ability.cooltime;
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

        UpdateAbility();
    }

    protected override void OnPauseGame(bool isPause)
    {
        base.OnPauseGame(isPause);
        anim.speed = isPause ? 0f : 1f;
    }

    // ����ġ&����
    protected virtual void AddExp(int amount)
    {
        exp += amount;
        if (exp >= NeedTotalExp(level + 1))
        {
            level++;
            LevelUp();
        }                
    }
    protected virtual void LevelUp()
    {

    }
    protected int NeedTotalExp(int level)
    {
        if (level <= 0)
            return 0;

        return Mathf.RoundToInt(5000f / 11 * (Mathf.Pow(1.11f, level - 1) - 1));
    }

    // �ɷ�ġ ���
    protected virtual void UpdateAbility()
    {
        // ���� ���� �������ͽ� ���.
        ability = origin + GetIncrease();
        hp = Mathf.Clamp(hp, 0, maxHp);
    }
    protected abstract Ability GetIncrease();

    public WeaponStatus ApplyAbility(WeaponStatus status)
    {
        return status * ability;
    }

    // �ǰ�, ������ ���
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
