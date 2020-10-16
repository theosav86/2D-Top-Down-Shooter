using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//build with events from the player controller to update stats
//MOVE SPEED
//ARMOR?
//POWERUP
//INVENTORY???

public class PlayerStats : MonoBehaviour
{
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
    }

    public void TakeHealthDamageStats(float damageValue)
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
