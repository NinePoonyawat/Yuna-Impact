using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbsintheSilence : BashSkill, ISkillProjectile
{
    public GameObject silentPf;
    public float silentDuration;
    public GameObject projectilePf;
    public float projectileSpeed = 15f;

    public override void ActivateSkill()
    {
        SetCooldown();
        if (target != null)
        {
            GameObject GO = Instantiate(projectilePf,transform.position,Quaternion.identity,gameController.projectileRoot);
            Projectile projectile = GO.GetComponent<Projectile>();
            projectile.SetUp(playerController,target,projectileSpeed,this);
            target = null;
        }
        rangeIndicator.SetActive(false);
        targetIndicator.SetActive(false);
    }

    public void Hit(EntityController entityController)
    {
        Debug.Log("enter");
        playerController.Attack(entityController,damage,AttackType.Range);

        GameObject GO = Instantiate(silentPf,entityController.transform);
        Silent silent = GO.GetComponent<Silent>();
        silent.Setting(silentDuration,entityController);
    }
}
