using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Melee : MonoBehaviour
{
    float power;
    float knockback;

    public void Setup(float power, float knockback)
    {
        this.power = power;
        this.knockback = knockback;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Enemy enemy = collision.GetComponent<Enemy>();
        if(enemy != null)
            enemy.TakeDamage(power, knockback);
    }
}
