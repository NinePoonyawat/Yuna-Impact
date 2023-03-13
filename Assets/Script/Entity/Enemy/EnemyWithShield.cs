using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWithShield : Enemy
{
    public GameObject stunPrefab;
    public GameObject shield;
    public bool isShieldActivate = true;
    public int shieldMaxHp;
    public int shieldCurrentHp;
    public float stunDuration = 3f;

    public void Start()
    {
        shieldCurrentHp = shieldMaxHp;
    }

    public override bool TakeDamage(int damage,AttackType attackType)
    {
        if (isShieldActivate)
        {
            if (attackType == AttackType.Range) return false;
            else if (attackType == AttackType.Hammer)
            {
                ShieldBreak();
                return false;
            }
            else
            {
                shieldCurrentHp -= damage;
                if (shieldCurrentHp <= 0) ShieldBreak();
                Debug.Log("Pass");
                return false;
            }
        }
        else return base.TakeDamage(damage,attackType);
    }

    void ShieldBreak()
    {
        isShieldActivate = false;
        shield.SetActive(false);
        GameObject GO = Instantiate(stunPrefab,transform);
        StunStatus stunStatus = GO.GetComponent<StunStatus>();
        stunStatus.Setting(stunDuration,entityController);
    }
}
