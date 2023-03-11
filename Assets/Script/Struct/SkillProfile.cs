using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum TargetType {Ally = 0, Enemy = 1, Auto = 2}

[CreateAssetMenu]
[Serializable]
public class SkillProfile : ScriptableObject
{
    public string name;
    public Sprite icon;
    [TextArea] public string description;
    [Space(10)]
    public TargetType targetType;
    public float duration;
    public float cooldown;
    public float cost;
    public float damage;
}
