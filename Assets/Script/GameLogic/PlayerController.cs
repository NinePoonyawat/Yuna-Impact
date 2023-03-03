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
    public EnemyController focusEnemy;
    [SerializeField] private Animator animator;

    [SerializeField] private bool isTaking = false; // is this character taking by player
    [SerializeField] public bool isPlayerMoving = false;
    private TeleportController focusTeleport;

    [SerializeField] private UIController uiController;

    public void Awake()
    {
        gameController = GameObject.Find("GameLogic").GetComponent<GameController>();
        uiController = FindObjectOfType<UIController>();
        playableCharacter = GetComponent<PlayableCharacter>();
        cam = FindObjectOfType<Camera>();
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();
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
            if (animator != null) animator.SetBool("isWalk",false);
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
        if (focusEnemy == null)
        {
            focusEnemy = gameController.FindNearestEnemy(transform.position,currentArea);
            if (focusEnemy == null) return false;
        }

        if(playableCharacter.isInAttackRange(focusEnemy.transform.position))
        {
            agent.SetDestination(this.transform.position);
            if (playableCharacter.IsAttackable())
            {
                playableCharacter.CallAttack(focusEnemy);
                if (animator != null) animator.SetBool("isWalk",false);
            }
            else
            {
                entityState = EntityState.PREATTACK;
            }
            return !(Input.GetMouseButtonDown(0));
        }
        return false;
    }

    void UpdateAIMoving()
    {
        if (isPlayerMoving) return; // there is player point destination since this character still control by player

        if (focusEnemy == null)
        {
            focusEnemy =  gameController.FindNearestEnemy(transform.position,currentArea);
            if(focusEnemy != null) agent.SetDestination(focusEnemy.transform.position);
            else if (focusTeleport == null)
            {
                TeleportController teleport = gameController.FindTeleport(transform.position,currentArea);
                focusTeleport = teleport;
                if (teleport != null)
                {
                    agent.SetDestination(focusTeleport.transform.position);
                    if (animator != null) animator.SetBool("isWalk",true);
                }
            }
            else if (focusTeleport != null)
            {
                agent.SetDestination(focusTeleport.transform.position);
                if (animator != null) animator.SetBool("isWalk",true);
            }
        }
        else
        {
            agent.SetDestination(focusEnemy.transform.position);
        }
        entityState = EntityState.MOVE;
        UpdateDirection();
        if(focusEnemy != null)
        {
            if (animator != null) animator.SetBool("isWalk",true);
            moveToPos = focusEnemy.transform.position;
        }
        else if (focusTeleport != null)
        {
            if (animator != null) animator.SetBool("isWalk",true);
            if (focusTeleport.Teleport(this)) focusTeleport = null;
        }
        else
        {
            if (animator != null) animator.SetBool("isWalk",false);
        }
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
            if (animator != null) animator.SetBool("isWalk",true);
        }
        UpdateDirection();
    }

    public override bool Attack(EntityController enemy)
    {
        return enemy.TakeDamage(playableCharacter.GetAttack(),playableCharacter.attackType);
    }

    public bool Targetable(EnemyController enemy)
    {
        return true;
    }

    public override bool TakeDamage(int damage,AttackType attackType)
    {
        if (playableCharacter.TakeDamage(damage,attackType))
        {
            SetEntityState(EntityState.DEAD);
            if (animator != null) animator.SetBool("isKO",true);
            return true;
        }
        uiController.UpdateStatusBar(gameController.getUIIndex(this), playableCharacter.GetStatusValue());
        return false;
    }

    public override void AfterAttack()
    {
        return;
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
