using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiamondDetection : MonoBehaviour
{
    private DiamontEnemyController diamondEnemy;

    private void Start()
    {
        diamondEnemy = GetComponentInParent<DiamontEnemyController>();
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        PlayerDetection playerDetection = other.GetComponent<PlayerDetection>();

        if (playerDetection != null)
        {
            diamondEnemy.playerSpotted = true;
            diamondEnemy.playerTransform = other.transform;

        }
    }
}
