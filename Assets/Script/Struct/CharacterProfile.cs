using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public struct CharacterProfile
{
    public string name;
    public Sprite icon;
    [TextArea] public string description;
    public List<SkillProfile> skills;

    public void SetSkill(PlayerSkill[] characterSkills)
    {
        for (int idx = 0; idx < characterSkills.Length; idx++)
        {
            skills.Add(characterSkills[idx].profile);
        }
    }
}