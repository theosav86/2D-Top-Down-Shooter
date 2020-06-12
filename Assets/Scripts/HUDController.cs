using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HUDController : MonoBehaviour
{
    private GameSceneController gameSceneController;

    public Canvas canvas;
    public Gradient healthBarColorGradient;
    public Slider healthSlider;
    public Image healthFillColorImage;
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI playerHealthStatusText; //Healthy., In Business..., Critical !!!
    public TextMeshProUGUI reloadingText;
    public TextMeshProUGUI ammoText;
    public TextMeshProUGUI magazinesLeftText;

    // Start is called before the first frame update
    void Start()
    {
        //Subscribing to the reload weapon event in the ReloadWeapon broker class
        ReloadWeaponBroker.WeaponIsReloading += ReloadWeaponBroker_WeaponIsReloading;

        //Subscribing to the ammo update event in the AmmoDisplay broker class
        AmmoDisplayBroker.UpdateAmmoOnHud += AmmoDisplayBroker_UpdateAmmoOnHud;

        //Subscribing to the Magazines update event in the AmmoDisplay broker class
        AmmoDisplayBroker.UpdateMagazinesOnHud += AmmoDisplayBroker_UpdateMagazinesOnHud;

        //Initialize the HUD SLIDER with color and value and lastly the health status text. Stupid stuff right here
        playerHealthStatusText.text = "Healthy.";
        healthSlider.maxValue = 100;
        healthFillColorImage.color = healthBarColorGradient.Evaluate(healthSlider.normalizedValue);

        //Reference to GameSceneController in order to subscribe to its events
        gameSceneController = FindObjectOfType<GameSceneController>();

        //The class HUDController subscribes to the event gamescenecontroller update score on kill. It could be any other class as long as there is a reference to game scene controller class. get component<ClassName>
        gameSceneController.UpdateScoreOnKill += GameSceneController_UpdateScoreOnKill;
        //The class HUDController subscribes to the event gamescenecontroller update health on damage. It could be any other class as long as there is a reference to game scene controller class. get component<ClassName>
        gameSceneController.UpdateHealthOnDamage += GameSceneController_UpdateHealthOnDamage;

    }

    
    private void AmmoDisplayBroker_UpdateMagazinesOnHud(int magazinesLeft)
    {
        magazinesLeftText.text = "Magazines Left " + magazinesLeft;
    }

    private void AmmoDisplayBroker_UpdateAmmoOnHud(int ammoInMagazine, int magazineSize)
    {
        ammoText.text = "Ammo: " + ammoInMagazine + " / " + magazineSize;
        
        double criticalAmmoPercentage = (double)ammoInMagazine / magazineSize;

        if(magazineSize == 0) // safety check to never divide with zero 
        {
            ammoText.color = Color.red;
        }
        else if (criticalAmmoPercentage <= 0.33) //example with SMG. Divide 30 (magazine size) with 10 (bullets in the mag) you get 3 so its the 1/3 of the mag. go critical ammo! 
        {
            ammoText.color = Color.red;
        }
        else
        {
            ammoText.color = Color.white;

        }
    }

    private void ReloadWeaponBroker_WeaponIsReloading()
    {
        reloadingText.GetComponent<Animator>().SetTrigger("PlayReload");
    }

    private void GameSceneController_UpdateHealthOnDamage(int damageValue)
    {
        UpdateHealth(damageValue);
    }

    private void GameSceneController_UpdateScoreOnKill(int pointValue)
    {
        UpdateScore(pointValue);
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    //method to update the score WHEN an enemy is killed
    private void UpdateScore(int pointValue)
    {
        scoreText.text = "Score: " + pointValue.ToString("D6");
    
    }

    //method to update the score WHEN player takes damage
    private void UpdateHealth(int damageValue)
    {
        healthSlider.value = damageValue;

        //normalized value because the Gradient.evaluate goes from 0f - 1f but our health might be 25 or 100 or w/e.
        healthFillColorImage.color = healthBarColorGradient.Evaluate(healthSlider.normalizedValue);


        healthText.text = "Health: " + damageValue.ToString("D3");


        //UPDATE THE HUD SLIDER TEXT. STUPID STUFF RIGHT HERE AS WELL!!!
        if (damageValue > 70)
        {
            playerHealthStatusText.text = "Healthy.";
        }
        else if (damageValue < 25)
        {
            playerHealthStatusText.text = "Critical !!!";
        }
        else
        {
            playerHealthStatusText.text = "In Busines...";
        }
    }

    /* need to create an UpdateShield Method
     * 
     * private void UpdateShield(int shieldValue)
     * {
     *      shieldFillColorImage.value = shieldValue;
     *      
     *      //if I want I can create a shieldText but not really necessary
     * 
        }
     */
}
