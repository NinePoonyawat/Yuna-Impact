using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthController : MonoBehaviour,Ivalueable
{
    private Entity entity;

    [SerializeField] private int defaultHealth;
    private int maxHealth;
    private int currentHealth;

    void Awake()
    {
        entity = gameObject.GetComponent<Entity>();
    }

    void Start()
    {
        maxHealth = defaultHealth;
        currentHealth = defaultHealth;
    }

    void takeDamage(int damage)
    {
        currentHealth -= damage;
    }

    void Heal(int heal)
    {
        currentHealth += heal;
        if (currentHealth > maxHealth) currentHealth = maxHealth;
    }

    public int getMaxValue()
    {
        return maxHealth;
    }

    public int getValue()
    {
        return currentHealth;
    }
}
