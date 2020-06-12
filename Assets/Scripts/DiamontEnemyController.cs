using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiamontEnemyController : MonoBehaviour
{
    [SerializeField]
    private GameObject enemyBullet;

    private int diamontPointValue = 20;

    private float diamontMoveSpeed = 2f;

    private float timeForNextStep = 2f;

    private float timeLeft = 2f;

    private float timeBetweenShots = 2f;

    private Vector2 diamontMovement;

    private Rigidbody2D diamontRigidBody;

    private bool patrolling;


    // Start is called before the first frame update
    void Start()
    {
        patrolling = true;

        diamontRigidBody = GetComponent<Rigidbody2D>();

        StartCoroutine(MoveDiamont());

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


    private IEnumerator MoveDiamont()
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
    }

   /* private IEnumerator DiamontShootsPlayer()
    {
        yield return new WaitForSeconds(timeBetweenShots);

        //Instantiate the bullet prefab at the firepoint's gameObject position
        GameObject bulletPrefab = Instantiate(enemyBullet, transform.position, transform.rotation);
        Rigidbody2D bulletRigidbody = bulletPrefab.GetComponent<Rigidbody2D>();
        bulletRigidbody.AddForce(firePoint.right * bulletForce, ForceMode2D.Impulse);
    }*/
}

