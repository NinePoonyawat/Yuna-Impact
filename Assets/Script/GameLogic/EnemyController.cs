using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : EntityController,IPlayerClickable
{
    private GameController gameController;
    private Enemy enemy;
    public GameObject main;
    
    [SerializeField] protected PlayerController focusCharacter = null;
    [SerializeField] protected float maxVision = 20f;

    [SerializeField] private Animator animator;

    [SerializeField] private UIEnemyProfile uiEnemy;

    public void Awake()
    {
        enemy = gameObject.GetComponent<Enemy>();
        gameController = GameObject.Find("GameLogic").GetComponent<GameController>();
        uiEnemy = GetComponentInChildren<UIEnemyProfile>();
        animator = GetComponentInChildren<Animator>();
        if (main == null) main = transform.parent.gameObject;
    }

    public void Start()
    {
        agent.speed = enemy.defaultSpeed;
        if (currentArea != -1) gameController.AddEnemy(this,currentArea);
        entityState = EntityState.IDLE;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateDirection();
        UpdateState();
        GetNextState();
       // UpdateEnemy();
    }

    void UpdateState()
    {
        switch (entityState)
        {
            case EntityState.MOVE :
                if (focusCharacter != null) agent.SetDestination(focusCharacter.transform.position);
                UpdateDirection();
                break;
            case EntityState.ATTACK :
                //if (focusCharacter != null && enemy.CallAttack(focusCharacter)) focusCharacter = null;
                if (focusCharacter != null) agent.SetDestination((transform.position - focusCharacter.transform.position) + transform.position);
                agent.speed = enemy.walkbackSpeed;
                break;
        }
    }

    public void GetNextState()
    {
        switch (entityState)
        {
            case EntityState.IDLE :
                focusCharacter = gameController.FindNearestCharacter(transform.position,maxVision,currentArea);
                if (focusCharacter != null)
                {
                    if(animator != null) animator.SetBool("isWalk", true);
                    entityState = EntityState.MOVE;
                }
                break;
            case EntityState.MOVE :
                if (focusCharacter == null)
                {
                    entityState = EntityState.IDLE;
                    if(animator != null) animator.SetBool("isWalk", false);
                }
                else if (enemy.isInAttackRange(focusCharacter.transform.position) && enemy.isAttackable)
                {
                    entityState = EntityState.ATTACK;
                    if(animator != null) animator.SetTrigger("LAttack");
                }
                break;
            case EntityState.ATTACK :
                entityState = EntityState.PREATTACK;
                break;
            case EntityState.PREATTACK :
                if (focusCharacter == null)
                {
                    entityState = EntityState.IDLE;
                    agent.speed = enemy.defaultSpeed;
                }
                else
                {
                    if (enemy.isAttackable)
                    {
                        agent.speed = enemy.defaultSpeed;
                        if (enemy.isInAttackRange(focusCharacter.transform.position)) entityState = EntityState.ATTACK;
                        else entityState = EntityState.MOVE;
                    }
                }
                break;
        }
    }

    public void AttackFocus()
    {
        if (enemy.CallAttack(focusCharacter)) focusCharacter = null;
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
        if(focusCharacter != null && enemy.isInAttackRange(focusCharacter.transform.position))
        {
            if (enemy.isAttackable)
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

    public override void AfterAttack()
    {
         if(focusCharacter != null)
         {
            agent.SetDestination((transform.position - focusCharacter.transform.position) + transform.position);
         }
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
        if(focusCharacter != null && focusCharacter.GetEntityState() == EntityState.DEAD) focusCharacter = null;

        if (focusCharacter == null)
        {
            focusCharacter = gameController.FindNearestCharacter(transform.position, maxVision,currentArea);
            if (focusCharacter != null) agent.SetDestination(focusCharacter.transform.position);
        }
        else
        {
            agent.SetDestination(focusCharacter.transform.position);
            UpdateDirection();
        }
    }

    public override bool TakeDamage(int damage,AttackType attackType)
    {
        if (enemy.TakeDamage(damage,attackType))
        {
            gameController.DeleteEnemy(this,currentArea);
            Destroy(main);
            return true;
        };
        uiEnemy.UpdateHPBar(enemy.GetStatusValue());
        return false;
    }

    public override bool Attack(EntityController player)
    {
        return player.TakeDamage(enemy.GetAttack(),enemy.attackType);
    }

    public void click(PlayerController playerController)
    {
        playerController.focusEnemy = this;
        playerController.isPlayerTarget = true;
    }
}
