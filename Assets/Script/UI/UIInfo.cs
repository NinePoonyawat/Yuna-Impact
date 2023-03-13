using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class UIInfo : MonoBehaviour
{
    public string nextScene;

    public int page;

    [Header ("Character Info")]
    public GameObject characterPanel;
    public CharacterProfile[] characterProfiles;
    public Button[] skillIcons;
    public UISkillDescription skillPanel;
    public Animator characterAnim;

    int characterIdx = -1;
    int skillIdx = -1;

    [Header ("Enemy Info")]
    public Animator enemyPanel;

    [Header ("Map Info")]
    public Animator mapPanel;

    void Awake()
    {
        CloseCharacterPanel();

        skillIcons = characterPanel.GetComponentsInChildren<Button>();
    }

    public void ToggleCharacter(int idx)
    {
        if (characterIdx != idx)
        {
            characterIdx = idx;
            UpdateCharacterPanel(idx);
            ToggleSkill(skillIdx);
        }
        else
        {
            characterIdx = -1;
            CloseCharacterPanel();
            ToggleSkill(skillIdx);
        }
    }

    public void ToggleSkill(int idx)
    {
        if (skillIdx != idx)
        {
            skillIdx = idx;
            skillPanel.UpdateUI(characterProfiles[characterIdx].skills[skillIdx]);
        }
        else
        {
            skillIdx = -1;
            skillPanel.HideUI();
        }
    }

    public void Next()
    {
        SetPage(true);
    }

    public void Back()
    {
        SetPage(false);
    }

    void SetPage(bool next)
    {
        if (page == 1)
        {
            mapPanel.SetTrigger("Hide");
        }
        if (page == 2)
        {
            enemyPanel.SetTrigger("Hide");
        }
        if (page == 3)
        {
            characterAnim.SetTrigger("Hide");
            CloseCharacterPanel();
        }

        if (!next && page > 0) page--;
        if (next) page++;

        if (page == 1)
        {
            mapPanel.SetTrigger("Appear");
        }
        if (page == 2)
        {
            enemyPanel.SetTrigger("Appear");
        }
        if (page == 3)
        {
            characterAnim.SetTrigger("Appear");
        }
        if (page == 4)
        {
            SceneManager.LoadScene(nextScene);
        }
    }

    void CloseCharacterPanel()
    {
        characterPanel.SetActive(false);
        skillPanel.HideUI();
    }

    void UpdateCharacterPanel(int idx)
    {
        characterPanel.SetActive(true);
        TMP_Text[] texts = characterPanel.GetComponentsInChildren<TMP_Text>();
        texts[0].text = characterProfiles[idx].name;
        texts[1].text = characterProfiles[idx].description;
        
        for (int i = 0; i < skillIcons.Length; i++)
        {
            if (i < characterProfiles[idx].skills.Count)
            {
                skillIcons[i].gameObject.SetActive(true);
                skillIcons[i].GetComponent<Image>().sprite = characterProfiles[idx].skills[i].icon;
            }
            else
            {
                skillIcons[i].gameObject.SetActive(false);
            }
        }
    }
}
