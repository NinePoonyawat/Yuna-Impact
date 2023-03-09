using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealSkill : PlayerSkill
{
    [Header ("Ability")]
    public bool canHealOther;
    public int healAmount = 1;
    public float range = 1.5f;

    [Header ("Indicator")]
    public GameObject rangeIndicator;
    public GameObject targetIndicator;
    public PlayerController target;

    public override void ActivateSkill()
    {
        if (!canHealOther || target == null)
        {
            GetComponent<EntityController>().TakeHeal(healAmount);
        }
        else
        {
            target.TakeHeal(healAmount);
        }

        rangeIndicator.SetActive(false);
        targetIndicator.SetActive(false);
    }

    public override void PlayerInput()
    {
        if (!canHealOther) return;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        rangeIndicator.SetActive(true);
        targetIndicator.SetActive(true);

        if (Physics.Raycast(ray, out hit, 999f, mask))
        {
            PlayerController character = hit.transform.GetComponent<PlayerController>();
            if (character != null)
            {
                if (range >= Vector3.Distance(character.transform.position, transform.position))
                {
                    target = character;
                    targetIndicator.transform.position = target.transform.position;
                }
                else
                {
                    target = null;
                    targetIndicator.transform.position = transform.position;
                }
            }
            else
            {
                targetIndicator.transform.position = transform.position;
            }
        }
    }

    public override void AIActivate()
    {
    }

    public override int AICalculate()
    {
        return 0;
    }
}
