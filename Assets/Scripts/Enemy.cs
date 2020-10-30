using UnityEngine;

public class Enemy : MonoBehaviour, IDamagable
{

    public int collisionDamage = 30;
    public virtual void Initialize(Transform[] patrolPointsArray) { }
    public virtual void TakeDamage(int damageValue) { }
    public virtual void EnemyDies() { }
}
