using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Entity : MonoBehaviour
{
    protected GameController gameController;

    [SerializeField] private HealthController healthController;
    [SerializeField] private ManaController manaController;

    [SerializeField] protected float attackPeriod = 1.5f;
    protected float attackCount;
    protected bool isAttackable;

    public virtual void Awake()
    {
        gameController = GameObject.Find("GameLogic").GetComponent<GameController>();
    }

    public virtual void Update()
    {
        if (attackCount <= attackPeriod)
        {
            attackCount += Time.deltaTime;
            if (attackCount >= attackPeriod)
            {
                isAttackable = true;
                attackCount = 0;
            }
        }
    }

    public void TakeDamage(int damage)
    {
        if (healthController.takeDamage(damage))
        {
            Die();
        }
    }

    public abstract bool isInAttackRange(Vector3 position);

    public void Die()
    {

    }

    public void Attack()
    {
        isAttackable = false;
    }

    public bool IsAttackable()
    {
        return isAttackable;
    }

    public void SetAttackable(bool newAttackable)
    {
        isAttackable = newAttackable;
    }
}
