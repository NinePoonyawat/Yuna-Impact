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
}
