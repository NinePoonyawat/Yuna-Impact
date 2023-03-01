using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayableCharacter : Entity
{
    // Start is called before the first frame update
    [SerializeField] private float attackRange;
    
    public bool Targetable(Enemy enemy)
    {
        return true;
    }

    public override bool isInAttackRange(Vector3 position)
    {
        return Vector3.Distance(position,transform.position) <= attackRange;
    }

    public int getHp()
    {
        return healthController.getValue();
    }

    public int getMaxHp()
    {
        return healthController.getMaxValue();
    }
}
