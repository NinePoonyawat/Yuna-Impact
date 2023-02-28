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

    [SerializeField] protected int attack;
    [SerializeField] protected int defense;

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

    public bool TakeDamage(int damage)
    {
        int rdamage = damage - defense;
        return (healthController.takeDamage(rdamage));
    }

    public abstract bool isInAttackRange(Vector3 position);

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

    public int GetAttack()
    {
        return attack;
    }

    public int GetDefense()
    {
        return defense;
    }
}
