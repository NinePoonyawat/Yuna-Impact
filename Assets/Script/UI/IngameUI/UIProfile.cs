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
    [SerializeField] private TMP_Text characterHPText;
    [SerializeField] private Slider characterSPBar;
    [SerializeField] private TMP_Text characterSPText;
    [SerializeField] private UISkillIcon[] skillIcons;
    [SerializeField] private UISkillDescription skillPanel;
    [SerializeField] private GameObject skillCancel;

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
        characterHPText.text = statusValue.getHp().ToString() + " / " + statusValue.getMaxHp().ToString();
        characterSPBar.maxValue = statusValue.getMaxMp();
        characterSPBar.value = statusValue.getMp();
        characterSPText.text = statusValue.getMp().ToString() + " / " + statusValue.getMaxMp().ToString();

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

    public void UpdateUI(StatusValueSet statusValue)
    {
        int hp = statusValue.getHp();
        int maxHp = statusValue.getMaxHp();
        int mp = statusValue.getMp();
        int maxMp = statusValue.getMaxMp();
        UpdateHPBar(hp, maxHp);
        UpdateSPBar(mp, maxMp);
    }

    void UpdateHPBar(int current, int max)
    {
        characterHPBar.value = current;
        characterHPText.text = current.ToString() + " / " + max.ToString();

        currentHealth = current;
    }
    
    void UpdateSPBar(int current, int max)
    {
        characterSPBar.value = current;
        characterSPText.text = current.ToString() + " / " + max.ToString();
    }

    public void HoldSkill(int idx)
    {
        skillIcons[idx].Hold();
        skillPanel.UpdateUI(skills[idx]);
        skillCancel.SetActive(true);
    }

    public void CastSkill(int idx)
    {
        skillIcons[idx].Cast();
        skillPanel.HideUI();
        skillCancel.SetActive(false);
    }

    public void HideUI()
    {
        gameObject.SetActive(false);
    }

}
