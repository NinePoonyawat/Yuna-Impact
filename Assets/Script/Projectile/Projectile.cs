using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    protected EntityController caller;
    protected EntityController destination;
    protected float speed;

    protected Vector3 entityPosition;

    public void SetUp(EntityController newCaller,EntityController newDestination,float newSpeed)
    {
        caller = newCaller;
        destination = newDestination;
        speed = newSpeed;
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
            caller.Attack(destination);
            Destroy(gameObject);
        }
    }
}