using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : EntityController
{
    private GameController gameController;
    [SerializeField] public PlayableCharacter playableCharacter;

    public PlayerSkill[] skills;

    public Camera cam;

    private Vector3 moveToPos;              //for Gizmos visualize
    private float acceptanceRadius = 0.8f;
    private float AIvision = 5f;
    public EnemyController focusEnemy;
    public List<EnemyController> blockedEnemy;
    public TeleportController focusTeleport;
    [SerializeField] private Animator animator;

    [SerializeField] private UIController uiController;

    [SerializeField] private bool isTaking = false; // is this character taking by player
    [SerializeField] public bool isPlayerMoving = false;
    [SerializeField] public bool isPlayerTarget = false;
    private int usingSkill = -1;

    private LayerMask layerClickMask;

    public void Awake()
    {
        gameController = GameObject.Find("GameLogic").GetComponent<GameController>();
        uiController = FindObjectOfType<UIController>();
        playableCharacter = GetComponent<PlayableCharacter>();
        cam = FindObjectOfType<Camera>();
        agent = GetComponentInParent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();
        skills = GetComponents<PlayerSkill>();

        layerClickMask = LayerMask.GetMask("Entity","PlayerClickable");
    }

    void Start()
    {
        gameController.AddCharacter(this);
        gameController.areas[currentArea].AddCharacter(this);
        entityState = EntityState.PLAYERSTART;
        if(animator != null) animator.Play("Base-Idle");

        agent.speed = playableCharacter.defaultSpeed;
    }

    void Update()
    {
        if (entityState == EntityState.DEAD) return;
        if (isStun) return;

        if (isPlayerMoving) UpdatePlayerClickMoving();
        if (isPlayerTarget)
        {
            if (focusEnemy == null) isPlayerTarget = false;
        }

        // if (isTaking) UpdateTaking();
        // else UpdateNotTaking();

        if (isTaking)
        {
            UpdatePlayerClick();

            //skill1
            if (skills.Length > 0 && skills[0].isActivatable)
            {
                if (Input.GetKey(KeyCode.Q))
                {
                    UsingSkill(0, true);
                }
                if (Input.GetKeyUp(KeyCode.Q))
                {
                    UsingSkill(0, false);
                }
            }

            if (skills.Length > 1 && skills[1].isActivatable)
            {
                //skill2
                if (Input.GetKey(KeyCode.W))
                {
                    UsingSkill(1, true);
                }
                if (Input.GetKeyUp(KeyCode.W))
                {
                    UsingSkill(1, false);
                }
            }
        }

        UpdateState();
        GetNextState();
    }

    void UsingSkill(int idx, bool keyDown)
    {
        if (usingSkill != idx && usingSkill != -1) return;
        if (skills.Length > idx)
        {
            if (keyDown)
            {
                if (!playableCharacter.isManaEnough(skills[idx].profile.cost)) return;
                Time.timeScale = 0.3f;
                skills[idx].PlayerInput();
                usingSkill = idx;
                uiController.HoldSkill(idx);
            }
            else
            {
                if (!playableCharacter.isManaEnough(skills[idx].profile.cost)) return;
                skills[idx].ActivateSkill();
                uiController.CastSkill(idx);
                playableCharacter.SpendMana(skills[idx].profile.cost);
                UpdateUI();
                usingSkill = -1;
                Time.timeScale = 1f;
                if (idx == 0) animator.SetTrigger("Skill1");
                if (idx == 1) animator.SetTrigger("Skill2");
            }
        }
    }

    void UpdateState()
    {
        switch (entityState)
        {
            case EntityState.MOVE :
                if(focusEnemy != null && !isPlayerMoving) agent.SetDestination(focusEnemy.transform.position);
                UpdateMovingDirection();
                break;
            // case EntityState.ATTACK :
            //     if (playableCharacter.CallAttack(focusEnemy)) focusEnemy = null;
            //     break;
            case EntityState.MOVETOTELEPORT :
                if (focusTeleport != null)
                {
                    agent.SetDestination(focusTeleport.transform.position);
                    if (focusTeleport.Teleport(this))
                    {
                        focusTeleport = null;
                    }
                    else UpdateMovingDirection();
                }
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
            case EntityState.PLAYERSTART :
                if (focusEnemy != null)
                {
                    if (!playableCharacter.isInAttackRange(focusEnemy.transform.position))
                    {
                        //agent.SetDestination(focusEnemy.transform.position);
                        if(animator != null) animator.SetBool("isWalk",true);
                        entityState = EntityState.MOVE;
                    }
                    else
                    {
                        if(animator != null) animator.SetTrigger("Attack1");
                        entityState = EntityState.ATTACK;
                        UpdateAttackPosition(focusEnemy.transform.position);
                    }
                }
                break;
            case EntityState.IDLE :
                if (!isPlayerTarget && !isTaking) focusEnemy = gameController.FindNearestEnemy(this.transform.position,currentArea,AIvision);
                if (focusEnemy != null)
                {
                    if(isTaking) gameController.SetNewTarget(focusEnemy);
                    if(!playableCharacter.isInAttackRange(focusEnemy.transform.position))
                    {
                        if(animator != null) animator.SetBool("isWalk",true);
                        entityState = EntityState.MOVE;
                    }
                    else
                    {
                        if(animator != null) animator.SetTrigger("Attack1");
                        entityState = EntityState.ATTACK;
                        UpdateAttackPosition(focusEnemy.transform.position);
                    }
                }
                // else
                // {
                //     if (isTaking)
                //     {
                //         focusEnemy = gameController.FindNearestEnemy(this.transform.position,currentArea);
                //         if (focusEnemy != null) gameController.SetNewTarget(focusEnemy);
                //     }
                //     else
                //     {
                //         if(animator != null) animator.SetBool("isWalk",true);
                //         if(gameController.areas[currentArea].areaEnemies.Count == 0) focusTeleport = gameController.FindTeleport(transform.position,currentArea);
                //         entityState = EntityState.MOVETOTELEPORT;
                //     }
                // }
                break;
            case EntityState.MOVE :
                if (focusEnemy == null)
                {
                    if (!isPlayerMoving)
                    {
                        if(animator != null) animator.SetBool("isWalk",false);
                        entityState = EntityState.IDLE;
                    }
                }
                else if (playableCharacter.isInAttackRange(focusEnemy.transform.position) && playableCharacter.isAttackable && !isPlayerMoving)
                {
                    if(animator != null) animator.SetBool("isWalk",false);
                    agent.SetDestination(this.transform.position);
                    if(animator != null) animator.SetTrigger("Attack1");
                    entityState = EntityState.ATTACK;
                    UpdateAttackPosition(focusEnemy.transform.position);
                }
                break;
            // case EntityState.ATTACK :
            //     entityState = EntityState.PREATTACK;
            //     break;
            case EntityState.PREATTACK :
                if (focusEnemy == null)
                {
                    if(animator != null) animator.SetBool("isWalk",false);
                    entityState = EntityState.IDLE;
                }
                else
                {
                    if (playableCharacter.isAttackable)
                    {
                        if(animator != null) animator.SetBool("isWalk",true);
                        if (playableCharacter.isInAttackRange(focusEnemy.transform.position))
                        {
                            if(animator != null) animator.SetTrigger("Attack1");
                            entityState = EntityState.ATTACK;
                            UpdateAttackPosition(focusEnemy.transform.position);
                        }
                        else entityState = EntityState.MOVE;
                    }
                }
                break;
            case EntityState.MOVETOTELEPORT :
                if (focusTeleport == null)
                {
                    if(animator != null) animator.SetBool("isWalk",false);
                    entityState = EntityState.IDLE;
                }
                break;                
        }
    }
                    

    void UpdatePlayerClick()
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
                if(animator != null) animator.SetBool("isWalk",true);
                SetEntityState(EntityState.MOVE);
            }
        }
        if (Input.GetMouseButtonDown(1))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray,out hit,999f,layerClickMask))
            {
                IPlayerClickable playerClick = hit.transform.GetComponent<IPlayerClickable>();
                if (playerClick != null)
                {
                    playerClick.click(this);
                }
                
                EnemyController enim = hit.transform.GetComponent<EnemyController>();
                if (enim == null)
                {
                    enim = gameController.FindNearestEnemy(hit.transform.position,currentArea,0.3f);
                }
                if (enim != null)
                {
                    gameController.SetNewTarget(enim);
                    focusEnemy = enim;
                    isPlayerTarget = true;
                    if (isPlayerMoving) isPlayerMoving = false;
                }
            }
        }
    }

    void UpdatePlayerClickMoving()
    {
        if ( (this.transform.position - moveToPos).magnitude <= acceptanceRadius)
        {
            isPlayerMoving = false;
            if (animator != null) animator.SetBool("isWalk",false);
        }
    }

    public override void SetCurrentArea(int newArea)
    {
        gameController.areas[currentArea].RemoveCharacter(this);
        currentArea = newArea;
        gameController.areas[currentArea].AddCharacter(this);
    }

    public bool IsBlockable()
    {
        return blockedEnemy.Count < playableCharacter.blockCount;
    }

    public override bool Attack(EntityController enemy)
    {
        return enemy.TakeDamage(playableCharacter.GetAttack(),playableCharacter.attackType);
    }

    public bool Attack(EntityController enemy,int damage,AttackType attackType)
    {
        return enemy.TakeDamage(damage,attackType);
    }

    public void AttackFocus()
    {
        if (focusEnemy == null) return;
        focusEnemy.TakeDamage(playableCharacter.GetAttack(),playableCharacter.attackType);
    }

    public void CallAttackFocus()
    {
        if (focusEnemy == null) return;
        playableCharacter.CallAttack(focusEnemy);
    }

    public bool Targetable(EnemyController enemy)
    {
        return true;
    }

    public override bool TakeDamage(int damage,AttackType attackType)
    {
        if (playableCharacter.TakeDamage(damage,attackType))
        {
            UpdateUI();
            SetEntityState(EntityState.DEAD);
            if (animator != null) animator.SetTrigger("Stun");
            return true;
        }
        
        UpdateUI();
        return false;
    }

    public override bool TakeHeal(int amount)
    {
        if (!playableCharacter.TakeHeal(amount))
        {
            return false;
        }
        UpdateUI();
        
        return true;
    }

    public void UpdateUI()
    {
        uiController.UpdateStatusBar(gameController.getUIIndex(this), isTaking, playableCharacter.GetStatusValue());
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
