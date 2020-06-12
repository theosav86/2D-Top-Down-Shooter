using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class ShieldController : MonoBehaviour
{
    private Animator shieldAnimator;

    private SpriteRenderer shieldSpriteRenderer;

    private BoxCollider2D shieldCollider;

    private float shieldHp = 100;

    [SerializeField]
    private float shieldDuration = 100f;

    [SerializeField]
    private float shieldTimeLeft;
  
    [SerializeField]
    private bool isShieldActive = false;


    private void Awake()
    {
        shieldSpriteRenderer = GetComponent<SpriteRenderer>();
        shieldCollider = GetComponent<BoxCollider2D>();
        shieldAnimator = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        DisableShield();

        ShieldBroker.CallShieldIsDisabled();

        ShieldBroker.ShieldTookDamage += ShieldBroker_ShieldTookDamage;

        shieldTimeLeft = shieldDuration;
    }

    

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if(!isShieldActive)
            {
                EnableShield(); // maybe better to invoke event shieldisenabled and enable the shield and burn its duration from shield stats.
            }
            else
            {
                DisableShield(); // disable the shield with an event. probably better.
            }
        }

        if (isShieldActive)
        {
            ShieldTimeBurning();
        }
    }

    private void EnableShield()
    {
        shieldSpriteRenderer.enabled = true;
        shieldCollider.enabled = true;
        isShieldActive = true;

        ShieldBroker.CallShieldIsEnabled();
    }

    private void DisableShield()
    {
        shieldSpriteRenderer.enabled = false;
        shieldCollider.enabled = false;
        isShieldActive = false;

        ShieldBroker.CallShieldIsDisabled();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(isShieldActive)
        {
            if(collision.collider.CompareTag("Enemy"))
            { 
                Destroy(collision.gameObject); // For testing purpose just destroy the enemy on contact
            }
        }
    }
    private void ShieldTimeBurning()
    {
        if (shieldTimeLeft > 0)
        {
            shieldTimeLeft -= Time.deltaTime;

            ShieldBroker.CallShieldIsBurning(shieldTimeLeft);
        }
        else
        {
            //What to do when shield time is depleted
            isShieldActive = false;
            DisableShield();
        }
    }
    private void ShieldBroker_ShieldTookDamage(float damageValue)
    {
        shieldHp -= damageValue;
        shieldTimeLeft -= damageValue;
    }

    private IEnumerator InitialShieldFlashing()
    {
        isShieldActive = true;

        shieldSpriteRenderer.enabled = true;
        shieldCollider.enabled = true;

        shieldAnimator.SetBool("shieldIsFlashing", true);

        yield return new WaitForSecondsRealtime(3);

        shieldAnimator.SetBool("shieldIsFlashing", false);

        shieldSpriteRenderer.enabled = false;
        shieldCollider.enabled = false;

        isShieldActive = false;
    }
}
