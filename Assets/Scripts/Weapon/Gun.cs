using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// weapon의 abstract를 함수를 구현해야하지만
// Gun도 단일로 존재할 수 없는 abstract 클래스이기 때문에 구현하지 않아도 된다.
public class Gun : WeaponObject
{
    [SerializeField] Bullet prefab;         // 프리팹.
    [SerializeField] float fireRate;        // 연사속도.
    [SerializeField] LayerMask targetMask;  // 공격 대상 마스크.

    private float fireRange;

    // 초기화.
    protected override void Initialize()
    {
        fireRange = Camera.main.orthographicSize * 2f * (Screen.width / (float)Screen.height);
        prefab.gameObject.SetActive(false);
    }
    protected override IEnumerator IEAttack()
    {
        // 적이 사정거리 내에 없으면 대기한다.
        while (!SearchTarget(out Vector2 _))
            yield return null;

        // 적을 발견하면 발사한다.
        for(int i = 0; i < projectileCount; i++)
        {
            if (SearchTarget(out Vector2 dir))
                Shoot(dir);

            yield return StartCoroutine(FireDealy());
        }

        // 내부 함수
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
        // 사격이 가능한 기본 조건은 적이 사정거리 안에 있는가?
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, fireRange, targetMask);
        if (colliders.Length <= 0)
        {
            dir = Vector2.zero;
            return false;
        }

        // 가장 가까운 적
        Collider2D near = colliders.OrderBy(c => Vector2.Distance(transform.position, c.transform.position)).First();
        dir = (near.transform.position - transform.position).normalized;
        return true;
    }

    private void Shoot(Vector3 dir)
    {
        Bullet bullet = Instantiate(prefab);                                                // 총알 프리팹 생성.
        bullet.Setup(power, penetrate);                                                     // 총알 오브젝트 셋업.

        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;                            // 방향에 따른 각도 계산.
        bullet.transform.rotation = Quaternion.AngleAxis(angle - 90f, Vector3.forward);     // z축 기준 회전량.
        bullet.transform.position = transform.position;                                     // 최초 위치 대입.
        bullet.gameObject.SetActive(true);                                                  // 오브젝트 활성화.
    }
}
