using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketProjectileController : MonoBehaviour
{

    public GameObject rocketExplosion;

    [SerializeField]
    private int rocketDamage = 100;
 // /*[SerializeField]
 //   private float rocketSpeed = 100f;
    [SerializeField]
    private float rocketSplashRadius = 3.0f;

   // private GameObject target;
  

    //declaration of event rockethitenemy
   // public event ProjectileHitEnemyHandler RocketHitEnemy;

    // Start is called before the first frame update
    void Start()
    {
     //   target = GameObject.FindGameObjectWithTag("RocketRadiusImage");
    }

    /*private void FixedUpdate()
    {
        transform.position = Vector3.MoveTowards(transform.position, target.transform.position, rocketSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, target.transform.position) < 1f)
        {

            Collider2D[] targetColliders = Physics2D.OverlapCircleAll(transform.position, rocketSplashRadius, 12);


            foreach (Collider2D collider in targetColliders)
            {
                if (collider.gameObject != null)
                { 
                    collider.gameObject.GetComponent<EnemyController>().TakeDamage(rocketDamage);
                }
            }

            Destroy(gameObject);
            Destroy(target.gameObject);
        }
    }*/


    private void OnCollisionEnter2D(Collision2D collision)
    {
        Collider2D[] collidersHitByRocketSplash;
        collidersHitByRocketSplash = Physics2D.OverlapCircleAll(transform.position, rocketSplashRadius);

        foreach(Collider2D collider in collidersHitByRocketSplash)
        {
            if(collider.CompareTag("Enemy"))
            {
                collider.GetComponent<Enemy>().TakeDamage(rocketDamage);
            }
        }



        //APPLY DAMAGE ON ENEMYCONTROLLER INSTANCE THAT WAS HIT
        if (collision.gameObject.CompareTag("Enemy"))
        {
            /* if (RocketHitEnemy != null)
             {
                 RocketHitEnemy(); //Invoke the event.  We type it as if it was a method, with () at the end.
             }
             */

             //apply damage
            //Debug.Log("Rocket HIT ENEMY COLLIDER METHOD IN PROJECTILE");
            collision.gameObject.GetComponent<Enemy>().TakeDamage(rocketDamage);

        }

        //INSTANTIATE ROCKET BULLET EXPLOSION PARTICLES
        GameObject rocketExplosionEffect = Instantiate(rocketExplosion, transform.position, Quaternion.identity);//instantiate the particle or animation of bullet explosion on collision with anything.

        //Instead of Destroy(bulletExplosionEffect) I used a script on the particle called SelfDestruct
        //Destroy(bulletExplosionEffect, 4f);//Destroy the bullet explosion effect after 4 seconds

        //Destroy the bullet Prefab on collision with anything
        Destroy(gameObject);

    }

}
