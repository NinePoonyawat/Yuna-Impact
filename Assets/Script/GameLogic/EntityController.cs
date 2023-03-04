using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class EntityController : MonoBehaviour
{
    public EntityState entityState;
    protected bool isSilent = false;

    public NavMeshAgent agent;
    [SerializeField] private Transform entityTransform;
    private Vector3 prevPosition;
    [SerializeField] private bool isRight = false;

    [SerializeField] public int currentArea = -1;

    public void UpdateDirection()
    {
        Vector3 newPosition = transform.position;
        float x = newPosition.x - prevPosition.x;

        if (isRight && x < -0.01f)
        {
            isRight = false;
            entityTransform.localScale = new Vector3(entityTransform.localScale.x * -1, entityTransform.localScale.y, entityTransform.localScale.z);
        }
        if (!isRight && x > 0.01f)
        {
            isRight = true;
            entityTransform.localScale = new Vector3(entityTransform.localScale.x * -1, entityTransform.localScale.y, entityTransform.localScale.z);
        }

        prevPosition = newPosition;
    }

    public void SetEntityState(EntityState state)
    {
        entityState = state;
    }

    public void SetSilent(bool newSilent)
    {
        isSilent = newSilent;
    }

    public EntityState GetEntityState()
    {
        return entityState;
    }

    public abstract bool Attack(EntityController entity);
    public abstract bool TakeDamage(int damage,AttackType attackType);
    public abstract void AfterAttack();
}
