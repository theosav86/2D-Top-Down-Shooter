﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class StalkerController : Enemy
{
    //explosion animation or particle
    public GameObject enemyExplosion;

    public int enemyHealth = 100;
    public float enemyMoveSpeed = 1f;

    //private Animator enemyAnimatorController;

    [SerializeField]
    private int pointValue = 2;
    [SerializeField]
    private int scrapValue = 10;
    private Transform playerTransform;
    private Vector3 enemyDirection;
    private Rigidbody2D enemyRigidBody;

    private float shieldDamage = 20f;
    private int playerDamage = 50;

    // Start is called before the first frame update
    void Start()
    {
        enemyRigidBody = GetComponent<Rigidbody2D>();
        playerTransform = FindObjectOfType<PlayerController>().transform;
        //enemyAnimatorController = GetComponent<Animator>();
    }


    private void FixedUpdate()
    {
        EnemyFollowsPlayer();
    }
    

    private void EnemyFollowsPlayer()
    {
        //look at the player
        enemyDirection = playerTransform.position - transform.position;
        float angle = Mathf.Atan2(enemyDirection.y, enemyDirection.x) * Mathf.Rad2Deg;
        enemyRigidBody.rotation = angle;

        //move towards the position of the player
        enemyRigidBody.MovePosition(transform.position + enemyDirection * enemyMoveSpeed * Time.deltaTime);
    }

    public override void TakeDamage(int damageValue)
    {
        enemyHealth -= damageValue;

        if (enemyHealth <= 0)
        {
            EnemyDies();
        }
    }

    public override void EnemyDies()
    {
        EnemyBroker.CallEnemyKilled(pointValue, scrapValue);

        //The effect has a self destruct script. Otherwise I would have to destroy it here after instatiate.
        GameObject enemyExplosionEffect = Instantiate(enemyExplosion, transform.position, Quaternion.identity);

        //Debug.Log("Enemy Destroyed");
        //enemyAnimatorController.SetTrigger("enemyDied");
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        PlayerStats playerStats = collision.collider.GetComponentInParent<PlayerStats>();

        if(playerStats != null)
        {
            Debug.Log("APPLY DAMAGE TO PLAYER ");
            playerStats.TakeHealthDamageStats(playerDamage);
            Destroy(gameObject);
        }
    }

    //apply duration penalty (damage) on the shield
    private void OnTriggerEnter2D(Collider2D other)
    {
        ShieldController shield = other.GetComponent<ShieldController>();

        if (shield != null)
        {
            Debug.Log("APPLY DAMAGE TO SHIELD ");
            UtilitiesBroker.CallShieldTookDamage(shieldDamage);
            Destroy(gameObject);
        }
    }
}
