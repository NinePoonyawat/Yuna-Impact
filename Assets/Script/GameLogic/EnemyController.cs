using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : EntityController
{
    private GameController gameController;
    [SerializeField] private Enemy enemy;
    
    [SerializeField] protected PlayerController focusCharacter = null;
    [SerializeField] protected float maxVision = 20f;

    [SerializeField] private NavMeshAgent navMeshAgent;
    public void Awake()
    {
        gameController = GameObject.Find("GameLogic").GetComponent<GameController>();
    }

    public void Start()
    {
        gameController.AddEnemy(this);
        entityState = EntityState.IDLE;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateEnemy();
    }

    void UpdateEnemy()
    {
        //Update when focus character int attack range
        bool isAttack = UpdateAttack();

        //Update when character in the vision
        if(!isAttack) UpdateMoving();
    }

    bool UpdateAttack()
    {
        if (focusCharacter == null) return false;
        if(enemy.isInAttackRange(focusCharacter.transform.position))
        {
            if (enemy.IsAttackable())
            {
                Attack(focusCharacter);
                entityState = EntityState.ATTACK;
                enemy.Attack();
            }
            else
            {
                entityState = EntityState.PREATTACK;
            }
            return true;
        }
        return false;
    }

    void UpdateMoving()
    {
        if (focusCharacter == null)
        {
            focusCharacter = gameController.FindNearestCharacter(transform.position, maxVision);
            if (focusCharacter != null) navMeshAgent.SetDestination(focusCharacter.transform.position);
        }
        else
        {
            navMeshAgent.SetDestination(focusCharacter.transform.position);
        }
    }

    public override bool TakeDamage(int damage)
    {
        enemy.TakeDamage(damage);
        return true;
    }

    void Attack(PlayerController player)
    {
        Debug.Log("attack" + player);
    }
}
