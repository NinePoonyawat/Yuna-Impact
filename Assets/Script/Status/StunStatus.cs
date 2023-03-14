using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StunStatus : Status
{

    public override void StatusEnd()
    {
        entityController.isStun = false;
        base.StatusEnd();
    }

    public void Setting(float duration,EntityController entityController)
    {
        statusPeriod = duration;
        statusCount = 0;
        SetEntity(entityController);
        entityController.isStun = true;
    }
}
