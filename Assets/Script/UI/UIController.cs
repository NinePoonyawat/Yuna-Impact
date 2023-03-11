using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    [SerializeField] private UIPartyProfile[] partyProfiles;
    [SerializeField] private UIProfile playerProfile;

    [Header ("Color")]
    [SerializeField] private Color[] partyColors;

    void Awake()
    {
        partyProfiles = GetComponentsInChildren<UIPartyProfile>();
        playerProfile = GetComponentInChildren<UIProfile>();
    }

    public void SetProfile(List<PlayerController> characters)
    {
        bool isTake = false;
        for (int i = 0; i < characters.Count; i++)
        {
            if (characters[i].getTaking())
            {
                playerProfile.Init(characters[i]);
                isTake = true;
            }
            partyProfiles[i].Init(characters[i].GetCharacter(), i+1, characters[i].getTaking());
        }
        if (!isTake)
        {
            playerProfile.HideUI();
        }
        for (int i = characters.Count; i < 4; i++)
        {
            partyProfiles[i].HideUI();
        }
    }

    public void UpdateStatusBar(int idx, bool isTaking, StatusValueSet statusValue)
    {
        int currentHealth = statusValue.getHp();
        int maxHealth = statusValue.getMaxHp();
        if (isTaking) playerProfile.UpdateHPBar(currentHealth, maxHealth);
        partyProfiles[idx].UpdateHPBar(currentHealth, maxHealth);
    }

    public void HoldSkill(int idx)
    {
        playerProfile.HoldSkill(idx);
    }

    public void CastSkill(int idx)
    {
        playerProfile.CastSkill(idx);
    }
}
