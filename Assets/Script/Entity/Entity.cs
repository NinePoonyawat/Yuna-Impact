using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    protected GameController gameController;

    [SerializeField] private HealthController healthController;
    [SerializeField] private ManaController manaController;

    public virtual void Awake()
    {
        gameController = GameObject.Find("GameLogic").GetComponent<GameController>();
    }

    public void TakeDamage(int damage)
    {
        if (healthController.takeDamage(damage))
        {
            Die();
        }
    }

    public void Die()
    {

    }
}
