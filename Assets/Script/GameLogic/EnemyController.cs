using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : EntityController,IPlayerClickable
{
    private GameController gameController;
    public Enemy enemy;
    public GameObject main;
    public EnemySkill[] skills;

    [SerializeField] public PlayerController focusCharacter = null;
    [SerializeField] protected float maxVision = 20f;

    [SerializeField] private Animator animator;

    [SerializeField] private UIEnemyProfile uiEnemy;
    [SerializeField] private SpriteFacingCam spriteFacingCam;

    public void Awake()
    {
        enemy = gameObject.GetComponent<Enemy>();
        gameController = GameObject.Find("GameLogic").GetComponent<GameController>();
        uiEnemy = GetComponentInChildren<UIEnemyProfile>();
        animator = GetComponentInChildren<Animator>();
        spriteFacingCam = GetComponent<SpriteFacingCam>();
        if (main == null) main = transform.parent.gameObject;
    }

    public void Start()
    {
        currentArea = gameObject.GetComponentInParent<AreaController>().areaIdx;
        agent.speed = enemy.defaultSpeed;
        if (currentArea != -1) gameController.AddEnemy(this,currentArea);
        entityState = EntityState.IDLE;
    }

    // Update is called once per frame
    void Update()
    {
        foreach(var skill in skills)
        {
            if (skill.AICalculate() > 0)
            {
                skill.ActivateSkill();
            }
        }

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
                break;
            case EntityState.ATTACK :
                //if (focusCharacter != null && enemy.CallAttack(focusCharacter)) focusCharacter = null;
                if (animator == null)
                {
                    enemy.CallAttack(focusCharacter);
                }
                agent.speed = 0f;
                break;
            case EntityState.PREATTACK :
                if (focusCharacter != null) agent.SetDestination((transform.position - focusCharacter.transform.position) + transform.position);
                agent.speed = enemy.walkbackSpeed;
                break;
        }
    }

    public void GetNextState()
    {
        if (isSetState)
        {
            isSetState = false;
            return;
        }

        switch (entityState)
        {
            case EntityState.IDLE :
                focusCharacter = gameController.FindBestCharacter(transform.position,maxVision,currentArea);
                if (focusCharacter != null)
                {
                    if(animator != null) animator.SetBool("isWalk", true);
                    entityState = EntityState.MOVE;
                }
                break;
            case EntityState.MOVE :
                agent.speed = enemy.defaultSpeed;
                if (!focusCharacter.IsBlockable())
                {
                    if (enemy.attackType != AttackType.Range) focusCharacter.blockedEnemy.Remove(this);
                    focusCharacter = null;
                }
                if (focusCharacter == null)
                {
                    entityState = EntityState.IDLE;
                    if(animator != null) animator.SetBool("isWalk", false);
                }
                else if (enemy.isInAttackRange(focusCharacter.transform.position) && enemy.isAttackable)
                {
                    entityState = EntityState.ATTACK;
                    if (enemy.attackType != AttackType.Range) focusCharacter.blockedEnemy.Add(this);
                    if(animator != null) animator.SetTrigger("LAttack");
                }
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
                        if (enemy.isInAttackRange(focusCharacter.transform.position))
                        {
                            entityState = EntityState.ATTACK;
                            if(animator != null) animator.SetTrigger("LAttack");
                        }
                        else
                        {
                            agent.speed = enemy.walkbackSpeed;
                            entityState = EntityState.MOVE;
                        }
                    }
                }
                break;
        }
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
            if (focusCharacter != null && enemy.attackType != AttackType.Range)
            {
                foreach (var member in focusCharacter.blockedEnemy)
                {
                    if (member == this)
                    {
                        focusCharacter.blockedEnemy.Remove(this);
                        break;
                    }
                }
                gameController.DeleteEnemy(this,currentArea);
                Destroy(main);
                return true;
            }
        }
        uiEnemy.UpdateHPBar(enemy.GetStatusValue());
        spriteFacingCam.TakeDamage();
        return false;
    }

    public override bool TakeHeal(int amount)
    {
        if (!enemy.TakeHeal(amount))
        {
            return false;
        }

        uiEnemy.UpdateHPBar(enemy.GetStatusValue());
        return true;
    }

    public override bool Attack(EntityController player)
    {
        enemy.AfterAttack();
        return player.TakeDamage(enemy.GetAttack(),enemy.attackType);
    }

    public bool AttackFocus()
    {
        enemy.AfterAttack();
        if (enemy.isInAttackRange(focusCharacter.transform.position)) return focusCharacter.TakeDamage(enemy.GetAttack(),enemy.attackType);
        else return false;
    }

    public override void SetCurrentArea(int newArea)
    {
        gameController.areas[currentArea].RemoveEnemy(this);
        currentArea = newArea;
        gameController.areas[currentArea].AddEnemy(this);
    }

    public void click(PlayerController playerController)
    {
        playerController.focusEnemy = this;
        playerController.isPlayerTarget = true;
    }

    public UIEnemyProfile GetProfile()
    {
        return uiEnemy;
    }
}
