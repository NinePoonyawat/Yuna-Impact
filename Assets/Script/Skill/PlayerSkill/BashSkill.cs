using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BashSkill : PlayerSkill
{
    [Header ("Ability")]
    public int damage = 1;
    public float range = 1.5f;
    public AttackType damageType;
    
    [Header ("Indicator")]
    public GameObject rangeIndicator;
    public GameObject targetIndicator;
    private EnemyController target;

    public override void ActivateSkill()
    {
        if (target != null)
        {
            target.TakeDamage(damage, damageType);
            target = null;
        }
        rangeIndicator.SetActive(false);
        targetIndicator.SetActive(false);
    }

    public override void PlayerInput()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        rangeIndicator.SetActive(true);
        targetIndicator.SetActive(true);

        if (Physics.Raycast(ray, out hit, 999f, mask))
        {
            EnemyController enemy = hit.transform.GetComponent<EnemyController>();
            if (enemy != null)
            {
                if (range >= Vector3.Distance(enemy.transform.position, transform.position))
                {
                    target = enemy;
                }
                else target = null;
            }
        }
    }
}
