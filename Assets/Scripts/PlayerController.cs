using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerController : MonoBehaviour
{
    #region Variables

    private Rigidbody2D playerRigidbody;
    private PlayerStats playerStats;
    private Vector2 axisInput;

    //select weapon variables
    public GameObject weaponHolder;

    public GameObject flashLight;
    private bool flashLightEnabled = false;
    public float flashLightBatteryLife = 100f;
    private float maxFlashLightBatteryLife = 100f;

    public float interactRadius = 2f;
    public LayerMask interactLayer;
    
    private MouseController mouseController;

    public bool isUsing = false;

    private IInteractable item;

    private PlayerBaseState currentState;

    public readonly PlayerNormalState normalState = new PlayerNormalState();
    public readonly PlayerUseState useState = new PlayerUseState();

    #endregion

    private void Awake()
    {
        playerRigidbody = GetComponent<Rigidbody2D>();
        playerStats = GetComponent<PlayerStats>();
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

        if (Input.GetButtonDown("Use"))
        {
            Use();
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
    }

    public void Use()
    {

        Collider2D interactable;
        interactable = Physics2D.OverlapCircle(transform.position, interactRadius, interactLayer);
        item = interactable.GetComponent<IInteractable>();

        if(item != null)
        {
            if (!isUsing)
            {
                isUsing = true;
                weaponHolder.SetActive(false);
                item.UseInteractable();
            }
            else
            {
                isUsing = false;
                weaponHolder.SetActive(true);
                item.StopUseInteractable();
                item = null;
            }
        }
        else
        {
            Debug.Log("No interactable found");
        }
        
    }

    public void TransitionToState(PlayerBaseState newState)
    {
        currentState = newState;
        PlayerEvents.CallPlayerChangedState(currentState);
        currentState.EnterState(this);
    }
}