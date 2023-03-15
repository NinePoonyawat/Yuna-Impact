using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbsintheAOE : PointRaycastSkill
{
    public int damage;
    public float explodeRange;

    public GameObject stunPrefab;
    public float stunDuration;
    
    public override void PointAt(Transform target)
    {
        Vector3 start = target.position;
        Collider[] colliders = Physics.OverlapCapsule(new Vector3(start.x,start.y + 1,start.z),new Vector3(start.x,start.y - 1,start.z),
            explodeRange,mask);
        foreach (var collider in colliders)
        {
            EnemyController enemyController = collider.GetComponent<EnemyController>();
            if (enemyController != null)
            {
                enemyController.TakeDamage(damage,damageType);
                GameObject GO = GameObject.Instantiate(stunPrefab,enemyController.transform);
                StunStatus stun = GO.GetComponent<StunStatus>();
                stun.Setting(stunDuration,enemyController);
            }
        }
    }

    public override void AIActivate()
    {
        throw new System.NotImplementedException();
    }

    public override int AICalculate()
    {
        throw new System.NotImplementedException();
    }
}
