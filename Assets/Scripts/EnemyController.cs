using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public delegate void EnemyKilledHandler(int pointValue);
public class EnemyController : MonoBehaviour
{
    //explosion animation or particle
    public GameObject enemyExplosion;

    public int enemyHealth = 100;
    public float enemyMoveSpeed = 1f;

    //private Animator enemyAnimatorController;

    [SerializeField]
    private int pointValue = 10;
    private Transform playerTransform;
    private Vector3 enemyDirection;
    private Rigidbody2D enemyRigidBody;
    private GameSceneController gameSceneController;

    //declaration of the event enemy killed
    public event EnemyKilledHandler EnemyKilled;

    // Start is called before the first frame update
    void Start()
    {
        enemyRigidBody = GetComponent<Rigidbody2D>();
        playerTransform = FindObjectOfType<PlayerController>().transform;
        gameSceneController = FindObjectOfType<GameSceneController>();
        //enemyAnimatorController = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
       
    }

    private void FixedUpdate()
    {
        EnemyFollowsPlayer();

    }

    private void EnemyFollowsPlayer()
    {
        //look at the player
        enemyDirection = playerTransform.position - transform.position;
        float angle = Mathf.Atan2(enemyDirection.y, enemyDirection.x) * Mathf.Rad2Deg;
        enemyRigidBody.rotation = angle;

        //move towards the position of the player
        enemyRigidBody.MovePosition(transform.position + enemyDirection * enemyMoveSpeed * Time.deltaTime);

    }

    public void TakeDamage(int damageValue)
    {
        enemyHealth -= damageValue;

        if(enemyHealth <= 0)
        {
            EnemyDies();
        }
    }

    public void EnemyDies()
    {
        if(EnemyKilled != null)
        {
            EnemyKilled(pointValue); //EnemyKilled event invokation
        }

        //Debug.Log("Enemy Destroyed");
        //enemyAnimatorController.SetTrigger("enemyDied");
        Destroy(gameObject);
    }

}

