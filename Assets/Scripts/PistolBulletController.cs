using System.Collections;
using System.Collections.Generic;
using UnityEngine;



//Declaration of delegate
public delegate void ProjectileHitEnemyHandler();

public class PistolBulletController : MonoBehaviour
{
    //explosion animation or particle
    public GameObject bulletExplosion;

    [SerializeField]
    private int pistolBulletDamage = 50;
    //[SerializeField]
    //private float bulletForce = 100f;


    //DECLARATION OF EVENT
    public event ProjectileHitEnemyHandler ProjectileHitEnemy;


    private void Start()
    {
       
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        //APPLY DAMAGE ON ENEMYCONTROLLER INSTANCE THAT WAS HIT
        if(collision.gameObject.CompareTag("Enemy"))
        {
            if(ProjectileHitEnemy != null)
            {
                ProjectileHitEnemy(); //Invoke the event.  We type it as if it was a method, with () at the end.
            }
            //apply damage
            collision.gameObject.GetComponent<EnemyController>().TakeDamage(pistolBulletDamage);

            //APPLY KNOCKBACK EFFECT
           // collision.gameObject.GetComponent<Rigidbody2D>().velocity =  //AddForce(gameObject.transform.forward * bulletForce * 100, ForceMode2D.Impulse);
        }

        //INSTANTIATE PISTOL BULLER EXPLOSION PARTICLES
        GameObject bulletExplosionEffect = Instantiate(bulletExplosion, transform.position, Quaternion.identity);//instantiate the particle or animation of bullet explosion on collision with anything.

        //Instead of Destroy(bulletExplosionEffect) I used a script on the particle called SelfDestruct
        //Destroy(bulletExplosionEffect, 4f);//Destroy the bullet explosion effect after 4 seconds
       
        //Destroy the bullet Prefab on collision with anything
        Destroy(gameObject);
        
    }
}
