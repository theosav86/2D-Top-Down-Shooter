using UnityEngine;

[CreateAssetMenu(fileName = "level", menuName ="level")]
public class Levels : ScriptableObject
{
    public int levelOrder;

    public int enemyCount;

    public float spawnTimer;
}
