using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldController : MonoBehaviour
{
    private Animator shieldAnimator;

    private SpriteRenderer shieldSpriteRenderer;

    private BoxCollider2D shieldCollider;

    private float shieldHp = 100f;

    [SerializeField]
    private float shieldDuration = 100f;

    [SerializeField]
    private float shieldTimeLeft;
  
    [SerializeField]
    private bool isShieldActive = false;

    private float shieldCriticalPercentage = 0.2f;
    private bool isShieldCritical = false;


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

        UtilitiesBroker.CallShieldIsDisabled();

        UtilitiesBroker.ShieldTookDamage += ShieldBroker_ShieldTookDamage;

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

        UtilitiesBroker.CallShieldIsEnabled();
    }

    private void DisableShield()
    {
        shieldSpriteRenderer.enabled = false;
        shieldCollider.enabled = false;
        isShieldActive = false;

        UtilitiesBroker.CallShieldIsDisabled();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Enemy enemy = collision.collider.GetComponent<Enemy>();

        if(isShieldActive)
        {
            if(enemy != null)
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

            UtilitiesBroker.CallShieldIsBurning(shieldTimeLeft);

            shieldAnimator.SetBool("shieldIsFlashing", false);

            if(shieldTimeLeft < shieldDuration * shieldCriticalPercentage)
            {
                Debug.Log("Shield critical health!!!");
                shieldAnimator.SetBool("shieldIsFlashing", true);
            }


        }
        else
        {
            //What to do when shield time is depleted
            DisableShield();
            shieldAnimator.SetBool("shieldIsFlashing", false);
            UtilitiesBroker.CallShieldIsDepleted();
        }
    }
    private void ShieldBroker_ShieldTookDamage(float damageValue)
    {
        shieldHp -= damageValue;
        shieldTimeLeft -= damageValue;
    }

   /* private IEnumerator InitialShieldFlashing()
    {
        isShieldActive = true;

        shieldSpriteRenderer.enabled = true;
        shieldCollider.enabled = true;

        shieldAnimator.SetBool("shieldIsFlashing", true);

        yield return new WaitForSecondsRealtime(3f);

        shieldAnimator.SetBool("shieldIsFlashing", false);

        shieldSpriteRenderer.enabled = false;
        shieldCollider.enabled = false;

        isShieldActive = false;
    }
   */
}
