using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketLauncherController : SelectedWeaponController
{

    public GameObject RocketRadiusImage;
    public GameObject rocket;

    private AudioSource rocketSound;

    [SerializeField]
    private AudioClip rocketShotClip;

    [SerializeField]
    private AudioClip rocketEmptyClip;

    [SerializeField]
    private AudioClip rocketReloadClip;

    private int currentRockets = 10;

    private int totalAllowedRockets = 10;
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
        AmmoDisplayBroker.CallUpdateMagazinesOnHud(currentRockets);
    }


    // Start is called before the first frame update
    void Start()
    {
        rocketSound = GetComponent<AudioSource>();

        rocketsInMagazine = 1;
        //Update bullet count and total rockets on HUD
        AmmoDisplayBroker.CallUpdateAmmoOnHud(rocketsInMagazine, rocketsInMagazine);
        AmmoDisplayBroker.CallUpdateMagazinesOnHud(currentRockets);
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
        if (Input.GetKeyDown(KeyCode.R) || Input.GetKeyDown(KeyCode.LeftControl))
        {
            if (gameObject.activeSelf && launcherIsReloading == false)
            {
                Debug.Log("RELOADING LAUNCHER YOU PRESSED R");
                launcherIsReloading = true;
                StartCoroutine(reloadRocket());
            }
        }
    }

    //Shooting the Pistol
    private void ShootRocket()
    {
        if (rocketsInMagazine > 0 && launcherIsReloading == false)
        {
            //Play Sound of rocket launcher shooting
            rocketSound.PlayOneShot(rocketShotClip);

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
        else
        {
            rocketSound.PlayOneShot(rocketEmptyClip);
        }
    }

    //Update Ammo
    public void UpdateTotalMagazines(int magazine)
    {
        currentRockets += magazine;

        if (currentRockets > totalAllowedRockets)
        {
            currentRockets = totalAllowedRockets;
        }

        AmmoDisplayBroker.CallUpdateMagazinesOnHud(currentRockets);
    }

    private void OnDisable()
    {
        //if player changes weapon mid reload
        StopCoroutine(reloadRocket());
    }

    private IEnumerator reloadRocket()
    {
        if (currentRockets > 0)
        {

            // Play Sound of rocket launcher reloading
            rocketSound.PlayOneShot(rocketReloadClip);

            //call brokers method weapon is reloading
            ReloadWeaponBroker.CallWeaponIsReloading();

            yield return new WaitForSecondsRealtime(rocketReloadSpeed);
            
            currentRockets--;
            launcherIsReloading = false;
            rocketsInMagazine = 1;

            //Update bullet and magazines count on HUD
            AmmoDisplayBroker.CallUpdateAmmoOnHud(rocketsInMagazine, rocketsInMagazine);
            AmmoDisplayBroker.CallUpdateMagazinesOnHud(currentRockets);

            Debug.LogError("ROCKET LAUNCHER RELOADED");

            ReloadWeaponBroker.CallWeaponFinishedReloading();
        }
        else
        {
            Debug.Log("NO AMMO LEFT. RUN !!!");

            ReloadWeaponBroker.CallWeaponFinishedReloading();

        }
    }
}
