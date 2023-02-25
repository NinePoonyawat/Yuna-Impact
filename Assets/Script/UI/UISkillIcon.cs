using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UISkillIcon : MonoBehaviour
{
    [Header ("Parameter")]
    [SerializeField] private float cooldownTimer;
    [SerializeField] private float cooldownTime;
    public Sprite tempSprite;

    [Header ("Visualize")]
    [SerializeField] private Image skillIcon;
    [SerializeField] private Image CooldownIcon;
    [SerializeField] private Animator animator;
    [SerializeField] private TMP_Text text;

    void Update()
    {
        if (cooldownTimer > 0)
        {
            cooldownTimer -= Time.deltaTime;
            text.text = cooldownTimer.ToString("#.##");
            CooldownIcon.fillAmount = cooldownTimer / cooldownTime;
        }
    }

    public void Init()
    {
        skillIcon.sprite = tempSprite;
        CooldownIcon.fillAmount = 0f;
    }

    public void Cast(float time)
    {
        cooldownTimer = time;
        cooldownTime = time;
    }
}
