using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireFloorCasting : MonoBehaviour
{
    public GameObject effectPrefab;
    public float explodeDuration;
    public float count = -10f;
    public float radius;
    public EnemyController enemyController;
    public Enemy enemy;
    private bool hasExplode = false;
    private bool isCasting = true;
    private LayerMask mask;
    private int areaIdx;

    private GameController gameController;

    public void Awake()
    {
        mask = LayerMask.GetMask("Entity");
        gameController = GameObject.Find("GameLogic").GetComponent<GameController>();
    }

    public void Update()
    {
        count += Time.deltaTime;
        if(count >= explodeDuration)
        {
            Explode();
        }
    }

    public void SetUp(float newExplodeDuration,float newRadius,int newAreaIdx,EnemyController newEnemyController, Enemy newEnemy)
    {
        explodeDuration = newExplodeDuration;
        radius = newRadius;
        enemyController = newEnemyController;
        enemy = newEnemy;
        areaIdx = newAreaIdx;
        count = 0;
    }

    public void Explode()
    {
        Vector3 start = transform.position;
        Collider[] colliders = Physics.OverlapCapsule(new Vector3(start.x,start.y + 1,start.z),new Vector3(start.x,start.y - 1,start.z),
            radius,mask);
        // Debug.Log("bomb :" + start);
        // Debug.Log("enemy :" + enemy.transform.position);
        // Debug.Log("player :" + gameController.characters[0].transform.position);
        bool f = false;
        foreach (var player in gameController.areas[areaIdx].areaCharacter)
        {
            if (Vector2.Distance(new Vector2(player.transform.position.x,player.transform.position.z),new Vector2(start.x,start.z)) <= radius)
            {
                if (enemy.Attack(player) && player == enemyController.focusCharacter)
                {
                    enemyController.focusCharacter = null;
                }
            }
            // if (playerController != null)
            // {
            //     if (enemy.Attack(playerController) && playerController == enemyController.focusCharacter)
            //     {
            //         enemyController.focusCharacter = null;
            //     }
            // }
        }
        //Debug.Log(f);
        // if (!f)
        // {
        //      Debug.Log(Vector2.Distance(new Vector2(start.x,start.z),new Vector2(gameController.characters[0].transform.position.x,gameController.characters[0].transform.position.z)));
        // }
        Instantiate(effectPrefab, transform.position, transform.rotation);
        Destroy(gameObject);
    }
}
