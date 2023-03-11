using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu]
[Serializable]
public class CharacterProfile : ScriptableObject
{
    public string name;
    public Sprite icon;
    [TextArea] public string description;
    public List<SkillProfile> skills;
}