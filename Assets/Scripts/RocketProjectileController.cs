using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketProjectileController : MonoBehaviour
{

    public GameObject rocketExplosion;

    [SerializeField]
    private int rocketDamage = 100;
    [SerializeField]
    private float rocketSplashRadius = 3.0f;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Collider2D[] collidersHitByRocketSplash;
        collidersHitByRocketSplash = Physics2D.OverlapCircleAll(this.transform.position, rocketSplashRadius);

        Enemy enemy = collision.collider.GetComponent<Enemy>();

        foreach(Collider2D collider in collidersHitByRocketSplash)
        {
            Enemy e = collider.GetComponent<Enemy>();

            if (e != null)
            {
                e.TakeDamage(rocketDamage);
            }
        }

        //APPLY DAMAGE ON ENEMYCONTROLLER INSTANCE THAT WAS HIT
        if (enemy != null)
        {
             //apply damage
            //Debug.Log("Rocket HIT ENEMY COLLIDER METHOD IN PROJECTILE");
            enemy.TakeDamage(rocketDamage);

        }

        //INSTANTIATE ROCKET BULLET EXPLOSION PARTICLES
        GameObject rocketExplosionEffect = Instantiate(rocketExplosion, transform.position, Quaternion.identity);//instantiate the particle or animation of bullet explosion on collision with anything.

        //Instead of Destroy(bulletExplosionEffect) I used a script on the particle called SelfDestruct
        //Destroy(bulletExplosionEffect, 4f);//Destroy the bullet explosion effect after 4 seconds

        //Destroy the bullet Prefab on collision with anything
        Destroy(gameObject);
    }
}
