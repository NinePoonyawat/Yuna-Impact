using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct StatusValueSet
{
    public int hp;
    public int maxHp;

    public int mp;
    public int maxMp;

    public EntityState state;

    public StatusValueSet(int hp, int maxHp, int mp, int maxMp, EntityState state)
    {
        this.hp = hp;
        this.maxHp = maxHp;
        this.mp = mp;
        this.maxMp = maxMp;
        this.state = state;
    }

    public int getHp() { return hp; }

    public int getMaxHp() { return maxHp; }

    public int getMp() { return mp; }

    public int getMaxMp() { return maxMp; }

    public EntityState getState() { return state; }
}
