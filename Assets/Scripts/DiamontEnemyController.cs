using System.Collections;
using System.Collections.Generic;
using UnityEngine;



//ANOTHER ENEMY TYPE CLASS NOT YET IMPLEMENTED (DIAMONT ENEMY).

//ENEMY SHOULD MOVE INSIDE THE STAGE AND STOP TO SHOOT A PROJECTILE TOWARDS THE PLAYER.



public class DiamontEnemyController : Enemy
{
    public Transform firePoint;
    public LayerMask checkWallsLayer;
    public LayerMask checkPlayerLayer;

    [SerializeField]
    private GameObject enemyBullet;

    private int diamontPointValue = 20;

    private float diamontMoveSpeed = 2f;

    private float timeForNextStep = 2f;

    private float timeLeft = 2f;

    private Vector2 diamontMovement;

    private Rigidbody2D diamontRigidBody;

    private bool canShoot = true;
    private float rateOfFire = 2f;
    private float reloadSpeed = 3f;

    private float checkRadius = 4f;

    public float checkRange = 10f;


    // Start is called before the first frame update
    void Start()
    {
        //patrolling = true;

        diamontRigidBody = GetComponent<Rigidbody2D>();

      //  StartCoroutine(MoveDiamont());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
     //   MoveDiamont();
    }

   /* private void SetNextRandomPoint()
    {
      //  RaycastHit2D checkForNextPoint = Physics2D.Raycast(firePoint.position, transform.right, checkRange);
       // if(checkForNextPoint.transform.CompareTag("Wall") )
    }*/

    private void CheckArea()
    {
        if(Physics2D.OverlapCircle(transform.position, checkRadius, checkPlayerLayer))
        {
            if(canShoot)
            {
                Instantiate(enemyBullet, firePoint.transform.position, Quaternion.identity);
                canShoot = false;
                Invoke(nameof(EnemyReload), reloadSpeed);
            }
        }
    }

    private void EnemyReload()
    {
        canShoot = true;
    }

    /*private IEnumerator MoveDiamont()
    {
        while (patrolling)
        {
           // Ray diamontRay = new Ray(transform.position, transform.forward * Random.Range(-1, 1));

            RaycastHit2D diamontHit;

            diamontHit = Physics2D.Raycast(transform.position, transform.forward * Random.Range(-1, 1));

            if (diamontHit.collider.CompareTag("Wall"))
            {
                yield return new WaitForSeconds(timeForNextStep);
            }
            else
                diamontRigidBody.MovePosition(diamontHit.point * Time.deltaTime * diamontMoveSpeed); //(new Vector2(transform.position.x + Random.Range(-1, 1), transform.position.y + Random.Range(-1, 1)));

            yield return new WaitForSeconds(timeForNextStep);

            Debug.Log(diamontRigidBody.position);
        }
    }*/

   /* private IEnumerator DiamontShootsPlayer()
    {
        yield return new WaitForSeconds(timeBetweenShots);

        //Instantiate the bullet prefab at the firepoint's gameObject position
        GameObject bulletPrefab = Instantiate(enemyBullet, transform.position, transform.rotation);
        Rigidbody2D bulletRigidbody = bulletPrefab.GetComponent<Rigidbody2D>();
        bulletRigidbody.AddForce(firePoint.right * bulletForce, ForceMode2D.Impulse);
    } */
}
