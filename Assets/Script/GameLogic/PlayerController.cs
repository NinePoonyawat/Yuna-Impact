using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{
    private GameController gameController;
    [SerializeField] private PlayableCharacter playableCharacter;

    public Camera cam;

    public EntityState entityState;
    public Vector3 moveToPos;   //for Gizmos visualize
    private EnemyController focusEnemy;

    [SerializeField] private  NavMeshAgent agent;

    [SerializeField] private bool isTaking = false; // is this character taking by player
    [SerializeField] private bool isPlayerMoving = false;

    public void Awake()
    {
        gameController = GameObject.Find("GameLogic").GetComponent<GameController>();
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
        if (isPlayerMoving) UpdatePlayerClickMoving();

        if (isTaking) UpdateTaking();
        else UpdateNotTaking();
    }

    void UpdatePlayerClickMoving()
    {
        if (this.transform.position == moveToPos)
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
            if (playableCharacter.IsAttackable())
            {
                Attack(focusEnemy);
                entityState = EntityState.ATTACK;
                playableCharacter.Attack();
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
        if (isPlayerMoving) return;

        if (focusEnemy == null)
        {
            focusEnemy =  gameController.FindNearestEnemy(transform.position);
            if(focusEnemy != null) agent.SetDestination(focusEnemy.transform.position);
        }
        else
        {
            agent.SetDestination(focusEnemy.transform.position);
        }
        entityState = EntityState.MOVE;
        moveToPos = focusEnemy.transform.position;
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
        }
    }

    void Attack(EnemyController enemy)
    {
        Debug.Log("attack" + enemy);
    }

    public bool Targetable(EnemyController enemy)
    {
        return true;
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
}
