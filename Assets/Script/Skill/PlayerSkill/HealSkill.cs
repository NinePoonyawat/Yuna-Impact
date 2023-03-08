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
    public GameObject skillIndicator;
    private PlayerController target;

    public override void ActivateSkill()
    {
        if (!canHealOther || target == null)
        {
            GetComponent<EntityController>().TakeHeal(healAmount);
            return;
        }

        target.TakeHeal(healAmount);
    }

    public override void PlayerInput()
    {
        if (!canHealOther) return;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        skillIndicator.SetActive(true);

        if (Physics.Raycast(ray, out hit, 999f, mask))
        {
            PlayerController character = hit.transform.GetComponent<PlayerController>();
            if (character != null)
            {
                if (range >= Vector3.Distance(character.transform.position, transform.position))
                {
                    target = character;
                }
                else target = null;
            }
        }
    }
}
