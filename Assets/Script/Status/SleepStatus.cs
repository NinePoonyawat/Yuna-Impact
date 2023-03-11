using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SleepStatus : Status
{
    private float duration = 0;
    private float durationCount = -10f;
    
    void Awake()
    {
        name = "sleep";
        isStackable = false;
    }

    public override void StatusEnd()
    {
        entityController.SetEntityState(EntityState.IDLE);
        base.StatusEnd();
    }

    void Setting(float newDuration,EntityController entityController)
    {
        duration = newDuration;
        durationCount = 0;
        SetEntity(entityController);
        entityController.SetEntityState(EntityState.SLEEP);
    }
}
