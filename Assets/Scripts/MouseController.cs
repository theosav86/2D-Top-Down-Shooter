using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseController : MonoBehaviour
{
    #region Variables

    //private variables for Aiming
    private Vector2 mousePosition;
    private Vector2 playerPosition;
    private Vector2 lookDirection;
    private Transform aimPoint;
    private LayerMask enemyLayer = 12;
    [SerializeField]
    private Camera sceneMainCamera;

    private Rigidbody2D playerRigidbody;


    public Texture2D[] cursors;

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        sceneMainCamera = Camera.main;
        playerRigidbody = GetComponent<Rigidbody2D>();
        PlayerController playerController = GetComponent<PlayerController>();
        playerPosition = playerController.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        //Position of the mouse cursor Vector 2
        //position of the camera Input mousePosition
        mousePosition = sceneMainCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.z - sceneMainCamera.transform.position.z));
        
        //Check if the player is aiming
        if (Input.GetMouseButtonDown(1))
        {
            AimSelectedWeapon();
        }

        //Cancel the aim animation
        if (Input.GetMouseButtonUp(1))
        {
            
        }

        //Check if the player is shooting. Alternative can be GetButtonDown("Fire1");
        if (Input.GetMouseButtonDown(0))
        {
         
        }
        
    }

    private void FixedUpdate()
    {
        //Rotation of the player according to mouse position (mousePos - playerPos)
        lookDirection = mousePosition - playerRigidbody.position;
        

        //This is radiants. Need to change it to degrees. This is why we use Rad2Deg.
        float angle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg; //Atabn2 method takes Y coordinate first then the X coordinate.
        playerRigidbody.rotation = angle;
       
    }

    private void AimSelectedWeapon()
    {

    }
}