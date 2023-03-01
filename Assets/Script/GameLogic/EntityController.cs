using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EntityController : MonoBehaviour
{
    protected EntityState entityState;
    protected bool isSilent = false;

    [SerializeField] private Transform entityTransform;
    private Vector3 prevPosition;
    [SerializeField] private bool isRight = false;

    public void UpdateDirection()
    {
        Vector3 newPosition = transform.position;
        float x = newPosition.x - prevPosition.x;

        if (isRight && x < 0)
        {
            isRight = false;
            entityTransform.localScale = new Vector3(entityTransform.localScale.x * -1, entityTransform.localScale.y, entityTransform.localScale.z);
        }
        if (!isRight && x > 0)
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
    public abstract bool TakeDamage(int damage);
}
