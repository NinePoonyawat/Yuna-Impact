using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContinuousDamageStatus : Status
{
    int damage;
    float damagePeriod;
    float damageCount = 0;

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        damageCount += Time.deltaTime;
        if (damageCount >= damagePeriod)
        {
            entity.TakeDamage(damage,AttackType.Melee);
            damageCount -= damagePeriod;
        }
    }

    public void SetDamage(int newDamage)
    {
        damage = newDamage;
    }

    public void SetDamagePeriod(int newDamagePeriod)
    {
        damagePeriod = newDamagePeriod;
    }

    public void SetUp(int newDamage,float newDamagePeriod,float duration)
    {
        StartStatus(duration);
        damage = newDamage;
        damagePeriod = newDamagePeriod;
    }
}
