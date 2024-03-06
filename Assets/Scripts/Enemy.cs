using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Unit
{
    [SerializeField] Transform damagePivot;       // �������� ��µǴ� ����.

    SpriteRenderer spriteRenderer;
    Rigidbody2D rigid;
    Player target;

    float delayTime;

    protected new void Start()
    {
        base.Start();

        spriteRenderer = GetComponent<SpriteRenderer>();
        rigid = GetComponent<Rigidbody2D>();
        delayTime = cooltime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player player = collision.GetComponent<Player>();
        if (player != null)
            target = player;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        Player player = collision.GetComponent<Player>();
        if (player != null)
            target = null;
    }

    void Update()
    {
        // �÷��̾�� �˹��� ���ϸ� velocity�� �����ϰ� �ȴ�.
        // �ش� velocity�� ���� �ð��� ���� 0���� �����Ѵ�.
        // velocity�� �����ϴ� �� ������ �� ����.

        if (!isAlive || isPauseObject)
            return;

        AttackToPlayer();

        rigid.velocity = Vector2.MoveTowards(rigid.velocity, Vector2.zero, Time.deltaTime * 8f);
        
        Vector3 destination = Player.Instance.transform.position;
        Vector3 dir = (destination - transform.position).normalized;
        spriteRenderer.flipX = dir.x < 0;

        // �˹��� ���� �� (= �и��� ���� ��)���� ������ �� �ִ�.
        if (rigid.velocity == Vector2.zero)
            transform.position = Vector3.MoveTowards(transform.position, destination, speed * Time.deltaTime);
    }

    private void AttackToPlayer()
    {
        if (delayTime > 0f)
        {
            delayTime = Mathf.Clamp(delayTime - Time.deltaTime, 0.0f, cooltime);
            return;
        }

        if (target != null)
        {
            target.TakeDamage(power);
            delayTime = cooltime;
        }
    }

    protected override void OnPauseGame(bool isPause)
    {
        base.OnPauseGame(isPause);
        rigid.constraints = isPause ? RigidbodyConstraints2D.FreezeAll : RigidbodyConstraints2D.None;
    }


    protected override Ability GetIncrease()
    {
        return new Ability();
    }

    public override void TakeDamage(float power, float knockback = 0)
    {
        base.TakeDamage(power, knockback);
        TopUI.Instance.AppearDamage(damagePivot.position, power);
    }

    protected override void Hit(float knockback)
    {
        anim.SetTrigger("onHit");

        // �ڷ� �б�.
        if(knockback > 0f)
        {
            Vector3 dir = (transform.position - Player.Instance.transform.position).normalized;
            rigid.velocity = dir * knockback;
        }
    }
    protected override void Dead()
    {
        GetComponent<Collider2D>().enabled = false;
        rigid.velocity = Vector2.zero;
        anim.SetTrigger("onDead");
        StartCoroutine(IEDead());

        ExpObject exp = ExpObjectPool.Instance.GetRandomExpObject();
        exp.transform.position = transform.position;
    }
    private IEnumerator IEDead()
    {
        float fadeTime = 1.0f;
        Color color = spriteRenderer.color;
        while(fadeTime > 0.0f)
        {
            fadeTime = Mathf.Clamp(fadeTime - Time.deltaTime, 0.0f, 1.0f);
            color.a = fadeTime;
            spriteRenderer.color = color;

            yield return null;
        }

        Destroy(gameObject);
    }
}

