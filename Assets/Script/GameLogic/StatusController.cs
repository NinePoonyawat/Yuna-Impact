using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusController : MonoBehaviour
{
    [SerializeField] private GameController gameController;
    [SerializeField] private EntityController entityController;
    [SerializeField] private Entity entity;
    private List<Status> statuses = new List<Status>();

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
        return status;
    }
}