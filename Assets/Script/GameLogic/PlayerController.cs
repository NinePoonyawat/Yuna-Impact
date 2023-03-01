using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : EntityController
{
    private GameController gameController;
    [SerializeField] private PlayableCharacter playableCharacter;

    public Camera cam;

    private Vector3 moveToPos;              //for Gizmos visualize
    private float acceptanceRadius = 0.8f;
    private EnemyController focusEnemy;

    [SerializeField] private  NavMeshAgent agent;

    [SerializeField] private bool isTaking = false; // is this character taking by player
    [SerializeField] private bool isPlayerMoving = false;

    [SerializeField] private UIController uiController;

    public void Awake()
    {
        gameController = GameObject.Find("GameLogic").GetComponent<GameController>();
        uiController = FindObjectOfType<UIController>();
        playableCharacter = GetComponent<PlayableCharacter>();
        cam = FindObjectOfType<Camera>();
    }

    void Start()
    {
        gameController.AddCharacter(this);
        entityState = EntityState.IDLE;
    }

    void Update()
    {
        if (entityState == EntityState.DEAD) return;

        if (isPlayerMoving) UpdatePlayerClickMoving();

        if (isTaking) UpdateTaking();
        else UpdateNotTaking();
    }

    void UpdatePlayerClickMoving()
    {
        if ( (this.transform.position - moveToPos).magnitude <= acceptanceRadius)
        {
            isPlayerMoving = false;
        }
    }

    void UpdateTaking()
    {
        //Update when focus enemy in attack range
        bool isAttack = UpdateAttack();

        // Update when has enemy in vision
        if (!isAttack) UpdatePlayerMoving();
    }

    void UpdateNotTaking()
    {
        bool isAttack = UpdateAttack();

        // Update when has enemy in vision
        if (!isAttack) UpdateAIMoving();
    }

    bool UpdateAttack()
    {
        if (isPlayerMoving) return false;
        if (focusEnemy == null) return false;

        if(playableCharacter.isInAttackRange(focusEnemy.transform.position))
        {
            agent.SetDestination(this.transform.position);
            if (playableCharacter.IsAttackable())
            {
                if (playableCharacter.GetIsProjectile()) RangeAttack();
                else MeleeAttack();
            }
            else
            {
                entityState = EntityState.PREATTACK;
            }
            return !(Input.GetMouseButtonDown(0));
        }
        return false;
    }

    public void MeleeAttack()
    {
        if (Attack(focusEnemy)) focusEnemy = null;
        entityState = EntityState.ATTACK;
        playableCharacter.AfterAttack(); // get attack cooldown;
    }

    public void RangeAttack()
    {
        GameObject GO = Instantiate(playableCharacter.GetPrefab(),gameController.projectileRoot) as GameObject;
        Projectile projectile = GO.GetComponent<Projectile>();
        projectile.SetUp(this,focusEnemy,playableCharacter.GetProjectileSpeed());
        entityState = EntityState.ATTACK;
        playableCharacter.AfterAttack(); // get attack cooldown;
    }

    void UpdateAIMoving()
    {
        if (isPlayerMoving) return; // there is player point destination since this character still control by player

        if (focusEnemy == null)
        {
            focusEnemy =  gameController.FindNearestEnemy(transform.position);
            if(focusEnemy != null) agent.SetDestination(focusEnemy.transform.position);
        }
        else
        {
            agent.SetDestination(focusEnemy.transform.position);
            UpdateDirection();
        }
        entityState = EntityState.MOVE;
        if(focusEnemy != null) moveToPos = focusEnemy.transform.position;
    }

    void UpdatePlayerMoving()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray,out hit))
            {
                agent.SetDestination(hit.point);
                moveToPos = hit.point;
                isPlayerMoving = true;
            }

            entityState = EntityState.MOVE;
            UpdateDirection();
        }
    }

    public override bool Attack(EntityController enemy)
    {
        return enemy.TakeDamage(playableCharacter.GetAttack());
    }

    public bool Targetable(EnemyController enemy)
    {
        return true;
    }

    public override bool TakeDamage(int damage)
    {
        if (playableCharacter.TakeDamage(damage))
        {
            SetEntityState(EntityState.DEAD);
            return true;
        }
        uiController.UpdateStatusBar(gameController.getUIIndex(this), playableCharacter.GetStatusValue());
        return false;
    }

    void OnDrawGizmos()
    {
        if (moveToPos == null) return;
        if (isTaking) Gizmos.color = Color.yellow;
        else Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, moveToPos);
    }

    public void SetTaking(bool newTaking)
    {
        isTaking = newTaking;
    }

    public bool getTaking()
    {
        return isTaking;
    }

    public PlayableCharacter GetCharacter()
    {
        return playableCharacter;
    }
}
