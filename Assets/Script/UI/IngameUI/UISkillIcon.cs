using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UISkillIcon : MonoBehaviour
{
    [Header ("Variable")]
    [SerializeField] private float cooldownTimer;
    [SerializeField] private float cooldownTime;
    public Sprite tempSprite;

    [Header ("Visualize")]
    [SerializeField] private Image skillIcon;
    [SerializeField] private Image CooldownIcon;
    [SerializeField] private Image HoldingIcon;
    [SerializeField] private Animator animator;
    [SerializeField] private TMP_Text text;

    void Update()
    {
        if (cooldownTimer < cooldownTime)
        {
            cooldownTimer += Time.deltaTime;
            UpdateTimer();
        }
    }

    public void Init(SkillProfile skill, float current, float max)
    {
        gameObject.SetActive(true);
        skillIcon.sprite = skill.icon;
        cooldownTimer = current;
        cooldownTime = max;
        UpdateTimer();
    }

    void UpdateTimer()
    {
        float cooldownLeft = cooldownTime - cooldownTimer;
        if (cooldownLeft > 0) text.text = cooldownLeft.ToString("#");
        else text.text = "";
        CooldownIcon.fillAmount = cooldownLeft / cooldownTime;
    }

    public void Hold()
    {
        HoldingIcon.enabled = true;
    }

    public void Cast()
    {
        HoldingIcon.enabled = false;
        cooldownTimer = 0;
    }

    public void HideUI()
    {
        gameObject.SetActive(false);
    }
}
