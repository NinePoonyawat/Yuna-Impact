using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Skill : MonoBehaviour
{
    public string skillName;
    public float skillDuration;
    public bool isActivateable = false;
    public float initialDuration;
    protected float skillDurationCount;

    public virtual void Update()
    {
        if (skillDurationCount < skillDuration)
        {
            skillDurationCount += Time.deltaTime;
            if (skillDurationCount >= skillDuration)
            {
                isActivateable = true;
            }
        }
    }

    public abstract void ActivateSkill();
}