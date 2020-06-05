using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketLauncherController : SelectedWeaponController
{

    public GameObject RocketRadiusImage;
    public GameObject rocket;

    private int totalRockets = 10;
    [SerializeField]
    private float rocketForce = 2f;
    [SerializeField]
    private float rocketReloadSpeed = 4f;

    private int rocketsInMagazine = 1; // check if weapon loaded
    private bool launcherIsReloading = false; // check if weapon is reloading

 //?????????????  private Vector3 mouseAimLocation; need to apply AOE DAMAGE with the launcher



    private void OnEnable()
    {   //if player interrupted the reload this is a check to see if the gun had bullets inside
        launcherIsReloading = false;
        AmmoDisplayBroker.CallUpdateAmmoOnHud(rocketsInMagazine, rocketsInMagazine);
        AmmoDisplayBroker.CallUpdateMagazinesOnHud(totalRockets);
    }


    // Start is called before the first frame update
    void Start()
    {
        rocketsInMagazine = 1;
        //Update bullet count and total rockets on HUD
        AmmoDisplayBroker.CallUpdateAmmoOnHud(rocketsInMagazine, rocketsInMagazine);
        AmmoDisplayBroker.CallUpdateMagazinesOnHud(totalRockets);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && gameObject.activeSelf)
        {
            ShootRocket();
           // mouseAimLocation = Camera.main.ScreenToWorldPoint(Input.mousePosition);//make the rocket travel to the mouse point when fired in order to create an OverlapCircle and damage enemies in AOE.
           // DrawRocketImpactPosition(mouseAimLocation);
        }
        if(gameObject.activeSelf && launcherIsReloading == false && Input.GetKeyDown(KeyCode.R))
        {
            Debug.Log("RELOADING LAUNCHER YOU PRESSED R");
            launcherIsReloading = true;
            StartCoroutine(reloadRocket());
        }
    }

    //Shooting the Pistol
    private void ShootRocket()
    {
        if (rocketsInMagazine > 0 && launcherIsReloading == false)
        {
            //Instantiate the Rocket prefab at the firepoint's gameObject position
            GameObject rocketPrefab = Instantiate(rocket, firePoint.position, firePoint.rotation);
            Rigidbody2D rocketRigidbody = rocketPrefab.GetComponent<Rigidbody2D>();
            rocketRigidbody.AddForce(firePoint.right * rocketForce, ForceMode2D.Impulse);

            //Set the rocket Color to black
            SpriteRenderer rocketSprite = rocketPrefab.GetComponent<SpriteRenderer>();
            rocketSprite.color = Color.black;

            rocketsInMagazine = 0;

            //Update bullet count on HUD
            AmmoDisplayBroker.CallUpdateAmmoOnHud(rocketsInMagazine, rocketsInMagazine); //totalRockets used to be "rocketsInMagazine". Because now the HUD shows 1/10 instead of 1/1 in the ammo counter. But it turns red on critical ammo. Which to keep?
        }
    }
    private void OnDisable()
    {
        //if player changes weapon mid reload
        StopCoroutine(reloadRocket());
    }

    private IEnumerator reloadRocket()
    {
        if (totalRockets > 0)
        {
            //call brokers method weapon is reloading
            ReloadWeaponBroker.CallWeaponIsReloading();

            yield return new WaitForSecondsRealtime(rocketReloadSpeed);
            
            totalRockets--;
            launcherIsReloading = false;
            rocketsInMagazine = 1;

            //Update bullet and magazines count on HUD
            AmmoDisplayBroker.CallUpdateAmmoOnHud(rocketsInMagazine, rocketsInMagazine);
            AmmoDisplayBroker.CallUpdateMagazinesOnHud(totalRockets);

            Debug.LogError("ROCKET LAUNCHER RELOADED");
        }
        else
        {
            Debug.Log("NO AMMO LEFT. RUN !!!");
        }
    }

    /*private void DrawRocketImpactPosition(Vector3 impactPosition)
    {
        if (GameObject.FindGameObjectWithTag("RocketRadiusImage") == null)
        { 
            GameObject rocketImpactObject = Instantiate(RocketRadiusImage, impactPosition, Quaternion.identity);
        }
    }*/
}
