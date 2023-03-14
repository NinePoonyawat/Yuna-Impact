using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Entity
{

    [SerializeField] protected float attackCooldownPeriod = 1.5f;
    protected float attackCooldownCount;
    public bool isAttackCooldown;

    public override void Update()
    {
        base.Update();
        // if (attackCooldownCount <= attackCooldownPeriod)
        // {
        //     attackCooldownCount += Time.deltaTime;
        //     if (attackCooldownCount >= attackCooldownPeriod)
        //     {
        //         isAttackCooldown = false;
        //         isAttackable = true;
        //         entityController.AfterAttack();
        //     }
        // }
    }

    public override bool CallAttack(EntityController toAttack)
    {
        isAttackCooldown = true;
        return base.CallAttack(toAttack);
    }

    public override void AfterAttack()
    {
        base.AfterAttack();
        isAttackCooldown = true;
        attackCooldownCount = 0;
    }

    public override bool isInAttackRange(Vector3 position)
    {
        return Vector3.Distance(position,transform.position) <= attackRange;
    }
}
