using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWithShield : Enemy
{
    public bool isShieldActivate;
    public int shieldMaxHp;
    public int shieldCurrentHp;

    public override bool TakeDamage(int damage,AttackType attackType)
    {
        if (isShieldActivate && attackType == AttackType.Range) return false;
        else return base.TakeDamage(damage,attackType);
    }
}
