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
    public int playerMaxHealth = 100;
    public int playerCurrentHealth;

    [SerializeField]
    private bool isImmortal = true;


    private PlayerController playerController;

    // Start is called before the first frame update
    void Start()
    {
        if (isImmortal == true)
        {
            playerCurrentHealth = 10000000;
       // playerCurrentHealth = playerMaxHealth;
        }
        else
        {
            playerCurrentHealth = playerMaxHealth;
            
        }


        playerController = GetComponent<PlayerController>();
        playerController.PlayerTookDamage += PlayerController_PlayerTookDamage; //PlayerController Class subscribes to the event PlayerTookDamage
    }

    private void PlayerController_PlayerTookDamage(int damageValue)
    {
        UpdateHealthStats(damageValue);
    }

    

    // Update is called once per frame
    void Update()
    {
        
    }

    private void UpdateHealthStats(int damageValue)
    {
        playerCurrentHealth -= damageValue;

        if (playerCurrentHealth <= 0)
        {
            PlayerDies();
        }
    }

    private void PlayerDies()
    {
        Time.timeScale = 0;
        Destroy(gameObject);
        print("You died...");
    }
}
