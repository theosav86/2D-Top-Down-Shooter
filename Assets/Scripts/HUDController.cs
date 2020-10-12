using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HUDController : MonoBehaviour
{
    #region Variables
    private GameSceneController gameSceneController;

    public Canvas canvas;
    public Gradient healthBarColorGradient;
    public Gradient shieldBarColorGradient;
    public Slider healthSlider;
    public Slider shieldSlider;
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI playerShieldStatusText; // ON / OFF
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI scrapText;
    public TextMeshProUGUI playerHealthStatusText; //Healthy., In Business..., Critical !!!
    public TextMeshProUGUI reloadingText;
    public TextMeshProUGUI ammoText;
    public TextMeshProUGUI magazinesLeftText;

    public Image healthFillColorImage;
    public Image shieldFillColorImage;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        //Actually Create HealthBroker for sure!
        //PlayerHealthBroker.playertookdamage etc.

        //Subscribing to the Shield is Draining Event.
        ShieldBroker.ShieldIsBurning += ShieldBroker_ShieldIsBurning;

        //Subscribing to the Shield is Enabled Event.
        ShieldBroker.ShieldIsEnabled += ShieldBroker_ShieldIsEnabled;

        //Subscribing to the Shield is Disabled Event.
        ShieldBroker.ShieldIsDisabled += ShieldBroker_ShieldIsDisabled;

        //Subscribing to the Shield is Depleted Event.
        ShieldBroker.ShieldIsDepleted += ShieldBroker_ShieldIsDepleted;

        //Subscribing to the reload weapon event in the ReloadWeapon broker class
        ReloadWeaponBroker.WeaponIsReloading += ReloadWeaponBroker_WeaponIsReloading;

        //Subscribing to the reload weapon Finished event in the ReloadWeapon broker class
        ReloadWeaponBroker.WeaponFinishedReloading += ReloadWeaponBroker_WeaponFinishedReloading;

        //Subscribing to the ammo update event in the AmmoDisplay broker class
        AmmoDisplayBroker.UpdateAmmoOnHud += AmmoDisplayBroker_UpdateAmmoOnHud;

        //Subscribing to the Magazines update event in the AmmoDisplay broker class
        AmmoDisplayBroker.UpdateMagazinesOnHud += AmmoDisplayBroker_UpdateMagazinesOnHud;

        //Initialize the HUD SLIDER with color and value and lastly the health status text.
        playerHealthStatusText.text = "Healthy.";
        healthSlider.maxValue = 100;
        healthFillColorImage.color = healthBarColorGradient.Evaluate(healthSlider.normalizedValue);

        //Initialize the HUD SHIELD SLIDER with color and value and lastly the shield status text.
        playerShieldStatusText.text = "OFF...";
        healthSlider.maxValue = 100;
        shieldFillColorImage.color = shieldBarColorGradient.Evaluate(shieldSlider.normalizedValue);

        //Reference to GameSceneController in order to subscribe to its events
        gameSceneController = FindObjectOfType<GameSceneController>();

        //The class HUDController subscribes to the eevent gamescenecontroller update score on kill. It could be any other class as long as there is a reference to game scene controller class. get component<ClassName>
        gameSceneController.UpdateScoreOnKill += GameSceneController_UpdateScoreOnKill1;
        //The class HUDController subscribes to the event gamescenecontroller update health on damage. It could be any other class as long as there is a reference to game scene controller class. get component<ClassName>
        gameSceneController.UpdateHealthOnDamage += GameSceneController_UpdateHealthOnDamage;

    }

    private void GameSceneController_UpdateScoreOnKill1(int pointValue, int scrapValue)
    {
        UpdateScore(pointValue, scrapValue);
    }

    private void ShieldBroker_ShieldIsDepleted()
    {
        ShieldIsDepleted();
    }

    private void ShieldBroker_ShieldIsBurning(float timeLeft)
    {
        UpdateShield(timeLeft);
    }
    private void ShieldBroker_ShieldIsDisabled()
    {
        //Toggle the Text to Off...
        playerShieldStatusText.text = "OFF...";
        playerShieldStatusText.color = Color.black;
    }

    private void ShieldBroker_ShieldIsEnabled()
    {
        //Toggle the Text to On
        playerShieldStatusText.text = "ON!";
        playerShieldStatusText.color = Color.red;
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
        reloadingText.GetComponent<Animator>().SetBool("PlayReload", true);
    }
    private void ReloadWeaponBroker_WeaponFinishedReloading()
    {
        reloadingText.GetComponent<Animator>().SetBool("PlayReload", false);
    }

    private void GameSceneController_UpdateHealthOnDamage(int damageValue)
    {
        UpdateHealth(damageValue);
    }

    private void GameSceneController_UpdateScoreOnKill(int pointValue, int scrapValue)
    {
        UpdateScore(pointValue, scrapValue);
    }

    //method to update the score WHEN an enemy is killed
    private void UpdateScore(int pointValue, int scrapValue)
    {
        scoreText.text = "Score: " + pointValue.ToString("D6");
        scrapText.text = "Scrap: " + scrapValue.ToString("D4");
    
    }

    //method to update the score WHEN player takes damage
    private void UpdateHealth(int damageValue)
    {
        healthSlider.value = damageValue;

        //normalized value because the Gradient.evaluate goes from 0f - 1f but our health might be 25 or 100 or w/e.
        healthFillColorImage.color = healthBarColorGradient.Evaluate(healthSlider.normalizedValue);

        //maybe getting obsolete. bar is nicer than text
        healthText.text = "Health: " + damageValue.ToString("D3");


        //UPDATE THE HUD SLIDER TEXT.
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

      
    private void UpdateShield(float shieldValue)
    {
        shieldSlider.value = shieldValue;
        //normalized value because the Gradient.evaluate goes from 0f - 1f but our health might be 25 or 100 or w/e.
        shieldFillColorImage.color = shieldBarColorGradient.Evaluate(shieldSlider.normalizedValue);
    }

    private void ShieldIsDepleted()
    {
            shieldSlider.value = 0f;
            shieldFillColorImage.color = shieldBarColorGradient.Evaluate(shieldSlider.normalizedValue);

            //Change the on/off Text to Depleted
            playerShieldStatusText.text = "Depleted!!!";
            playerShieldStatusText.color = Color.red;
    }
     
}
