using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AoeSkill : PlayerSkill
{
    [Header ("Ability")]
    public int damage = 1;
    public float range = 1.5f;
    public AttackType damageType;
    
    [Header ("Indicator")]
    public GameObject rangeIndicator;

    public override void ActivateSkill()
    {
        SetCooldown();

        List<EnemyController> characters = gameController.FindAllNearestEnemy(transform.position, range);
        foreach(EnemyController target in characters)
        {
            target.TakeDamage(damage, damageType);
        }
        
        if (rangeIndicator != null) rangeIndicator.SetActive(false);
    }

    public override void PlayerInput()
    {
        rangeIndicator.SetActive(true);
    }

    public override void AIActivate()
    {
    }

    public override int AICalculate()
    {
        return 0;
    }
}
