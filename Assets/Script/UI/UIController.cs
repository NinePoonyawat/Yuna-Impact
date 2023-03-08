using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    [SerializeField] private UIPartyProfile[] partyProfiles;
    [SerializeField] private UIProfile playerProfile;
    [SerializeField] private UISkillIcon skillProfile;

    [Header ("Color")]
    [SerializeField] private Color[] partyColors;

    void Start()
    {
        partyProfiles = GetComponentsInChildren<UIPartyProfile>();
        playerProfile = GetComponentInChildren<UIProfile>();
        skillProfile = GetComponentInChildren<UISkillIcon>();
    }

    //idx 0 : Taking Character //idx 1-3 : Other Character
    public void UpdateStatusBar(int idx, StatusValueSet statusValue)
    {
        int currentHealth = statusValue.getHp();
        int maxHealth = statusValue.getMaxHp();
        if (idx == 0) playerProfile.UpdateHPBar(currentHealth, maxHealth);
        else partyProfiles[idx-1].UpdateHPBar(currentHealth, maxHealth);
    }

    public void SetProfile(List<PlayerController> characters)
    {
        int idx = 0;
        bool isTake = false;
        for (int i = 0; i < characters.Count; i++)
        {
            if (characters[i].getTaking())
            {
                playerProfile.Init(characters[i].GetCharacter());
                skillProfile.Init();
                isTake = true;
            }
            partyProfiles[idx++].Init(characters[i].GetCharacter(), i+1, characters[i].getTaking());
        }
        if (!isTake)
        {
            playerProfile.HideUI();
            skillProfile.HideUI();
        }
        for (int i = characters.Count; i < 4; i++)
        {
            partyProfiles[i].HideUI();
        }
    }
}
