using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class YunaPierce : PlayerSkill
{
    public SpriteRenderer skillIndicator;
    public Transform floorTransform;
    private Vector3 playerPos = new Vector3(0,100,0);
    public Vector3 areaEffect;

    public override void ActivateSkill()
    {
        //if (Vector3.Distance(playerPos,transform.position) > areaEffect.x) return;
        Vector3 unit = Vector3.Normalize(playerPos - floorTransform.position);
        unit.y = 0;
        Collider[] hitColliders = Physics.OverlapBox(floorTransform.position + (unit * areaEffect.x / 2), areaEffect / 2,Quaternion.LookRotation(floorTransform.position,playerPos),mask);
        foreach (var collider in hitColliders)
        {
            //Debug.Log(collider);
            collider.gameObject.GetComponent<EnemyController>().TakeDamage(40,AttackType.Melee);
        }
        skillIndicator.GetComponent<SpriteRenderer>().enabled = false;
    }

    public override void PlayerInput()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray,out hit))
        {
            playerPos = hit.point;

            skillIndicator.GetComponent<SpriteRenderer>().enabled = true;
            Quaternion transRot = Quaternion.LookRotation(hit.point - transform.position);
            transRot.eulerAngles = new Vector3(90, transRot.eulerAngles.y,transRot.eulerAngles.z);
            skillIndicator.transform.rotation = Quaternion.Lerp(transRot, skillIndicator.transform.rotation, 0f);
        }
    }

    void OnDrawGizmos()
    {
        Vector3 unit = Vector3.Normalize(floorTransform.position - playerPos);
        unit.y = 0;
        Gizmos.matrix = transform.localToWorldMatrix;
        Matrix4x4 matrix = Gizmos.matrix;
        matrix *= Matrix4x4.TRS(floorTransform.position + (unit * areaEffect.x / 2),Quaternion.LookRotation(floorTransform.position,playerPos),Vector3.one);
        Gizmos.matrix = matrix;
        Gizmos.DrawCube(floorTransform.position + (unit * areaEffect.x / 2), areaEffect);
        Gizmos.matrix = Matrix4x4.identity;
    }
}
