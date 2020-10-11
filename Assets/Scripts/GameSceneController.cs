using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameSceneController : MonoBehaviour
{
    //event declaration to update the score
    public event EnemyKilledHandler UpdateScoreOnKill;

    //event declaration to update player health
    public event PlayerTookDamageHandler UpdateHealthOnDamage;

  
    public int level = 1;
    public Transform[] enemySpawnPoints;
    public int numberOfEnemies = 50;
    [SerializeField]
    private float enemySpawnDelay = 0.5f;

    private int currentScore = 0;
    private int currentScrap = 0;
    private int currentHealth = 100;
    public StalkerController stalkerEnemyPrefab;

    [SerializeField]
    private PlayerController playerController;

    // Start is called before the first frame update
    void Start()
    {
        //playerController = FindObjectOfType<PlayerController>();
        playerController.PlayerTookDamage += PlayerController_PlayerTookDamage;
 
        //Start spawning of enemies
        StartCoroutine(SpawnEnemies());
    }

    //method that generated from the subscription of GameSceneController class to the PlayerTookDamage Event.
    private void PlayerController_PlayerTookDamage(int damageValue)
    {
        currentHealth -= damageValue;

        if (UpdateHealthOnDamage != null)
        {
            UpdateHealthOnDamage(currentHealth);//invoking new parameterized event UpdateHealthOnDamage
        }
    }

   //Coroutine that spawns enemies
    private IEnumerator SpawnEnemies()
    {
        WaitForSeconds wait = new WaitForSeconds(level + enemySpawnDelay);
        yield return wait;

        for (int i = 0; i < level * numberOfEnemies; i++)
        {
            //select random spawn point for the enemy
            int numberOfSpawnPoints = enemySpawnPoints.Length;
            int selectedRandomSpawnPoint = Random.Range(0, numberOfSpawnPoints);
            Transform randomEnemySpawnPosition = enemySpawnPoints[selectedRandomSpawnPoint];

            //create an instance of an enemy on random enemy spawn position
            StalkerController enemy = Instantiate(stalkerEnemyPrefab, randomEnemySpawnPosition.position, Quaternion.identity);
           // enemy.gameObject.layer = LayerMask.NameToLayer("Enemy");

            // enemy.shotSpeed = currentLevel.enemyShotSpeed;
            // enemy.speed = currentLevel.enemySpeed;
            // enemy.shotdelayTime = currentLevel.enemyShotDelay;
            // enemy.angerdelayTime = currentLevel.enemyAngerDelay;

            //subscribing to the event EnemyKilled
            enemy.EnemyKilled += Enemy_EnemyKilled;  

            yield return wait;
        }
    }


    //method generated from the subscription to the event EnemyKilled
    private void Enemy_EnemyKilled(int pointValue, int scrapValue)
    {

        //add point value to HUD
        currentScore += pointValue;
        currentScrap += scrapValue;

        if(UpdateScoreOnKill != null)
        {
            UpdateScoreOnKill(currentScore, currentScrap); //invoking new parameterized event UpdateScoreOnKill
        }
    }
}
