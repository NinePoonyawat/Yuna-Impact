using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AoeSkill : PlayerSkill
{
    [Header ("Ability")]
    public int damage = 1;
    public float range = 1.5f;
    public AttackType damageType;

    public GameObject statusPf;
    public float float1;
    
    [Header ("Indicator")]
    public GameObject rangeIndicator;

    public override void ActivateSkill()
    {
        SetCooldown();

        Vector3 start = transform.position;
        Collider[] colliders = Physics.OverlapCapsule(new Vector3(start.x,start.y + 1,start.z),new Vector3(start.x,start.y - 1,start.z),
            range,mask);
        foreach(var collider in colliders)
        {
            EnemyController enemyController = collider.GetComponent<EnemyController>();
            if (enemyController != null)
            {
                enemyController.TakeDamage(damage, damageType);
                if (statusPf != null)
                {
                    GameObject GO = Instantiate(statusPf,enemyController.transform);
                    Status status = GO.GetComponent<Status>();
                    switch (status.GetType().Name)
                    {
                        case ("StunStatus") :
                            ((StunStatus) status).Setting(float1,enemyController);
                            break;
                    }
                }
            }
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

    public override void CancelSkill()
    {
        rangeIndicator.SetActive(false);
    }
}
