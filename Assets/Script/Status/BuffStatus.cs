using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffStatus : Status
{
    public Entity entity;

    protected int buffAttack;
    protected int buffDefense;

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

    public void SetUp(int duration,int newBuffAttack,int newBuffDefense,bool isPercentage)
    {
        if (entity == null) entity = gameObject.GetComponent<Entity>();
        buffAttack = (isPercentage)? (entity.attack * newBuffAttack) / 100 : newBuffAttack;
        buffDefense = (isPercentage)? (entity.defense * newBuffDefense) / 100 : newBuffDefense;
        entity.buffedAttack += buffAttack;
        entity.buffedDefense += buffDefense;
        StartStatus(duration);
    }

    public override void StatusEnd()
    {
        entity.buffedAttack -= buffAttack;
        entity.buffedDefense -= buffDefense;
        base.StatusEnd();
    }
}
