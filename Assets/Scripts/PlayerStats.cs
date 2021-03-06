﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//build with events from the player controller to update stats
//MOVE SPEED
//ARMOR?
//POWERUP
//INVENTORY???

public class PlayerStats : Singleton<PlayerStats>, IDamagable
{
    // HEALTH  
    public int playerMaxHealth = 100;
    private int playerCurrentHealth;

    //SCRAP
    public int playerCurrentScrap = 0;
    private int playerMaxScrap = 9999;

    //SCORE
    public int playerCurrentScore = 0;
    

    [SerializeField]
    private bool isImmortal = true;

    public float playerMoveSpeed = 7f;

    // Start is called before the first frame update
    void Start()
    {
        if (isImmortal == true)
        {
            playerCurrentHealth = 999999;
            playerCurrentScrap = 9999;
        }
        else
        {
            playerCurrentHealth = playerMaxHealth;
        }

        //subscribing to the event EnemyKilled
        EnemyBroker.EnemyKilled += UpdateScore;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Enemy enemy = collision.collider.GetComponent<Enemy>();

        if (enemy != null)
        {
            TakeDamage(enemy.collisionDamage);
        }
    }

    private void UpdateScore(int pointValue, int scrapValue)
    {
        // add point value to HUD
        playerCurrentScore += pointValue;
        playerCurrentScrap += scrapValue;

        if(playerCurrentScrap >= playerMaxScrap)
        {
            playerCurrentScrap = playerMaxScrap;
        }

        //Invoking Update Score/Scrap events to update the HUD
        PlayerEvents.CallUpdatePlayerScore(playerCurrentScore); 
        PlayerEvents.CallUpdatePlayerScrap(playerCurrentScrap);   
    }

    //IDamagable Function
    public void TakeDamage(int damageValue)
    {
        playerCurrentHealth -= damageValue;

        PlayerEvents.CallPlayerTookDamage(damageValue);

        PlayerEvents.CallPlayerRemainingHP(playerCurrentHealth);

        if (playerCurrentHealth <= 0)
        {
            PlayerDies();
        }
    }

    private void PlayerDies()
    {
        PlayerEvents.CallPlayerDied();
        Time.timeScale = 0;
        Destroy(gameObject);
        print("You died...");
    }

    
}
