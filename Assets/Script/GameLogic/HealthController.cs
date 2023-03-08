using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthController : MonoBehaviour,Ivalueable
{
    private Entity entity;

    [SerializeField] private int defaultHealth;
    private int maxHealth;
    public int currentHealth;

    void Awake()
    {
        entity = gameObject.GetComponent<Entity>();
    }

    void Start()
    {
        maxHealth = defaultHealth;
        currentHealth = defaultHealth;
    }

    public bool TakeDamage(int damage)
    {
        currentHealth -= damage;
        return currentHealth <= 0;
    }

    public bool Heal(int heal)
    {
        if (currentHealth >= maxHealth) return false;

        currentHealth += heal;
        if (currentHealth > maxHealth) currentHealth = maxHealth;
        return true;
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
