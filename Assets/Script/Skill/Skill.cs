using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Skill : MonoBehaviour
{
    [Header ("Skill")]
    public SkillProfile profile;
    public float skillDuration;
    public float initialDuration;

    protected float skillDurationCount;
    private bool isActivateable = false;
    protected GameController gameController;

    protected LayerMask mask;

    public virtual void Start()
    {
        gameController = FindObjectOfType<GameController>();
        mask = LayerMask.GetMask("Entity");
    }

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