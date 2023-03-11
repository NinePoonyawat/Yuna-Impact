using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Status : MonoBehaviour
{

    protected EntityController entityController;
    public StatusController statusController;
    public bool isStackable;
    public string statusName;
    protected float statusPeriod = 0f;
    protected float statusCount = -10f;
    public Sprite sprite;

    protected virtual void Start()
    {
        entityController = gameObject.GetComponentInParent<EntityController>();
        statusController = entityController.statusController;
    }

    protected virtual void Update()
    {
        statusCount += Time.deltaTime;
        if (statusCount >= statusPeriod)
        {
            StatusEnd();
        }
    }

    public virtual void StatusEnd()
    {
        statusController.RemoveStatus(this);
        Destroy(this);
    }

    public void StartStatus(float duration)
    {
        statusPeriod = duration;
        statusCount = 0;
    }

    public void SetEntity(EntityController newEntity)
    {
        entityController = newEntity;
    }

    public static bool operator ==(Status l,Status r)
    {
        return l.statusName == r.statusName;
    }

    public static bool operator !=(Status l,Status r)
    {
        return l.statusName != r.statusName;
    }
}
