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
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Init();
        }
        if (Input.GetKeyDown(KeyCode.G))
        {
            TakeDamage(1);
        }
    }

    public void Init()
    {
        characterName.text = tempName;
        characterIcon.sprite = tempSprite;

        characterHPBar.maxValue = maxHealth;
        characterHPBar.value = currentHealth;
    }

    public void TakeDamage(int damage)
    {
        if (KO) return;

        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            KO = true;
            animator.SetBool("KnockOut", true);
        }
        
        characterHPBar.value = currentHealth;
        animator.SetTrigger("TakeDamage");
    }
}
