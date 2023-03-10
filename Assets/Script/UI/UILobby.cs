using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UILobby : MonoBehaviour
{
    public CharacterProfile[] characterProfiles;
    public GameObject characterPanel;
    public GameObject skillPanel;
    public Button[] skillIcons;

    public int characterIdx = -1;
    public int skillIdx = -1;

    void Awake()
    {
        CloseCharacterPanel();
        CloseSkillPanel();

        skillIcons = characterPanel.GetComponentsInChildren<Button>();
    }

    public void ToggleCharacter(int idx)
    {
        if (characterIdx != idx)
        {
            characterIdx = idx;
            UpdateCharacterPanel(idx);
            ToggleSkill(skillIdx);
        }
        else
        {
            characterIdx = -1;
            CloseCharacterPanel();
            ToggleSkill(skillIdx);
        }
    }

    public void ToggleSkill(int idx)
    {
        if (skillIdx != idx)
        {
            skillIdx = idx;
            UpdateSkillPanel(idx);
        }
        else
        {
            skillIdx = -1;
            CloseSkillPanel();
        }
    }

    void CloseCharacterPanel()
    {
        characterPanel.SetActive(false);
    }

    void UpdateCharacterPanel(int idx)
    {
        characterPanel.SetActive(true);
        TMP_Text[] texts = characterPanel.GetComponentsInChildren<TMP_Text>();
        texts[0].text = characterProfiles[idx].name;
        texts[1].text = characterProfiles[idx].description;
        
        for (int i = 0; i < skillIcons.Length; i++)
        {
            if (i < characterProfiles[idx].skills.Count)
            {
                skillIcons[i].gameObject.SetActive(true);
                skillIcons[i].GetComponent<Image>().sprite = characterProfiles[idx].skills[i].icon;
            }
            else
            {
                skillIcons[i].gameObject.SetActive(false);
            }
        }
    }

    void CloseSkillPanel()
    {
        skillPanel.SetActive(false);
    }

    void UpdateSkillPanel(int idx)
    {
        skillPanel.SetActive(true);
        TMP_Text[] texts = skillPanel.GetComponentsInChildren<TMP_Text>();
        SkillProfile skill = characterProfiles[characterIdx].skills[skillIdx];
        texts[0].text = skill.name;
        texts[1].text = skill.description;
    }
}
