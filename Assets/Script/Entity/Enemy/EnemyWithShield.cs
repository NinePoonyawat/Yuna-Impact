using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWithShield : Enemy
{
    public bool isShieldActivate = true;
    public int shieldMaxHp;
    public int shieldCurrentHp;

    public void Start()
    {
        shieldCurrentHp = shieldMaxHp;
    }

    public override bool TakeDamage(int damage,AttackType attackType)
    {
        if (isShieldActivate)
        {
            if (attackType == AttackType.Range) return false;
            else
            {
                shieldCurrentHp -= damage;
                if (shieldCurrentHp <= 0) isShieldActivate = false;
                return false;
            }
        }
        else return base.TakeDamage(damage,attackType);
    }
}
