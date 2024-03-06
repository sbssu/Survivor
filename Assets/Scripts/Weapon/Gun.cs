using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// weapon�� abstract�� �Լ��� �����ؾ�������
// Gun�� ���Ϸ� ������ �� ���� abstract Ŭ�����̱� ������ �������� �ʾƵ� �ȴ�.
public class Gun : WeaponObject
{
    [SerializeField] Bullet prefab;         // ������.
    [SerializeField] float fireRate;        // ����ӵ�.
    [SerializeField] LayerMask targetMask;  // ���� ��� ����ũ.

    private float fireRange;

    // �ʱ�ȭ.
    protected override void Initialize()
    {
        fireRange = Camera.main.orthographicSize * 2f * (Screen.width / (float)Screen.height);
        prefab.gameObject.SetActive(false);
    }
    protected override IEnumerator IEAttack()
    {
        // ���� �����Ÿ� ���� ������ ����Ѵ�.
        while (!SearchTarget(out Vector2 _))
            yield return null;

        // ���� �߰��ϸ� �߻��Ѵ�.
        for(int i = 0; i < projectileCount; i++)
        {
            if (SearchTarget(out Vector2 dir))
                Shoot(dir);

            yield return StartCoroutine(FireDealy());
        }

        // ���� �Լ�
        IEnumerator FireDealy()
        {
            float delay = fireRate;
            while(true)
            {
                if(!isPauseObject)
                {
                    if ((delay -= Time.deltaTime) <= 0.0f)
                        break;
                }
                yield return null;
            }
        }
    }

    protected virtual bool SearchTarget(out Vector2 dir)
    {
        // ����� ������ �⺻ ������ ���� �����Ÿ� �ȿ� �ִ°�?
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, fireRange, targetMask);
        if (colliders.Length <= 0)
        {
            dir = Vector2.zero;
            return false;
        }

        // ���� ����� ��
        Collider2D near = colliders.OrderBy(c => Vector2.Distance(transform.position, c.transform.position)).First();
        dir = (near.transform.position - transform.position).normalized;
        return true;
    }

    private void Shoot(Vector3 dir)
    {
        Bullet bullet = Instantiate(prefab);                                                // �Ѿ� ������ ����.
        bullet.Setup(power, penetrate);                                                     // �Ѿ� ������Ʈ �¾�.

        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;                            // ���⿡ ���� ���� ���.
        bullet.transform.rotation = Quaternion.AngleAxis(angle - 90f, Vector3.forward);     // z�� ���� ȸ����.
        bullet.transform.position = transform.position;                                     // ���� ��ġ ����.
        bullet.gameObject.SetActive(true);                                                  // ������Ʈ Ȱ��ȭ.
    }
}
