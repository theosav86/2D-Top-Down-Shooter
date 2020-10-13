using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPistolBulletController : MonoBehaviour
{

    private float enemyPistolBulletDamage = 5f;


    private void OnCollisionEnter2D(Collision2D other)
    {
        PlayerController player = other.collider.GetComponent<PlayerController>();

        if(player != null)
        {
            PlayerEvents.CallPlayerTookDamage(enemyPistolBulletDamage);
            Debug.LogError("PLAYER WAS HIT BY PISTOL");
        }

        Destroy(gameObject);
    }
}
