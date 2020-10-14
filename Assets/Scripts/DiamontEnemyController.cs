using UnityEngine;

//ENEMY SHOULD MOVE INSIDE THE STAGE AND STOP TO SHOOT A PROJECTILE TOWARDS THE PLAYER.

public class DiamontEnemyController : Enemy
{
    #region Variables

    private int diamondHP = 120;

    private int pointValue = 20;

    private int scrapValue = 5;

    public Transform firePoint;
    public LayerMask checkWallsLayer;
    public LayerMask checkPlayerLayer;

    [SerializeField]
    private GameObject enemyBullet;

    private int diamontPointValue = 20;

    private float diamontMoveSpeed = 2f;

    private float timeForNextStep = 2f;

    private Vector2 enemyDirection;

    private Rigidbody2D diamontRigidBody;

    private bool isPatrolling = true;
    public bool playerSpotted = false;

    private bool canShoot = true;

    private float rateOfFire = 1f;

    private float reloadSpeed = 1f;

    private float checkRadius = 12f;

    public float checkRange = 10f;

    private Transform[] patrolPoints;

    private int patrolPointIndex = 0;

    private float closestPoint = 100f;

    [HideInInspector]
    public Transform playerTransform = null;

    [SerializeField]
    [Range(0f, 2f)]
    private float bulletForce = 0.3f;

    #endregion

    public override void Initialize(Transform[] patrolPointsArray)
    {
        patrolPoints = new Transform[patrolPointsArray.Length];

        for (int i = 0; i < patrolPointsArray.Length; i++)
        {
            patrolPoints[i] = patrolPointsArray[i];
        }

        FindStartingPatrolPoint();

        SetNextDestination();
    }

    private void Awake()
    {
        closestPoint = 100f;

        diamontRigidBody = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if(isPatrolling)
        {
            this.transform.position = Vector2.MoveTowards(transform.position, patrolPoints[patrolPointIndex].position, diamontMoveSpeed * Time.fixedDeltaTime);

            if(Vector2.Distance(transform.position, patrolPoints[patrolPointIndex].position) < 0.1f)
            {
          
                SetNextDestination();
            }
        }

        if (playerSpotted)
        {
            //look at the player
            enemyDirection = playerTransform.position - transform.position;

            float angle = Mathf.Atan2(enemyDirection.y, enemyDirection.x) * Mathf.Rad2Deg;

            diamontRigidBody.rotation = angle;

            //Shoot at the player
            ShootPlayer();
        }
        
    }

    private void FindStartingPatrolPoint()
    {
        for(int i = 0; i < patrolPoints.Length; i++)
        {
            if(Vector2.Distance(transform.position, patrolPoints[i].position) < closestPoint)
            {
                closestPoint = Vector2.Distance(transform.position, patrolPoints[i].position);
                patrolPointIndex = i;
            }
        }
    }

    private void SetNextDestination()
    {
        patrolPointIndex++;

        if(patrolPointIndex >= patrolPoints.Length)
        {
            patrolPointIndex = 0;
        }
    }

 

   /* private void OnTriggerStay2D(Collider2D other)
    {
        PlayerController player = other.GetComponent<PlayerController>();

        if(player != null)
        {
            playerTransform = other.transform;

            //look at the player
            enemyDirection = playerTransform.position - transform.position;
            float angle = Mathf.Atan2(enemyDirection.y, enemyDirection.x) * Mathf.Rad2Deg;
            diamontRigidBody.rotation = angle;

            playerSpotted = true;
            
        }
        else
        {
            playerSpotted = false;
            playerTransform = null;
        }
    }*/

    private void ShootPlayer()
    {
        if(playerSpotted && canShoot)
        {
            CheckArea();
        }
    }

    private void CheckArea()
    {
        if (Physics2D.OverlapCircle(transform.position, checkRadius, checkPlayerLayer))
        {
            if (canShoot)
            {
                if(playerTransform != null)
                {
                    GameObject bullet = Instantiate(enemyBullet, firePoint.transform.position, firePoint.rotation);
                    Rigidbody2D bulletRB = bullet.GetComponent<Rigidbody2D>();
                    bulletRB.AddForce((playerTransform.position - this.transform.position) * bulletForce, ForceMode2D.Impulse);
                }
                
                canShoot = false;
                Invoke(nameof(EnemyReload), reloadSpeed);
            }
        }

    }
    private void EnemyReload()
    {
        canShoot = true;
    }

    public override void TakeDamage(int damageValue)
    {
        diamondHP -= damageValue;

        if(diamondHP <= 0)
        {
            EnemyDies();
        }
    }

    public override void EnemyDies()
    {
        EnemyBroker.CallEnemyKilled(pointValue, scrapValue);

        Destroy(gameObject);
    }


    /* private IEnumerator DiamontShootsPlayer()
     {
         yield return new WaitForSeconds(timeBetweenShots);

         //Instantiate the bullet prefab at the firepoint's gameObject position
         GameObject bulletPrefab = Instantiate(enemyBullet, transform.position, transform.rotation);
         Rigidbody2D bulletRigidbody = bulletPrefab.GetComponent<Rigidbody2D>();
         bulletRigidbody.AddForce(firePoint.right * bulletForce, ForceMode2D.Impulse);
     } */
}
