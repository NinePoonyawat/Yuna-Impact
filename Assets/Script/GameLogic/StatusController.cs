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

    public void AddStatus(Status status)
    {
        statuses.Add(status);
    }

    public void RemoveStatus(Status status)
    {
        statuses.Remove(status);
    }
}