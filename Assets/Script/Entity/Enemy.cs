using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Entity
{
    [SerializeField] protected float attackRange;

    public override bool isInAttackRange(Vector3 position)
    {
        return Vector3.Distance(position,transform.position) <= attackRange;
    }
}
