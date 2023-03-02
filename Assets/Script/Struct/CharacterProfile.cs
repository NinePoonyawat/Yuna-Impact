using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public struct CharacterProfile
{
    public string name;
    public Sprite characterIcon;
    public List<SkillProfile> skills;
}