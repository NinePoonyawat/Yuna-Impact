using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    protected EntityController caller;
    protected EntityController destination;
    protected float speed;
    public GameObject effectPrefab;

    protected Vector3 entityPosition;
    protected bool isSkillCalling = false;
    public ISkillProjectile skill;

    public void SetUp(EntityController newCaller,EntityController newDestination,float newSpeed)
    {
        caller = newCaller;
        destination = newDestination;
        speed = newSpeed;
    }

    public void SetUp(EntityController newCaller,EntityController newDestination,float newSpeed,ISkillProjectile newSkillCalling)
    {
        caller = newCaller;
        destination = newDestination;
        speed = newSpeed;
        isSkillCalling = true;
        skill = newSkillCalling;
    }

    // Update is called once per frame
    void Update()
    {
        if (destination == null)
        {
            Destroy(gameObject);
            return;
        }
        entityPosition = destination.transform.position;
        transform.position = Vector3.MoveTowards(transform.position,entityPosition,speed * Time.deltaTime);
        if (Mathf.Abs(Vector3.Distance(entityPosition,transform.position)) <= 0.001f)
        {
            if(isSkillCalling)
            {
                skill.Hit(destination);
                Destroy(gameObject);
            }
            else
            {
                caller.Attack(destination);
                Hit();
            }
        }
    }

    void Hit()
    {
        if (effectPrefab != null) Instantiate(effectPrefab, transform.position, transform.rotation);
        Destroy(gameObject);
    }
}
