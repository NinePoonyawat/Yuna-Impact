using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIEnemyProfile : MonoBehaviour
{
    [Header ("Parameter")]
    [SerializeField] private int maxHealth;
    [SerializeField] private int currentHealth;

    [Header ("Visualize")]
    [SerializeField] private Slider characterHPBar;
    [SerializeField] private Image[] statusIcons;

    void Awake()
    {
        characterHPBar = GetComponentInChildren<Slider>();
    }

    public void UpdateHPBar(StatusValueSet statusValue)
    {
        int current = statusValue.getHp();
        int max = statusValue.getMaxHp();

        characterHPBar.maxValue = max;
        characterHPBar.value = current;

        currentHealth = current;
        maxHealth = max;
    }

    public void UpdateStatus(List<Status> statuses)
    {
        for (int i = 0; i < statusIcons.Length; i++)
        {
            if (i < statuses.Count)
            {
                statusIcons[i].sprite = statuses[i].sprite;
                statusIcons[i].enabled = true;
            }
            else statusIcons[i].enabled = false;
        }
    }
}
