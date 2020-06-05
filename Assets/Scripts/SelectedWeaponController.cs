using UnityEngine;

public class SelectedWeaponController : MonoBehaviour
{
    public GameObject[] weapons;
    public int currentWeapon;
    public Transform firePoint; //visible in the Inspector to place the gameObject FirePoint inside the Player hierarchy

    public Cursor[] cursors;
    
    // Start is called before the first frame update
    void Start()
    {
        currentWeapon = 0;
        weapons[0].SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {        
            if (Input.GetButtonDown("Pistol"))
            {
                currentWeapon = 0;
                weapons[0].SetActive(true);

                weapons[1].SetActive(false);
                weapons[2].SetActive(false);

        }
            if (Input.GetButtonDown("SMG"))
            {
                currentWeapon = 1;
                weapons[1].SetActive(true);

                weapons[0].SetActive(false);
                weapons[2].SetActive(false);

        }
            if (Input.GetButtonDown("Rocket"))
            {
                currentWeapon = 2;
                weapons[2].SetActive(true);

                weapons[0].SetActive(false);
                weapons[1].SetActive(false);
        }

    }
        
        
}
