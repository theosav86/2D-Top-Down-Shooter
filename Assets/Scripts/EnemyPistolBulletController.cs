using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPistolBulletController : MonoBehaviour
{
    [SerializeField]
    private int pistolBulletDamage = 50;


    private void OnCollisionEnter2D(Collision2D other)
    {
        PlayerController player = other.collider.GetComponent<PlayerController>();

        if(player != null)
        {
            //PlayerTookDamage(pistolBulletDamage);
            Debug.LogError("PLAYER WAS HIT");
        }

        Destroy(gameObject);
    }
}
