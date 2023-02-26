using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{
    private GameController gameController;
    [SerializeField] private PlayableCharacter playableCharacter;

    public Camera cam;

    public Vector3 moveToPos;
    private EnemyController focusEnemy;

    [SerializeField] private  NavMeshAgent agent;

    private bool isTaking = false; // is this character taking by player

    public void Awake()
    {
        gameController = GameObject.Find("GameLogic").GetComponent<GameController>();
    }

    void Start()
    {
        gameController.AddCharacter(this);
    }

    void Update()
    {
        if (isTaking) UpdateTaking();
        else UpdateNotTaking();
    }

    void UpdateTaking()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray,out hit))
            {
                agent.SetDestination(hit.point);
                moveToPos = hit.point;  
            }
        }
    }

    void UpdateNotTaking()
    {
        if (focusEnemy == null)
        {
            focusEnemy =  gameController.FindNearestEnemy(transform.position);
            agent.SetDestination(focusEnemy.transform.position);
        }
        else
        {
            agent.SetDestination(focusEnemy.transform.position);
        }
    }

    public bool Targetable(EnemyController enemy)
    {
        return true;
    }

    void OnDrawGizmos()
    {
        if (moveToPos == null) return;
        Gizmos.color = Color.yellow;
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
