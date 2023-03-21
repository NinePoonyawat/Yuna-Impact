using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class EntityController : MonoBehaviour
{
    public EntityState entityState;
    protected bool isSetState = false;
    protected bool isSilent = false;
    public bool isStun = false;

    public StatusController statusController;
    public NavMeshAgent agent;
    [SerializeField] private Transform entityTransform;
    private Vector3 prevPosition;
    [SerializeField] private bool isRight = false;

    [SerializeField] public int currentArea = -1;

    public void UpdateMovingDirection()
    {
        Vector3 newPosition = transform.position;
        float x = newPosition.x - prevPosition.x;
        float z = newPosition.z - prevPosition.z;

        if (isRight && z < -Mathf.Abs(x) - 0.01f)
        {
            isRight = false;
            entityTransform.localScale = new Vector3(entityTransform.localScale.x * -1, entityTransform.localScale.y, entityTransform.localScale.z);
        }
        else if (!isRight && z > Mathf.Abs(x) + 0.01f)
        {
            isRight = true;
            entityTransform.localScale = new Vector3(entityTransform.localScale.x * -1, entityTransform.localScale.y, entityTransform.localScale.z);
        }

        prevPosition = newPosition;
    }

    public void UpdateAttackPosition(Vector3 toAttackPos)
    {
        float x = toAttackPos.x - transform.position.x;
        float z = toAttackPos.z - transform.position.z;

        if (isRight && z < -x - 0.01f)
        {
            isRight = false;
            entityTransform.localScale = new Vector3(entityTransform.localScale.x * -1, entityTransform.localScale.y, entityTransform.localScale.z);
        }
        if (!isRight && z > -x + 0.01f)
        {
            isRight = true;
            entityTransform.localScale = new Vector3(entityTransform.localScale.x * -1, entityTransform.localScale.y, entityTransform.localScale.z);
        }
    }

    public void SetEntityState(EntityState state)
    {
        isSetState = true;
        entityState = state;
        SetStateTrigger();
    }

    public void SetSilent(bool newSilent)
    {
        isSilent = newSilent;
    }

    public virtual void SetStun(bool newStun)
    {
        isStun = newStun;
    }

    public EntityState GetEntityState()
    {
        return entityState;
    }

    public void SetState(EntityState newState)
    {
        entityState = newState;
        isSetState = true;
        SetStateTrigger();
    }

    public abstract bool Attack(EntityController entity);
    public abstract bool TakeDamage(int damage,AttackType attackType);
    public abstract bool TakeHeal(int amount);
    public abstract void AfterAttack();
    public abstract void SetCurrentArea(int newArea);
    public abstract void SetStateTrigger();
}
