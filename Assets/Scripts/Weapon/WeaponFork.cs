using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponFork : Weapon
{
    [SerializeField] Bullet prefab;
    [SerializeField] float fireRate;    // 연사속도.


    protected override void Initialize()
    {
        prefab.gameObject.SetActive(false);
    }

    protected override IEnumerator IEAttack()
    {
        // 총알 개수만큼 프리팹 생성.
        Queue<Bullet> projectiles = new Queue<Bullet>();
        for (int i = 0; i < projectileCount; i++)
        {
            Bullet newBullet = Instantiate(prefab);
            newBullet.Setup(power, penetrate);            
            projectiles.Enqueue(newBullet);
        }

        // 일정 시간에 한 번씩 총알을 발사.
        float delayTime = 0f;
        while (projectiles.Count > 0)
        {
            if (!isPauseObject)
            {
                delayTime -= Time.deltaTime;
                if (delayTime <= 0.0f)
                {
                    delayTime = fireRate;
                    Bullet bullet = projectiles.Dequeue();
                    Shoot(bullet);
                }
            }
            yield return null;
        }
    }

    private void Shoot(Bullet bullet)
    {
        Vector3 dir = Player.Instance.direction;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        bullet.transform.rotation = Quaternion.AngleAxis(angle - 90f, Vector3.forward);        
        bullet.transform.position = transform.position;
        bullet.gameObject.SetActive(true);
    }

}
