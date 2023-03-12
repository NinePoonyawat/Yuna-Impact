using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UISkillDescription : MonoBehaviour
{
    [SerializeField] private TMP_Text name;
    [SerializeField] private TMP_Text description;
    [SerializeField] private TMP_Text cooldown;
    [SerializeField] private TMP_Text cost;
    [SerializeField] private TMP_Text targetType;
    [SerializeField] private Image targetBox;
    [SerializeField] private Color[] targetColors;
    [SerializeField] private string[] targetTexts;

    void Awake()
    {
        HideUI();
    }

    public void UpdateUI(SkillProfile skill)
    {
        gameObject.SetActive(true);

        name.text = skill.name;
        description.text = skill.description;
        cooldown.text = skill.cooldown.ToString() + " s";
        cost.text = skill.cost.ToString() + " mp";

        targetType.text = targetTexts[(int)skill.targetType];
        targetBox.color = targetColors[(int)skill.targetType];
    }

    public void HideUI()
    {
        gameObject.SetActive(false);
    }
}
