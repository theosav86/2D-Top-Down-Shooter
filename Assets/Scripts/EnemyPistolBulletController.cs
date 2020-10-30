using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPistolBulletController : MonoBehaviour
{

    private int enemyPistolBulletDamage = 5;


    private void OnCollisionEnter2D(Collision2D other)
    {
        IDamagable damagable = other.collider.GetComponent<IDamagable>();

        if(damagable != null)
        {
            damagable.TakeDamage(enemyPistolBulletDamage);
        }

        Destroy(gameObject);
    }
}
