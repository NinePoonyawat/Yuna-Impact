using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAreaBuff : EnemySkill
{
    [Header ("Skill Data")]
    public GameObject buffPrefab;
    public float effectRadius;
    public int buffDuration;
    public int buffAttack;
    public int buffDefense;
    public bool isPercentage;
    // Start is called before the first frame update
    public override void ActivateSkill()
    {
        Vector3 start = transform.position;
        Collider[] colliders = Physics.OverlapCapsule(new Vector3(start.x,start.y + 1,start.z),new Vector3(start.x,start.y - 1,start.z),
            effectRadius,mask);
        if (colliders.Length != 0) SetCooldown();
        foreach (var collider in colliders)
        {
            Enemy toBuff = collider.GetComponent<Enemy>();
            if (toBuff != null)
            {
                GameObject GO = Instantiate(buffPrefab,toBuff.transform);
                BuffStatus status = GO.GetComponent<BuffStatus>();
                status.SetUp(buffDuration,buffAttack,buffDefense,isPercentage,toBuff,toBuff.entityController);
            }
        }
    }

    public override int AICalculate()
    {
        if (!isActivatable || gameController.areas[enemyController.currentArea].areaCharacter.Count == 0)
            return -1;
        return 10;
    }
}
