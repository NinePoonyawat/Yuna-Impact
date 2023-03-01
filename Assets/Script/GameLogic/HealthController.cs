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

    public bool takeDamage(int damage)
    {
        currentHealth -= damage;
        return currentHealth <= 0;
    }

    public void Heal(int heal)
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
