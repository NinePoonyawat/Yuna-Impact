using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Status : MonoBehaviour
{
    public string statusName;
    public Sprite sprite;

    protected EntityController entityController;
    public StatusController statusController;
    public bool isStackable;
    protected float statusPeriod = 0f;
    protected float statusCount = -10f;

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
            Debug.Log("called");
            StatusEnd();
        }
    }

    public virtual void StatusEnd()
    {
        statusController.RemoveStatus(this);
        Destroy(gameObject);
    }

    public void StartStatus(float duration)
    {
        statusController.AddStatus(this);
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
