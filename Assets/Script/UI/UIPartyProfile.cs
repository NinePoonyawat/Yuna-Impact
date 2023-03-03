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
    [SerializeField] private Slider characterSPBar;
    [SerializeField] private TMP_Text numkey;
    [SerializeField] private Animator animator;

    public void Init(StatusValueSet statusValue, int idx)
    {
        gameObject.SetActive(true);

        characterName.text = tempName;
        characterIcon.sprite = tempSprite;
        numkey.text = idx.ToString();

        characterHPBar.maxValue = statusValue.getMaxHp();
        characterHPBar.value = statusValue.getHp();
        characterSPBar.maxValue = statusValue.getMaxMp();
        characterSPBar.value = statusValue.getMp();
    }

    public void UpdateHPBar(int current, int max)
    {
        if (KO) return;

        characterHPBar.maxValue = max;
        characterHPBar.value = current;

        if (currentHealth <= 0)
        {
            KO = true;
            animator.SetBool("KnockOut", true);
        }
        
        if (current < currentHealth) animator.SetTrigger("TakeDamage");

        currentHealth = current;
        maxHealth = max;
    }

    public void HideUI()
    {
        gameObject.SetActive(false);
    }

}
