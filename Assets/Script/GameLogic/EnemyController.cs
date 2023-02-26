using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    private GameController gameController;
    [SerializeField] private Enemy enemy;

    protected PlayerController focusCharacter = null;
    [SerializeField] protected float maxVision = 20f;

    [SerializeField] private NavMeshAgent navMeshAgent;
    public void Awake()
    {
        gameController = GameObject.Find("GameLogic").GetComponent<GameController>();
    }

    public void Start()
    {
        gameController.AddEnemy(this);
    }

    // Update is called once per frame
    void Update()
    {
        UpdateEnemy();
    }

    void UpdateEnemy()
    {
        if (focusCharacter == null)
        {
            focusCharacter = gameController.FindNearestCharacter(transform.position,maxVision);
            if (focusCharacter != null) navMeshAgent.SetDestination(focusCharacter.transform.position);
        }
        else
        {
            navMeshAgent.SetDestination(focusCharacter.transform.position);
        }
    }
}
