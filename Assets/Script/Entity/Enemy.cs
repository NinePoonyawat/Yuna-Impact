using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Entity
{
    [SerializeField] protected float attackRange;

    public override bool isInAttackRange(Vector3 position)
    {
        return (position - transform.position).sqrMagnitude <= attackRange;
    }
}
