using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StunStatus : Status
{

    public override void StatusEnd()
    {
        entityController.SetStun(false);
        base.StatusEnd();
    }

    public void Setting(float duration,EntityController newEntityController)
    {
        statusPeriod = duration;
        statusCount = 0;
        entityController = newEntityController;
        statusController = entityController.statusController;
        StartStatus(duration);
        entityController.SetStun(true);
    }
}
