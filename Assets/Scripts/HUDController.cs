using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HUDController : MonoBehaviour
{
    #region Variables

    public Canvas canvas;
    public Gradient healthBarColorGradient;
    public Gradient shieldBarColorGradient;
    public Gradient batteryBarColorGradient;
    public Slider healthSlider;
    public Slider shieldSlider;
    public Slider batterySlider;
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI playerShieldStatusText; // ON / OFF
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI scrapText;
    public TextMeshProUGUI playerHealthStatusText; //Healthy., In Business..., Critical !!!
    public TextMeshProUGUI reloadingText;
    public TextMeshProUGUI ammoText;
    public TextMeshProUGUI magazinesLeftText;
    public TextMeshProUGUI batteryText;

    public Image healthFillColorImage;
    public Image shieldFillColorImage;
    public Image batteryFillColorImage;

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        #region Event Subscriptions

        //Subscribing to the Shield is Draining Event.
        UtilitiesBroker.ShieldIsBurning += ShieldBroker_ShieldIsBurning;

        //Subscribing to the Shield is Enabled Event.
        UtilitiesBroker.ShieldIsEnabled += ShieldBroker_ShieldIsEnabled;

        //Subscribing to the Shield is Disabled Event.
        UtilitiesBroker.ShieldIsDisabled += ShieldBroker_ShieldIsDisabled;

        //Subscribing to the Shield is Depleted Event.
        UtilitiesBroker.ShieldIsDepleted += ShieldBroker_ShieldIsDepleted;

        //Subscribing to the Battery is Draining Event.
        UtilitiesBroker.FlashlightIsBurning += UtilitiesBroker_FlashlightIsBurning;

        //Subscribing to the reload weapon event in the ReloadWeapon broker class
        ReloadWeaponBroker.WeaponIsReloading += ReloadWeaponBroker_WeaponIsReloading;

        //Subscribing to the reload weapon Finished event in the ReloadWeapon broker class
        ReloadWeaponBroker.WeaponFinishedReloading += ReloadWeaponBroker_WeaponFinishedReloading;

        //Subscribing to the ammo update event in the AmmoDisplay broker class
        AmmoDisplayBroker.UpdateAmmoOnHud += AmmoDisplayBroker_UpdateAmmoOnHud;

        //Subscribing to the Magazines update event in the AmmoDisplay broker class
        AmmoDisplayBroker.UpdateMagazinesOnHud += AmmoDisplayBroker_UpdateMagazinesOnHud;

        //Subscribing to game scene's controller update health on hud
        GameSceneController.Instance.UpdateHealthOnDamage += UpdateHUDHealthOnDamage;

        //Subscribing to game scene's controller update score on hud
        GameSceneController.Instance.UpdateScoreOnKill += UpdateHUDOnKill;

        #endregion

        #region HUD Initialization

        //Initialize the HEALTH HUD SLIDER with color and value and lastly the health status text.
        playerHealthStatusText.text = "Healthy.";
        healthSlider.maxValue = 100;
        healthFillColorImage.color = healthBarColorGradient.Evaluate(healthSlider.normalizedValue);

        //Initialize the SHIELD HUD SLIDER with color and value and lastly the shield status text.
        playerShieldStatusText.text = "OFF...";
        shieldSlider.maxValue = 100;
        shieldFillColorImage.color = shieldBarColorGradient.Evaluate(shieldSlider.normalizedValue);

        //Initialize the BATTERY HUD SLIDER with color and value and lastly the shield status text.
        batteryText.text = "ALKALINE";
        batterySlider.maxValue = 100;
        batteryFillColorImage.color = shieldBarColorGradient.Evaluate(batterySlider.normalizedValue);

        #endregion
    }

    private void UpdateHUDHealthOnDamage(int currPlayerHP)
    {
        UpdateHealth(currPlayerHP);
    }

    private void UpdateHUDOnKill(int pointValue, int scrapValue)
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

    private void UtilitiesBroker_FlashlightIsBurning(float batteryLeft)
    {
        UpdateBattery(batteryLeft);
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

    private void UpdateBattery(float batteryRemaining)
    {
        batterySlider.value = batteryRemaining;
        //normalized value because the Gradient.evaluate goes from 0f - 1f but our health might be 25 or 100 or w/e.
        batteryFillColorImage.color = batteryBarColorGradient.Evaluate(batterySlider.normalizedValue);
    }
}
