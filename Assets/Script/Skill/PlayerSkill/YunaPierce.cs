using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class YunaPierce : PlayerSkill
{
    public GameObject skillIndicator;
    public Transform floorTransform;
    private Vector3 playerPos = new Vector3(0,100,0);
    public Vector3 areaEffect;

    public override void ActivateSkill()
    {
        SetCooldown();
        //if (Vector3.Distance(playerPos,transform.position) > areaEffect.x) return;
        Vector3 d = playerPos - floorTransform.position;
        d.y = 0;
        Vector3 unit = Vector3.Normalize(d);
        Collider[] hitColliders = Physics.OverlapBox(floorTransform.position + (unit * areaEffect.x / 2), areaEffect / 2,Quaternion.LookRotation(floorTransform.position,playerPos),mask);
        foreach (var collider in hitColliders)
        {
            //Debug.Log(collider);
            EnemyController target = collider.gameObject.GetComponent<EnemyController>();
            if (target != null) target.TakeDamage(40,AttackType.Melee);
        }
        skillIndicator.SetActive(false);
    }

    public override void PlayerInput()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        skillIndicator.SetActive(true);
        
        if (Physics.Raycast(ray,out hit))
        {
            playerPos = hit.point;

            Quaternion transRot = Quaternion.LookRotation(hit.point - transform.position);
            transRot.eulerAngles = new Vector3(90, transRot.eulerAngles.y,transRot.eulerAngles.z);
            skillIndicator.transform.rotation = Quaternion.Lerp(transRot, skillIndicator.transform.rotation, 0f);
        }
    }

    public override void AIActivate()
    {
        playerPos = player.focusEnemy.transform.position;
        ActivateSkill();
    }

    public override int AICalculate()
    {
        if (Vector3.Distance(player.focusEnemy.transform.position,transform.position) <= areaEffect.x / 2) return 10;
        return -1;
    }
}
