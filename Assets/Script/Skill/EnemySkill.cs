using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemySkill : Skill
{
    protected EnemyController enemyController;
    protected Enemy enemy;

    public virtual void Awake()
    {
        enemy = gameObject.GetComponent<Enemy>();
        enemyController = gameObject.GetComponent<EnemyController>();
    }

    public abstract int AICalculate();
}
