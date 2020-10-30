using System.Collections;
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
    [HideInInspector]
    public SelectedWeaponController weaponHolder;

    public GameObject flashLight;
    private bool flashLightEnabled = false;
    public float flashLightBatteryLife = 100f;
    private float maxFlashLightBatteryLife = 100f;

    public float interactRadius = 2f;
    public LayerMask interactLayer;
    
    public bool canInteract = false;

    private bool movementIsEnabled = true;
    private MouseController mouseController;

    public IInteractable item;

    private PlayerBaseState currentState;

    public readonly PlayerNormalState normalState = new PlayerNormalState();
    public readonly PlayerShopState useState = new PlayerShopState();

    #endregion

    private void Awake()
    {
        playerRigidbody = GetComponent<Rigidbody2D>();
        playerStats = GetComponent<PlayerStats>();
        weaponHolder = GetComponentInChildren<SelectedWeaponController>(); // so we have access in variable firePoint for example. I AM NOT USING THIS ONE. MAYBE DELETE IT .
        mouseController = GetComponent<MouseController>();
        flashLightBatteryLife = maxFlashLightBatteryLife;
        flashLight.SetActive(false);
    }

    private void Start()
    {
        TransitionToState(normalState);
    }

    //Getter for currentState
    public PlayerBaseState CurrentState
    {
        get { return currentState; }
    }

    // Update is called once per frame
    void Update()
    {
        currentState.Update(this);

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


        
    }

    private void FixedUpdate()
    {
        playerRigidbody.MovePosition(playerRigidbody.position + axisInput * playerStats.playerMoveSpeed * Time.fixedDeltaTime);
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

    public void Use()
    {

        Collider2D[] interactables;

        interactables = Physics2D.OverlapCircleAll(transform.position, interactRadius);

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

    public void TransitionToState(PlayerBaseState newState)
    {
        currentState = newState;
        PlayerEvents.CallPlayerChangedState(currentState);
        currentState.EnterState(this);
    }
}