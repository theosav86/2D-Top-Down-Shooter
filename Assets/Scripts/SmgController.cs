﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SmgController : SelectedWeaponController
{

    private AudioSource smgSound;

    [SerializeField]
    private AudioClip smgShotClip;
    [SerializeField]
    private AudioClip smgEmptyClip;
    [SerializeField]
    private AudioClip smgReloadClip;

    private int smgDamage = 30;
    private float smgRange = 5f;
    [SerializeField]
    private float smgReloadSpeed = 2.5f;
    private float nextShotTime;
    [SerializeField]
    private float rateOfFire = 13f;
    [SerializeField]
    private int smgMagazineSize = 30;
    private int bulletsInMagazine = 30;
    private int totalMagazines = 5;
    private bool smgIsReloading;
    //[SerializeField]
    //private LineRenderer lineRenderer;


    private void OnEnable()
    {   //if player interrupted the reload
        
        smgIsReloading = false;
        AmmoDisplayBroker.CallUpdateAmmoOnHud(bulletsInMagazine, smgMagazineSize);
        AmmoDisplayBroker.CallUpdateMagazinesOnHud(totalMagazines);

      //lineRenderer = GetComponent<LineRenderer>();
    }

    // Start is called before the first frame update
    void Start()
    {
        smgSound = GetComponent<AudioSource>();

        //set initial bullets in magazine
        bulletsInMagazine = smgMagazineSize;

        //Update bullet count on HUD
        AmmoDisplayBroker.CallUpdateAmmoOnHud(bulletsInMagazine, smgMagazineSize);
        AmmoDisplayBroker.CallUpdateMagazinesOnHud(totalMagazines);
    }


    // Update is called once per frame
    void Update()
    {
        //check if SMG is selected && mouse button pressed && time has passed to fire again && smg is not reloading 
        if (gameObject.activeSelf && smgIsReloading == false && Input.GetMouseButton(0) && Time.time > nextShotTime)
        {
            if (bulletsInMagazine > 0)
            {
                ShootSMG();
                nextShotTime = Time.time + 1f/rateOfFire; // limit the fire rate of the weapon to the value of rateOfFire
                
            }
            
        }
        if (Input.GetKeyDown(KeyCode.R) || Input.GetKeyDown(KeyCode.LeftControl))
        {
            //check if the player is pressing R/ctrl to reload. If the weapon isn't already reloading the following is executed
            if (gameObject.activeSelf && smgIsReloading == false)
            {
                Debug.Log("RELOADING SMG YOU PRESSED R");
                smgIsReloading = true;
                StartCoroutine(ChangeMagazine());
            }
        }
    }

    private void OnDisable()
    {
        //if player changes weapon mid reload
        StopCoroutine(ChangeMagazine());
    }

    //Method that shoots ray for the SMG
    private void ShootSMG()
    {
        if (bulletsInMagazine > 0)
        {

            //play smg shot sound
            smgSound.PlayOneShot(smgShotClip);


            //SHOOT
            RaycastHit2D smgHit = Physics2D.Raycast(firePoint.position, firePoint.right, smgRange);
            //lineRenderer.SetPosition(0,firePoint.position);
            //lineRenderer.SetPosition(1,firePoint.right * smgRange);
            bulletsInMagazine--;

            //Update bullet count on HUD
            AmmoDisplayBroker.CallUpdateAmmoOnHud(bulletsInMagazine, smgMagazineSize);

            //if enemy hit damage it
            if (smgHit)
            {
                EnemyController enemyController = smgHit.collider.gameObject.GetComponent<EnemyController>();
                if (enemyController != null)
                {
                    enemyController.TakeDamage(smgDamage);
                }

            }  
        }
        else
        {
            //PLAY EMPTY GUN SOUND CLICK CLICK CLICK
            smgSound.PlayOneShot(smgEmptyClip);
        }

       
        Debug.DrawRay(transform.position, transform.right * smgRange, Color.red);
        Debug.Log("SMG NEW BULLET");
    }


    private IEnumerator ChangeMagazine()
    {
        if (totalMagazines > 0)
        {
            //call the method in the broker to invoke the reload event for the hud controller
            ReloadWeaponBroker.CallWeaponIsReloading();

            //Play Reload Sound
            smgSound.PlayOneShot(smgReloadClip);

            yield return new WaitForSecondsRealtime(smgReloadSpeed);
            totalMagazines--;
            smgIsReloading = false;
            bulletsInMagazine = 30;

            //Update bullet and magazine count on HUD
            AmmoDisplayBroker.CallUpdateAmmoOnHud(bulletsInMagazine, smgMagazineSize);
            AmmoDisplayBroker.CallUpdateMagazinesOnHud(totalMagazines);

            Debug.LogError("SMG RELOADED");

            ReloadWeaponBroker.CallWeaponFinishedReloading();
        }
        else
        {
            Debug.LogError("SMG OUT OF AMMO");

            ReloadWeaponBroker.CallWeaponFinishedReloading();
        }
    }

}


