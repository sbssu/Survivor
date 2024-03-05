using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : RootBehaiour
{
    [SerializeField] float speed;

    int penetrate;
    float power;


    public void Setup(float power, int penetrate)
    {
        this.power = power;
        this.penetrate = penetrate;
    }

    private void Update()
    {
        if (!isPauseObject)
        {
            transform.position += transform.up * speed * Time.deltaTime;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Enemy enemy = collision.GetComponent<Enemy>();
        if (enemy == null)
            return;

        enemy.TakeDamage(power);
        penetrate -= 1;
        if (penetrate <= 0)
            Destroy(gameObject);
    }
}
