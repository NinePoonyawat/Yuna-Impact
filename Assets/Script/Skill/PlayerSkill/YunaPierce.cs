using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class YunaPierce : PlayerSkill
{
    public Image skillIndicator;
    private Vector3 playerPos = new Vector3(0,100,0);
    public Vector3 areaEffect;

    public override void ActivateSkill()
    {
        //if (Vector3.Distance(playerPos,transform.position) > areaEffect.x) return;
        Vector3 unit = Vector3.Normalize(playerPos - transform.position);
        Collider[] hitColliders = Physics.OverlapBox(transform.position + (unit * areaEffect.x / 2), areaEffect / 2,Quaternion.LookRotation(transform.position,playerPos),mask);
        Debug.Log(hitColliders.Length);
    }

    public override void PlayerInput()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray,out hit))
        {
            playerPos = hit.point;
            //skillIndicator.GetComponent<Image>().enabled = true;
        }
    }
}
