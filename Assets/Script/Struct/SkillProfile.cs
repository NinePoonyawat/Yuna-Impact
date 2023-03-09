using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public struct SkillProfile
{
    public string name;
    public Sprite icon;
    [TextArea] public string description;
    public float cooldown;
    public float cost;
}
