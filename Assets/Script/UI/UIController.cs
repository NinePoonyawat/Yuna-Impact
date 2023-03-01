using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    public UIPartyProfile[] partyProfiles;
    public UIProfile playerProfile;

    void Start()
    {
        partyProfiles = GetComponentsInChildren<UIPartyProfile>();
        playerProfile = GetComponentInChildren<UIProfile>();
    }

    //idx 0 : Taking Character //idx 1-3 : Other Character
    public void UpdateHPBar(int idx, StatusValueSet statusValue)
    {
        int currentHealth = statusValue.getHp();
        int maxHealth = statusValue.getMaxHp();
        if (idx == 0) playerProfile.UpdateHPBar(currentHealth, maxHealth);
        else partyProfiles[idx-1].UpdateHPBar(currentHealth, maxHealth);
    }

    public void SetProfile(List<PlayerController> characters)
    {
        int idx = 0;
        for (int i = 0; i < characters.Count; i++)
        {
            if (characters[i].getTaking())
            {
                playerProfile.Init(characters[i].GetCharacter().GetStatusValue());
            }
            else 
            {
                partyProfiles[idx++].Init(characters[i].GetCharacter().GetStatusValue());
            }
        }
    }
}
