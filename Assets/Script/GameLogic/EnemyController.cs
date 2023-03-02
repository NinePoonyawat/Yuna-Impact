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

    [SerializeField] private UIEnemyProfile uiEnemy;

    public void Awake()
    {
        gameController = GameObject.Find("GameLogic").GetComponent<GameController>();
        uiEnemy = GetComponentInChildren<UIEnemyProfile>();
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
                enemy.CallAttack(focusCharacter);
            }
            else
            {
                entityState = EntityState.PREATTACK;
            }
            return true;
        }
        return false;
    }

    public void MeleeAttack()
    {
        if (Attack(focusCharacter)) focusCharacter = null;
        entityState = EntityState.ATTACK;
        enemy.AfterAttack(); // get attack cooldown;
    }

    public void RangeAttack()
    {
        GameObject GO = Instantiate(enemy.GetPrefab(),gameController.projectileRoot) as GameObject;
        Projectile projectile = GO.GetComponent<Projectile>();
        projectile.SetUp(this,focusCharacter,enemy.GetProjectileSpeed());
        entityState = EntityState.ATTACK;
        enemy.AfterAttack(); // get attack cooldown;
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
            UpdateDirection();
        }
    }

    public override bool TakeDamage(int damage,AttackType attackType)
    {
        if (enemy.TakeDamage(damage,attackType))
        {
            gameController.DeleteEnemy(this);
            Destroy(gameObject);
            return true;
        };
        uiEnemy.UpdateHPBar(enemy.GetStatusValue());
        return false;
    }

    public override bool Attack(EntityController player)
    {
        return player.TakeDamage(enemy.GetAttack(),enemy.attackType);
    }
}
