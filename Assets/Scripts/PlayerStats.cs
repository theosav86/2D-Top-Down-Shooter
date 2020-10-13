using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//build with events from the player controller to update stats
//MOVE SPEED
//ARMOR?
//POWERUP
//INVENTORY???

//Declaration of player took damage delegate
public delegate void PlayerTookDamageHandler(float damageValue);
public class PlayerStats : MonoBehaviour
{
    //event declaration to update player health
    public event PlayerTookDamageHandler UpdateHUDHealthOnDamage;

    // HEALTH  
    public float playerMaxHealth = 100f;
    public float playerCurrentHealth;

    [SerializeField]
    private bool isImmortal = true;

    public float playerMoveSpeed = 7f;

    // Start is called before the first frame update
    void Start()
    {
        if (isImmortal == true)
        {
            playerCurrentHealth = 10000000f;
       // playerCurrentHealth = playerMaxHealth;
        }
        else
        {
            playerCurrentHealth = playerMaxHealth;
        }

        //Subscribe to playerTookDamageEvent
        PlayerEvents.PlayerTookDamage += PlayerEvents_PlayerTookDamage;
    }

    private void PlayerEvents_PlayerTookDamage(float dmg)
    {
        TakeHealthDamageStats(dmg);
        
    }

    private void TakeHealthDamageStats(float damageValue)
    {
        playerCurrentHealth -= damageValue;

        if (UpdateHUDHealthOnDamage != null)
        {
            UpdateHUDHealthOnDamage(playerCurrentHealth);
        }

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
