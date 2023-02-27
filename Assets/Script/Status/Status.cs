using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Status : MonoBehaviour
{

    protected EntityController entity;
    protected string statusName;
    protected float statusPeriod = 0f;
    protected float statusCount = -10f;

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

    public void SetEntity(EntityController newEntity)
    {
        entity = newEntity;
    }
}
