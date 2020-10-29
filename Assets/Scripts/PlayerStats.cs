using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//build with events from the player controller to update stats
//MOVE SPEED
//ARMOR?
//POWERUP
//INVENTORY???

public class PlayerStats : Singleton<PlayerStats>
{
    // HEALTH  
    public int playerMaxHealth = 100;
    private int playerCurrentHealth;

    //SCRAP
    private int playerMaxScrap;
    public int playerCurrentScrap;

    //SCORE
    public int playerCurrentScore;
    

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

            playerCurrentScrap = playerMaxScrap;
        }

        //subscribing to the event EnemyKilled
        EnemyBroker.EnemyKilled += UpdateScore;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Enemy enemy = collision.collider.GetComponent<Enemy>();

        if (enemy != null)
        {
            TakeHealthDamage(enemy.collisionDamage);
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
        PlayerEvents.CallUpdatePlayerScore(playerCurrentScrap);   
    }

    public void TakeHealthDamage(int damageValue)
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
