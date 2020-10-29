using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class GameSceneController : Singleton<GameSceneController>
{
    #region Variables

    public int level = 1;
    public Transform[] enemySpawnPoints;
    public int numberOfEnemies = 50;

    [SerializeField]
    private float enemySpawnDelay = 0.5f;

    public Transform[] diamontPatrolPoints;

    [SerializeField]
    private PlayerController playerController;

    public Levels[] levels;

    private int levelIndex;

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        levelIndex = 1;
 
        //Start spawning of enemies
        StartCoroutine(SpawnEnemies());
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
        ////    StalkerController enemy = Instantiate(stalkerEnemyPrefab, randomEnemySpawnPosition.position, Quaternion.identity);
            // enemy.gameObject.layer = LayerMask.NameToLayer("Enemy");
     //       Mathf.Clamp()
            Enemy enemy = Instantiate(levels[levelIndex].enemyTypes[Random.Range(0, levels[levelIndex].enemyTypes.Length)], randomEnemySpawnPosition.position, Quaternion.identity);

            enemy.Initialize(diamontPatrolPoints);

            // enemy.shotSpeed = currentLevel.enemyShotSpeed;
            // enemy.speed = currentLevel.enemySpeed;
            // enemy.shotdelayTime = currentLevel.enemyShotDelay;
            // enemy.angerdelayTime = currentLevel.enemyAngerDelay;            

            yield return wait;
        }
    }

}
