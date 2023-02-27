using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusController : MonoBehaviour
{
    [SerializeField] private GameController gameController;
    [SerializeField] private EntityController entityController;
    [SerializeField] private Entity entity;

    bool AddStatus(Status status)
    {
        gameObject.AddComponent<Status>();
        return true;
    }
}