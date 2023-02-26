using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    [SerializeField] private HealthController healthController;
    [SerializeField] private ManaController manaController;

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
