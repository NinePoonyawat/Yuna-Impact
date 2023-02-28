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
    public float tempCooldown;

    [Header ("Visualize")]
    [SerializeField] private TMP_Text characterName;
    [SerializeField] private Slider characterHPBar;
    [SerializeField] private Slider characterSPBar;
    [SerializeField] private UISkillIcon[] skillIcons;

    void Update()
    {
        //Debug
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            CastSkill(0);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            CastSkill(1);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            CastSkill(2);
        }
    }

    public void Init()
    {
        characterName.text = tempName;

        characterHPBar.maxValue = maxHealth;
        characterHPBar.value = currentHealth;

        foreach (UISkillIcon skillIcon in skillIcons)
        {
            skillIcon.Init();
        }
    }

    public void CastSkill(int idx)
    {
        skillIcons[idx].Cast(tempCooldown);
    }

    public void UpdateHPBar(int current, int max)
    {
        characterHPBar.value = current;

        currentHealth = current;
    }

}
