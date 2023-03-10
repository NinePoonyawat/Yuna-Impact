using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Skill : MonoBehaviour
{
    [Header ("Skill")]
    public SkillProfile profile;
    public float cooldown;

    public float cooldownCount;
    public bool isActivatable = true;
    protected GameController gameController;

    protected LayerMask mask;

    public virtual void Start()
    {
        gameController = FindObjectOfType<GameController>();
        mask = LayerMask.GetMask("Entity");

        cooldown = profile.cooldown;
        cooldownCount = cooldown;
    }

    public virtual void Update()
    {
        if (cooldownCount < cooldown)
        {
            cooldownCount += Time.deltaTime;
            if (cooldownCount >= cooldown)
            {
                isActivatable = true;
            }
        }
    }

    public virtual void SetCooldown()
    {
        isActivatable = false;
        cooldownCount = 0f;
    }

    public abstract void ActivateSkill();
}