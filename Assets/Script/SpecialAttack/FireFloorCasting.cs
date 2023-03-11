using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireFloorCasting : MonoBehaviour
{
    public GameObject indicator;
    public float explodeDuration;
    public float count = -10f;
    public float radius;
    public EnemyController enemyController;
    public Enemy enemy;
    private bool hasExplode = false;
    private bool isCasting = true;
    private LayerMask mask;

    public void Awake()
    {
        mask = LayerMask.GetMask("Entity");
    }

    public void Update()
    {
        count += Time.deltaTime;
        if(count >= explodeDuration)
        {
            Explode();
        }
    }

    public void SetUp(float newExplodeDuration,float newRadius,EnemyController newEnemyController, Enemy newEnemy)
    {
        explodeDuration = newExplodeDuration;
        radius = newRadius;
        enemyController = newEnemyController;
        enemy = newEnemy;
        count = 0;
    }

    public void Explode()
    {
        Vector3 start = transform.position;
        Collider[] colliders = Physics.OverlapCapsule(new Vector3(start.x,start.y + 1,start.z),new Vector3(start.x,start.y - 1,start.z),
            radius,mask);
        foreach (var collider in colliders)
        {
            PlayerController playerController = collider.gameObject.GetComponent<PlayerController>();
            if (playerController != null)
            {
                if (enemy.Attack(playerController) && playerController == enemyController.focusCharacter)
                {
                    enemyController.focusCharacter = null;
                }
            }
        }
        Destroy(gameObject);
    }
}
