using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Entity : MonoBehaviour
{
    protected GameController gameController;

    [SerializeField] protected HealthController healthController;
    [SerializeField] protected ManaController manaController;

    [SerializeField] protected float attackPeriod = 1.5f;
    protected float attackCount;
    protected bool isAttackable;

    [SerializeField] protected int attack;
    [SerializeField] protected int defense;
    [SerializeField] protected bool isProjectile;
    [SerializeField] protected GameObject projectilePf;
    [SerializeField] protected float projectileSpeed = 5f;

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
            }
        }
    }

    public bool TakeDamage(int damage)
    {
        int rdamage = damage - defense;
        return (healthController.takeDamage(rdamage));
    }

    public abstract bool isInAttackRange(Vector3 position);

    public void AfterAttack()
    {
        isAttackable = false;
        attackCount = 0;
    }

    public bool IsAttackable()
    {
        return isAttackable;
    }

    public void SetAttackable(bool newAttackable)
    {
        isAttackable = newAttackable;
    }

    public StatusValueSet GetStatusValue()
    {
        int hp = healthController.getValue();
        int maxHp = healthController.getMaxValue();
        int mp = manaController.getValue();
        int maxMp = manaController.getMaxValue();
        return new StatusValueSet(hp, maxHp, mp, maxMp);
    }

    public int GetAttack()
    {
        return attack;
    }

    public int GetDefense()
    {
        return defense;
    }

    public bool GetIsProjectile()
    {
        return isProjectile;
    }

    public GameObject GetPrefab()
    {
        return projectilePf;
    }

    public float GetProjectileSpeed()
    {
        return projectileSpeed;
    }
}
