using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Entity : MonoBehaviour
{
    protected GameController gameController;
    public EntityController entityController;

    [SerializeField] protected HealthController healthController;
    [SerializeField] protected ManaController manaController;

    [SerializeField] protected float attackPeriod = 1.5f;
    protected float attackCount;
    public bool isAttackable = false;

    [SerializeField] protected int attack;
    [SerializeField] protected int defense;
    [SerializeField] public AttackType attackType;
    [SerializeField] protected GameObject projectilePf;
    [SerializeField] protected float projectileSpeed = 5f;

    public float defaultSpeed = 3.5f;
    public float walkbackSpeed = 1f;

    public virtual void Awake()
    {
        gameController = GameObject.Find("GameLogic").GetComponent<GameController>();
        entityController = gameObject.GetComponent<EntityController>();
        healthController = GetComponent<HealthController>();
        manaController = GetComponent<ManaController>();
    }

    public virtual void Update()
    {
        if (attackCount < attackPeriod)
        {
            attackCount += Time.deltaTime;
            if (attackCount >= attackPeriod)
            {
                isAttackable = true;
            }
        }
    }

    public int CalculateDamage(int damage)
    {
        return (damage *  100) / (100 + defense);
    }

    public virtual bool CallAttack(EntityController toAttack)
    {
        if (toAttack == null) return true;
        if (attackType == AttackType.Melee)
        {
            if (Attack(toAttack)) return true;
            AfterAttack();
        }
        else
        {
            GameObject GO = Instantiate(GetPrefab(),transform.position,Quaternion.identity,gameController.projectileRoot) as GameObject;
            Projectile projectile = GO.GetComponent<Projectile>();
            projectile.SetUp(entityController,toAttack,GetProjectileSpeed());
            AfterAttack();
        }
        return false;
    }

    public virtual bool CallAttack(EntityController toAttack,int dam)
    {
        if (toAttack == null) return true;
        if (attackType == AttackType.Melee)
        {
            if (Attack(toAttack,dam)) return true;
            AfterAttack();
        }
        else
        {
            GameObject GO = Instantiate(GetPrefab(),transform.position,Quaternion.identity,gameController.projectileRoot) as GameObject;
            Projectile projectile = GO.GetComponent<Projectile>();
            projectile.SetUp(entityController,toAttack,GetProjectileSpeed());
            AfterAttack();
        }
        return false;
    }

    public virtual bool TakeDamage(int damage, AttackType attackType)
    {
        int rdamage = CalculateDamage(damage);
        return (healthController.TakeDamage(rdamage));
    }

    public virtual bool TakeHeal(int amount)
    {
        return (healthController.Heal(amount));
    }

    public abstract bool isInAttackRange(Vector3 position);

    public virtual void AfterAttack()
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

    public virtual bool Attack(EntityController toAttack)
    {
        return toAttack.TakeDamage(attack,attackType);
    }

    public virtual bool Attack(EntityController toAttack,int dam)
    {
        return toAttack.TakeDamage(dam,attackType);
    }

    public int GetAttack()
    {
        return attack;
    }

    public int GetDefense()
    {
        return defense;
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
