﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerController : MonoBehaviour
{
    #region Variables

    private Rigidbody2D playerRigidbody;
    private PlayerStats playerStats;
    private Vector2 axisInput;
    private float enemyCollisionDamageValue = 30;

    //select weapon variables
    private SelectedWeaponController selectedWeapon;

    public GameObject flashLight;
    private bool flashLightEnabled = false;
    public float flashLightBatteryLife = 100f;
    private float maxFlashLightBatteryLife = 100f;

    public float interactRadius = 2f;
    public LayerMask interactLayer;
    public bool canInteract = false;

    #endregion


    //declaration of the event enemy killed
    public event PlayerTookDamageHandler PlayerTookDamage;

    private void Awake()
    {
        playerRigidbody = GetComponent<Rigidbody2D>();
        playerStats = GetComponent<PlayerStats>();
        selectedWeapon = GetComponentInChildren<SelectedWeaponController>(); // so we have access in variable firePoint for example. I AM NOT USING THIS ONE. MAYBE DELETE IT .
        flashLightBatteryLife = maxFlashLightBatteryLife;
        flashLight.SetActive(false);
    }
    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        axisInput.x = Input.GetAxisRaw("Horizontal");
        axisInput.y = Input.GetAxisRaw("Vertical");

        if(Input.GetButtonDown("Flashlight"))
        {
            ToggleFlashLight();
        }

        if(flashLight.activeSelf)
        {  
            BatteryLifeDraining();
        }


        if(Input.GetButtonDown("Use") && canInteract)
        {
            Use();  
        }
    }

    private void FixedUpdate()
    {
        playerRigidbody.MovePosition(playerRigidbody.position + axisInput * playerStats.playerMoveSpeed * Time.fixedDeltaTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.CompareTag("Enemy"))
        {
             PlayerEvents.CallPlayerTookDamage(enemyCollisionDamageValue);

            //  PlayerTakesDamage(50);
            Destroy(collision.gameObject);
        }
    }

    private void ToggleFlashLight()
    {
        flashLightEnabled = !flashLightEnabled;
        flashLight.SetActive(flashLightEnabled);
    }

    private void BatteryLifeDraining()
    {
        flashLightBatteryLife -= Time.deltaTime;

        //Invoke the flashlight is draining event
        UtilitiesBroker.CallFlashlightIsBurning(flashLightBatteryLife);

        if(flashLightBatteryLife <= 0)
        {
            flashLightEnabled = false;
            flashLight.SetActive(false);
            flashLightBatteryLife = 0;

            //Invoke the flashlight is depleted event
            UtilitiesBroker.CallFlashlightIsDepleted();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        IInteractable interactable = collision.GetComponent<IInteractable>();

        if(interactable != null)
        {
            canInteract = true;
        }
        else
        {
            canInteract = false;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {

        IInteractable interactable = collision.GetComponent<IInteractable>();

        if (interactable != null)
        {
            canInteract = false;
        }
    }

    private void Use()
    {

        Collider2D[] interactables;

        interactables = Physics2D.OverlapCircleAll(transform.position, interactRadius);

        Debug.Log(interactables);

        IInteractable item = null;

        foreach(Collider2D interactable in interactables)
        {
            item = interactable.GetComponent<IInteractable>();

            if (item != null)
            {
                item.UseInteractable();
                Debug.Log(item);
            }
        }
        
    }

}