using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class UIInfo : MonoBehaviour
{
    public string[] nextScene;

    public int page;

    [Header ("Character Info")]
    public GameObject characterPanel;
    public CharacterProfile[] characterProfiles;
    public Button[] skillIcons;
    public UISkillDescription skillPanel;
    public Animator characterAnim;

    [Header ("Button")]
    public GameObject[] stageButtons;
    public GameObject backButton;
    public GameObject nextButton;
    public GameObject startButton;
    public TMP_Text countText;

    int characterIdx = -1;
    int skillIdx = -1;
    int stageIdx;

    [Header ("Enemy Info")]
    public Animator enemyPanel;

    [Header ("Map Info")]
    public Animator mapPanel;

    void Awake()
    {
        skillIcons = characterPanel.GetComponentsInChildren<Button>();

        CloseCharacterPanel();
        startButton.SetActive(false);
        backButton.SetActive(false);
        nextButton.SetActive(false);
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

    public void ChooseStage(int idx)
    {
        stageIdx = idx;
        SetPage(true);
    }

    void SetPage(bool next)
    {
        if (page == 0)
        {
            foreach (GameObject button in stageButtons)
            {
                button.SetActive(false);
            }
            characterAnim.SetTrigger("Appear");
        }
        if (page == 1)
        {
            
        }
        if (page == 2)
        {
            mapPanel.SetTrigger("Hide");
        }
        if (page == 3)
        {
            enemyPanel.SetTrigger("Hide");
        }

        if (!next && page > 0) page--;
        if (next) page++;

        if (page == 0)
        {
            foreach (GameObject button in stageButtons)
            {
                button.SetActive(true);
            }
            nextButton.SetActive(false);
            backButton.SetActive(false);
            characterAnim.SetTrigger("Hide");
            CloseCharacterPanel();
        }
        if (page == 1)
        {
            nextButton.SetActive(true);
        }
        if (page == 2)
        {
            backButton.SetActive(true);
            mapPanel.SetTrigger("Appear");
            startButton.SetActive(false);
        }
        if (page == 3)
        {
            enemyPanel.SetTrigger("Appear");
            startButton.SetActive(true);
        }
        if (page == 4)
        {
            StartStage();
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

    public void StartStage()
    {
        StartCoroutine(SwitchScene());
    }

    IEnumerator SwitchScene()
    {
        for (int time = 3; time >= 0; time--)
        {
            yield return new WaitForSeconds(1f);
            countText.text = time.ToString();
        }
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene(nextScene[stageIdx]);
    }
}
