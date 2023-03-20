using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PointRaycastSkill : PlayerSkill
{
    [Header ("Ability")]
    public AttackType damageType;
    public float range = 10f;
    public GameObject effectPrefab;
    
    [Header ("Indicator")]
    public GameObject rangeIndicator;
    public GameObject targetIndicator;
    public Vector3 target;

    public override void ActivateSkill()
    {
        SetCooldown();
        if (target != null)
        {
            PointAt(target);
            if (effectPrefab != null) Instantiate(effectPrefab, target, Quaternion.identity);
        }
        //if (effectPrefab != null) Instantiate(effectPrefab, target.transform.position, Quaternion.identity);
        
        rangeIndicator.SetActive(false);
        targetIndicator.SetActive(false);
    }

    public override void PlayerInput()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        rangeIndicator.SetActive(true);
        targetIndicator.SetActive(true);

        if (Physics.Raycast(ray, out hit))
        {
            targetIndicator.transform.position = hit.point;
            if (range >= Vector3.Distance(hit.transform.position, transform.position))
            {
                target = hit.point;
                targetIndicator.transform.position = hit.point;
            }
            else
            {
                targetIndicator.transform.position = transform.position;
            }
        }
    }

    public abstract void PointAt(Vector3 target);
}
