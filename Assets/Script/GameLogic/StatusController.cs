using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusController : MonoBehaviour
{
    [SerializeField] private GameController gameController;
    [SerializeField] private EntityController entityController;
    [SerializeField] private Entity entity;
    private List<Status> statuses = new List<Status>();

    void Start()
    {
        gameController = GameObject.Find("GameLogic").GetComponent<GameController>();
        entityController = gameObject.GetComponent<EntityController>();
        entity = gameObject.GetComponent<Entity>();
    }

    public Status AddStatus(Status status)
    {
        if (!status.isStackable)
        {
            foreach (var member in statuses)
            {
                if (status == member)
                {
                    Destroy(status);
                    return member;
                }
            }
        }
        statuses.Add(status);
        UpdateUI();

        return status;
    }

    public void RemoveStatus(Status status)
    {
        statuses.Remove(status);

        UpdateUI();
    }

    void UpdateUI()
    {
        if (entityController is EnemyController)
        {
            EnemyController enemy = (EnemyController) entityController;
            enemy.GetProfile().UpdateStatus(statuses);
        }
        if (entityController is PlayerController)
        {
            PlayerController player = (PlayerController) entityController;
        }
    }
}