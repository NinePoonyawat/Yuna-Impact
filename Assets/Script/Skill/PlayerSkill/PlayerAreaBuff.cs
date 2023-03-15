using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAreaBuff : PlayerSkill
{
    [Header ("Skill Data")]
    public GameObject rangeIndicator;
    public GameObject buffPrefab;
    public float effectRadius;
    public int buffInterval;
    public float buffDuration;
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
            PlayableCharacter toBuff = collider.GetComponent<PlayableCharacter>();
            if (toBuff != null)
            {
                GameObject GO = Instantiate(buffPrefab,toBuff.transform);
                BuffStatus status = GO.GetComponent<BuffStatus>();
                status.SetUp(buffDuration,buffInterval,toBuff,toBuff.entityController);
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

    public override void PlayerInput()
    {
        rangeIndicator.SetActive(true);
    }
}
