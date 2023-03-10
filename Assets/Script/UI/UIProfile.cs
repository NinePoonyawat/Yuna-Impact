using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIProfile : MonoBehaviour
{
    [Header ("Parameter")]
    [SerializeField] private int maxHealth;
    [SerializeField] private int currentHealth;
    [SerializeField] private List<SkillProfile> skills;
    public string tempName;

    [Header ("Visualize")]
    [SerializeField] private TMP_Text characterName;
    [SerializeField] private Slider characterHPBar;
    [SerializeField] private Slider characterSPBar;
    [SerializeField] private UISkillIcon[] skillIcons;
    [SerializeField] private GameObject skillPanel;

    public void Init(PlayerController playerController)
    {
        gameObject.SetActive(true);

        PlayableCharacter character = playerController.GetCharacter();
        StatusValueSet statusValue = character.GetStatusValue();
        CharacterProfile profile = character.GetProfile();
        PlayerSkill[] characterSkills = playerController.skills;

        characterName.text = profile.name;

        characterHPBar.maxValue = statusValue.getMaxHp();
        characterHPBar.value = statusValue.getHp();
        characterSPBar.maxValue = statusValue.getMaxMp();
        characterSPBar.value = statusValue.getMp();

        skills.Clear();
        for (int idx = 0; idx < skillIcons.Length; idx++)
        {
            if (idx < profile.skills.Count)
            {
                skillIcons[idx].Init(profile.skills[idx], characterSkills[idx].cooldownCount, characterSkills[idx].cooldown);
                skills.Add(profile.skills[idx]);
            }
            else skillIcons[idx].HideUI();
        }
    }

    public void UpdateHPBar(int current, int max)
    {
        characterHPBar.value = current;

        currentHealth = current;
    }

    public void HoldSkill(int idx)
    {
        skillIcons[idx].Hold();
        skillPanel.SetActive(true);
        UpdatePanel(idx);
    }

    public void CastSkill(int idx)
    {
        skillIcons[idx].Cast();
        skillPanel.SetActive(false);
    }

    void UpdatePanel(int idx)
    {
        TMP_Text[] texts = skillPanel.GetComponentsInChildren<TMP_Text>();
        texts[0].text = skills[idx].name;
        texts[1].text = skills[idx].description;
    }

    public void HideUI()
    {
        gameObject.SetActive(false);
    }

}
