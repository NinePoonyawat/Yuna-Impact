using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Silent : Status
{

    // Start is called before the first frame update
    void Awake()
    {
        name = "silent";
        isStackable = false;
    }

    public override void StatusEnd()
    {
        entity.SetSilent(false);
        base.StatusEnd();
    }

    void Setting(int duration,EntityController entityController)
    {
        statusPeriod = duration;
        statusCount = 0;
        SetEntity(entityController);
        entity.SetSilent(true);
    }
}
