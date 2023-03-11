using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireCaster : Enemy
{
    [Header ("Fire Caster")]
    public GameObject fireAreaPf;
    public float explodeDuration;
    public int explodeRadius;

    public override bool CallAttack(EntityController toAttack)
    {
        AfterAttack();
        GameObject GO = Instantiate(fireAreaPf,gameController.projectileRoot);
        GO.transform.position = toAttack.transform.position;
        Debug.Log("enter");
        FireFloorCasting fireFloorCasting = GO.GetComponent<FireFloorCasting>();
        fireFloorCasting.SetUp(explodeDuration,explodeRadius,(EnemyController) entityController,this);
        entityController.SetEntityState(EntityState.PREATTACK);
        return false;
    }
}
