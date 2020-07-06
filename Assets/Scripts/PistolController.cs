﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class PistolController : SelectedWeaponController
{

    public GameObject bullet; //visible in the Inspector to place the bullet prefab itself.

    private AudioSource pistolSound;

    [SerializeField]
    private AudioClip pistolShotClip;

    [SerializeField]
    private AudioClip pistolEmptyClip;

    [SerializeField]
    private AudioClip pistolReloadClip;


    [SerializeField]
    private const int pistolRange = 20;

    private int totalMagazines = 10;

    private int magazineSize = 12;
    [SerializeField]
    private float pistolReloadSpeed = 1.5f; //in seconds
   
    //Pistol variables
    //bullet force visible in the Inspector
    [SerializeField]
    private float bulletForce = 3f;
        
    private bool pistolIsReloading = false;
    
    private int bulletsInMagazine = 12;

   

    private void OnEnable()
    {   //if player interrupted the reload this is a check to see if the gun had bullets inside the magazine.
        
        pistolIsReloading = false;
        AmmoDisplayBroker.CallUpdateAmmoOnHud(bulletsInMagazine, magazineSize);
        AmmoDisplayBroker.CallUpdateMagazinesOnHud(totalMagazines);
        
    }

    // Start is called before the first frame update
    void Start()
    {
        pistolSound = GetComponent<AudioSource>();

        bulletsInMagazine = magazineSize;

        //Update ammo and magazine count on HUD
        AmmoDisplayBroker.CallUpdateAmmoOnHud(bulletsInMagazine, magazineSize);
        AmmoDisplayBroker.CallUpdateMagazinesOnHud(totalMagazines);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && gameObject.activeSelf && pistolIsReloading == false)
        {
            ShootPistol();
        }
        if (Input.GetKeyDown(KeyCode.R) || Input.GetKeyDown(KeyCode.LeftControl))
        { 
            if (gameObject.activeSelf && pistolIsReloading == false)
            {
                Debug.Log("RELOADING PISTOL YOU PRESSED R");
                pistolIsReloading = true;
                StartCoroutine(ChangeMagazine());
            }
        }
    }

    private void OnDisable()
    {
        //if player changes weapon mid reload
        StopCoroutine(ChangeMagazine());
    }

    //Shooting the Pistol
    private void ShootPistol()
    {
       
        if (bulletsInMagazine > 0)
        {

            //Play Pistol Shooting Sound
            pistolSound.PlayOneShot(pistolShotClip);

            //remove 1 bullet from the magazine
            bulletsInMagazine--;

            //invoke event to update ammo count on HUD
            AmmoDisplayBroker.CallUpdateAmmoOnHud(bulletsInMagazine, magazineSize);

            //Instantiate the bullet prefab at the firepoint's gameObject position
            GameObject bulletPrefab = Instantiate(bullet, firePoint.position, firePoint.rotation);
            Rigidbody2D bulletRigidbody = bulletPrefab.GetComponent<Rigidbody2D>();
            bulletRigidbody.AddForce(firePoint.right * bulletForce, ForceMode2D.Impulse);

            //Set the bullet Color to red
            SpriteRenderer bulletSprite = bulletPrefab.GetComponent<SpriteRenderer>();
            bulletSprite.color = Color.red;
        }
        else
        {
            //WEAPON EMPTY SOUND
            pistolSound.PlayOneShot(pistolEmptyClip);
        }
    }


    
    private IEnumerator ChangeMagazine()
    {
        //yield return new WaitForSeconds(reloadSpeed);

        if (totalMagazines > 0)
        {
            //Pistol reload SOUND
            pistolSound.PlayOneShot(pistolReloadClip);

            //Update the HUD to notify the player that the gun is reloading
            ReloadWeaponBroker.CallWeaponIsReloading();

            totalMagazines--;

            //Wait for reload time
            yield return new WaitForSecondsRealtime(pistolReloadSpeed);

            pistolIsReloading = false;
            bulletsInMagazine = magazineSize;

            AmmoDisplayBroker.CallUpdateAmmoOnHud(bulletsInMagazine, magazineSize);
            AmmoDisplayBroker.CallUpdateMagazinesOnHud(totalMagazines);

            ReloadWeaponBroker.CallWeaponFinishedReloading();

        }
        else
        {
            Debug.Log("NO AMMO LEFT. RUN !!!");

            ReloadWeaponBroker.CallWeaponFinishedReloading();
        }
    }


}
