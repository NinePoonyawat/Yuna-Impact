using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Status : MonoBehaviour
{

    protected EntityController entity;
    public bool isStackable;
    public string statusName;
    protected float statusPeriod = 0f;
    protected float statusCount = -10f;

    protected virtual void Start()
    {
        entity = gameObject.GetComponent<EntityController>();
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
        Destroy(this);
    }

    public void StartStatus(float duration)
    {
        statusPeriod = duration;
        statusCount = 0;
    }

    public void SetEntity(EntityController newEntity)
    {
        entity = newEntity;
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
