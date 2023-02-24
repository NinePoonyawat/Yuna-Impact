using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManaController : MonoBehaviour,Ivalueable
{
    private Entity entity;

    [SerializeField] private int defaultMaxMana;
    [SerializeField] private int initialMana;
    private int maxMana;
    private int currentMana;

    void Awake()
    {
        entity = gameObject.GetComponent<Entity>();
    }

    void Start()
    {
        maxMana = defaultMaxMana;
        currentMana = initialMana;
    }

    public int getMaxValue()
    {
        return maxMana;
    }

    public int getValue()
    {
        return currentMana;
    }
}