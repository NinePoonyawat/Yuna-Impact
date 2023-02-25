using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Status : MonoBehaviour
{
    protected Entity entity;
    protected string statusName;
    protected float statusPeriod;
    protected float statusCount = 0;

    void Update()
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

    public void SetEntity(Entity newEntity)
    {
        entity = newEntity;
    }
}
