using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu]
[Serializable]
public class SkillProfile : ScriptableObject
{
    public string name;
    public Sprite icon;
    [TextArea] public string description;
    [Space(10)]
    public float duration;
    public float cooldown;
    public float cost;
    public float damage;
}
