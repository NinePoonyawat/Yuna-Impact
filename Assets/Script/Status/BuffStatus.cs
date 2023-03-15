using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffStatus : Status
{
    public Entity entity;

    protected int buffAttack;
    protected int buffDefense;
    protected float currentInterval;
    protected float buffInterval;
    protected bool isIntervalBuff;

    void Awake()
    {
        name = "buff";
        isStackable = true;
    }

    protected override void Start()
    {
        base.Start();
        if(entity == null) entity = gameObject.GetComponent<Entity>();
    }

    public void SetUp(float duration,int newBuffAttack,int newBuffDefense,bool isPercentage,Entity newEntity,EntityController newEntityController)
    {
        entityController = newEntityController;
        statusController = entityController.statusController;
        entity = newEntity;
        buffAttack = (isPercentage)? (entity.attack * newBuffAttack) / 100 : newBuffAttack;
        buffDefense = (isPercentage)? (entity.defense * newBuffDefense) / 100 : newBuffDefense;
        entity.buffedAttack += buffAttack;
        entity.buffedDefense += buffDefense;
        isIntervalBuff = false;
        StartStatus(duration);
    }

    public void SetUp(float duration,int newBuffInterval,Entity newEntity,EntityController newEntityController)
    {
        entityController = newEntityController;
        statusController = entityController.statusController;
        entity = newEntity;
        currentInterval = entity.attackPeriod;
        buffInterval = (currentInterval * (100 - newBuffInterval)) / 100;
        entity.attackPeriod = buffInterval;
        isIntervalBuff = true;
        StartStatus(duration);
    }

    public override void StatusEnd()
    {
        if (isIntervalBuff)
        {
            entity.attackPeriod = currentInterval;
        }
        else
        {
            entity.buffedAttack -= buffAttack;
            entity.buffedDefense -= buffDefense;
        }
        base.StatusEnd();
    }
}
