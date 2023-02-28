using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIPartyProfile : MonoBehaviour
{
    [Header ("Parameter")]
    [SerializeField] private int maxHealth;
    [SerializeField] private int currentHealth;
    [SerializeField] private bool KO;
    public Sprite tempSprite;
    public string tempName;

    [Header ("Visualize")]
    [SerializeField] private TMP_Text characterName;
    [SerializeField] private Image characterIcon;
    [SerializeField] private Slider characterHPBar;
    [SerializeField] private Animator animator;

    void Update()
    {

    }

    public void Init()
    {
        characterName.text = tempName;
        characterIcon.sprite = tempSprite;

        characterHPBar.maxValue = maxHealth;
        characterHPBar.value = currentHealth;
    }

    public void UpdateHPBar(int current, int max)
    {
        if (KO) return;

        characterHPBar.value = current;
        characterHPBar.maxValue = max;

        if (currentHealth <= 0)
        {
            KO = true;
            animator.SetBool("KnockOut", true);
        }
        
        if (current < currentHealth) animator.SetTrigger("TakeDamage");

        currentHealth = current;
        maxHealth = max;
    }

}
