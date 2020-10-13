﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerController : MonoBehaviour
{
    #region Variables

    private Rigidbody2D playerRigidbody;
    private PlayerStats playerStats;
    private Vector2 axisInput;
    private int enemyCollisionDamageValue = 30;

    //select weapon variables
    private SelectedWeaponController selectedWeapon;

    #endregion


    //declaration of the event enemy killed
    public event PlayerTookDamageHandler PlayerTookDamage;

    private void Awake()
    {
        playerRigidbody = GetComponent<Rigidbody2D>();
        playerStats = GetComponent<PlayerStats>();
        selectedWeapon = GetComponentInChildren<SelectedWeaponController>(); // so we have access in variable firePoint for example. I AM NOT USING THIS ONE. MAYBE DELETE IT .
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

    }

    private void FixedUpdate()
    {
        playerRigidbody.MovePosition(playerRigidbody.position + axisInput * playerStats.playerMoveSpeed * Time.fixedDeltaTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.CompareTag("Enemy"))
        {
            if (PlayerTookDamage != null)
            {
                PlayerTookDamage(enemyCollisionDamageValue);
            }
            
            //  PlayerTakesDamage(50);
            Destroy(collision.gameObject);
        }
    }

}