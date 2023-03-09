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
    public string tempName;

    [Header ("Visualize")]
    [SerializeField] private TMP_Text characterName;
    [SerializeField] private Slider characterHPBar;
    [SerializeField] private Slider characterSPBar;
    [SerializeField] private UISkillIcon[] skillIcons;

    public void Init(PlayableCharacter character)
    {
        gameObject.SetActive(true);
        StatusValueSet statusValue = character.GetStatusValue();
        CharacterProfile profile = character.GetProfile();

        characterName.text = profile.name;

        characterHPBar.maxValue = statusValue.getMaxHp();
        characterHPBar.value = statusValue.getHp();
        characterSPBar.maxValue = statusValue.getMaxMp();
        characterSPBar.value = statusValue.getMp();

        for (int idx = 0; idx < skillIcons.Length; idx++)
        {
            if (idx < profile.skills.Count) skillIcons[idx].Init(profile.skills[idx]);
            else skillIcons[idx].HideUI();
        }
    }

    public void CastSkill(int idx)
    {
        skillIcons[idx].Cast();
    }

    public void UpdateHPBar(int current, int max)
    {
        characterHPBar.value = current;

        currentHealth = current;
    }

    public void HideUI()
    {
        gameObject.SetActive(false);
    }

}
