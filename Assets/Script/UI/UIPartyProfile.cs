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

    public void Init(PlayableCharacter character, int idx, bool isTake)
    {
        gameObject.SetActive(true);
        StatusValueSet statusValue = character.GetStatusValue();
        CharacterProfile profile = character.GetProfile();

        characterHPBar.gameObject.SetActive(true);
        characterSPBar.gameObject.SetActive(true);

        characterName.text = profile.name;
        characterIcon.sprite = profile.icon;
        numkey.text = idx.ToString();

        characterHPBar.maxValue = statusValue.getMaxHp();
        characterHPBar.value = statusValue.getHp();
        characterSPBar.maxValue = statusValue.getMaxMp();
        characterSPBar.value = statusValue.getMp();

        animator.SetBool("Control", false);
        
        if (isTake)
        {
            animator.SetBool("Control", true);
            characterHPBar.gameObject.SetActive(false);
            characterSPBar.gameObject.SetActive(false);
        }
    }

    public void UpdateHPBar(int current, int max)
    {
        if (KO) return;

        characterHPBar.maxValue = max;
        characterHPBar.value = current;

        if (current <= 0)
        {
            KO = true;
            animator.SetBool("KnockOut", true);
        }
        
        if (current < currentHealth) animator.SetTrigger("TakeDamage");
        if (current > currentHealth) animator.SetTrigger("TakeHeal");

        currentHealth = current;
        maxHealth = max;
    }

    public void HideUI()
    {
        gameObject.SetActive(false);
    }

}
