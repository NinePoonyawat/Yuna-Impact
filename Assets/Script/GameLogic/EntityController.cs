using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EntityController : MonoBehaviour
{
    protected EntityState entityState;
    protected bool isSilent = false;

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

    public abstract bool TakeDamage(int damage);
}
