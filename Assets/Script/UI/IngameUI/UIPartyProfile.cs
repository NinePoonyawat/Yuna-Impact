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

    [Header ("Visualize")]
    [SerializeField] private TMP_Text characterName;
    [SerializeField] private Image characterIcon;
    [SerializeField] private Slider characterHPBar;
    [SerializeField] private Slider characterSPBar;
    [SerializeField] private TMP_Text numkey;
    [SerializeField] private Image stateIcon;
    [SerializeField] private Animator animator;

    [Space (20)]
    [SerializeField] private Sprite[] stateIconList;

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
            //characterHPBar.gameObject.SetActive(false);
            //characterSPBar.gameObject.SetActive(false);
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

        EntityState state = statusValue.getState();
        if (state == EntityState.IDLE)
        {
            stateIcon.color = new Color(255, 255, 255, 1);
            stateIcon.sprite = stateIconList[0];
        }
        else if (state == EntityState.MOVE || state == EntityState.MOVETOTELEPORT)
        {
            stateIcon.color = new Color(255, 255, 255, 1);
            stateIcon.sprite = stateIconList[1];
        }
        else if (state == EntityState.ATTACK || state == EntityState.PREATTACK)
        {
            stateIcon.color = new Color(255, 255, 255, 1);
            stateIcon.sprite = stateIconList[2];
        }
        else stateIcon.color = new Color(255, 255, 255, 0);
    }

    void UpdateHPBar(int current, int max)
    {
        if (KO) return;

        characterHPBar.maxValue = max;
        characterHPBar.value = current;

        if (current <= 0)
        {
            KO = true;
            animator.SetBool("KnockOut", true);
        }
        
        if (current < currentHealth || currentHealth == 0) animator.SetTrigger("TakeDamage");
        if (current > currentHealth && currentHealth != 0) animator.SetTrigger("TakeHeal");

        currentHealth = current;
        maxHealth = max;
    }

    void UpdateSPBar(int current, int max)
    {
        characterSPBar.maxValue = max;
        characterSPBar.value = current;
    }

    public void HideUI()
    {
        gameObject.SetActive(false);
    }

}
