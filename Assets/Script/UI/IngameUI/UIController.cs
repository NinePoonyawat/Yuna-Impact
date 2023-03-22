using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    private GameController gameController;
    [SerializeField] private UIPartyProfile[] partyProfiles;
    [SerializeField] private UIProfile playerProfile;

    [Header ("Color")]
    [SerializeField] private Color[] partyColors;

    void Awake()
    {
        gameController = GameObject.Find("GameLogic").GetComponent<GameController>();
        partyProfiles = GetComponentsInChildren<UIPartyProfile>();
        playerProfile = GetComponentInChildren<UIProfile>();
    }

    public void SetProfile(List<PlayerController> characters)
    {
        for(int i = 0; i != 3;i++)
        {
            Debug.Log(characters[i]);
        }
        bool isTake = false;
        for (int i = 0; i < characters.Count; i++)
        {
            if (characters[i].getTaking())
            {
                playerProfile.Init(characters[i]);
                isTake = true;
            }
            Debug.Log(characters[i].playableCharacter);
            partyProfiles[i].Init(characters[i].playableCharacter, i+1, characters[i].getTaking());
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
        if (isTaking) playerProfile.UpdateUI(statusValue);
        partyProfiles[idx].UpdateUI(statusValue);
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
