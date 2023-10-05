using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class EnemyBehavior : MonoBehaviour
{
    [Header("Max")] 
    [SerializeField] protected int maxLife = 100;
    [SerializeField] protected int maxShield = 30;
    [FormerlySerializedAs("OnLifeShieldCooldown")]
    [Header("Shield Regenation")] 
    [SerializeField] private float OnLifeCooldown = 4f;
    [SerializeField] private float OnShieldCooldown = 1f;
    [SerializeField] private float regenCooldown = .2f;
    [SerializeField] private int regenFactor = 2;
    [Space] 
    [SerializeField] private EnemyLifeBar enemyLifeBar;
    [Space]
    [Header("Current Health/Shield")] //TODO remove SerializeField
    [SerializeField] protected int life;
    [SerializeField] protected int shield;

    private bool isShield;

    private Coroutine ShieldRegenCoroutine;

    protected virtual void Start()
    {
        life = maxLife;
        shield = maxShield;
        isShield = shield > 0;
        UpdateStats();
    }

    public virtual void Hit(int damage = 1, bool overflow = false)
    {
        if (isShield)
        {
            if (overflow)
            {
                int overflowDamage = damage - shield;
                if (overflowDamage > 0) life -= overflowDamage;
                if(life <= 0) Destroy();
            }
            shield -= damage;
            if (shield <= 0) BreakShield();
        }
        else
        {
            life -= damage;
            if (life <= 0)
            {
                PoolOfObject.instance.SpawnFromPool(PoolOfObject.Type.EnemyExplosion, transform.position,
                    Quaternion.identity);
                Destroy();
            }
            
        }
        ResetShieldRegen();
        UpdateStats();
    }

    void ResetShieldRegen()
    {
        if (ShieldRegenCoroutine != null) StopCoroutine(ShieldRegenCoroutine);
        ShieldRegenCoroutine = StartCoroutine(ShieldCoolDown());
    }

    void BreakShield()
    {
        isShield = false;
    }

    protected virtual void Destroy()
    {
        Destroy(gameObject);
    }

    public virtual bool IsPlayer()
    {
        return false;
    }

    protected virtual void UpdateStats()
    {
        if (enemyLifeBar) enemyLifeBar.UpdateStats(life, shield, maxLife);
    }



    IEnumerator ShieldCoolDown()
    {
        if (!isShield) yield return new WaitForSeconds(OnLifeCooldown);
        else yield return new WaitForSeconds(OnShieldCooldown);
        isShield = true;
        shield+=regenFactor;
        UpdateStats();
        ShieldRegenCoroutine = StartCoroutine(ShieldRegen());
    }

    IEnumerator ShieldRegen()
    {
        yield return new WaitForSeconds(regenCooldown);
        shield+= regenFactor;
        UpdateStats();
        if (shield < maxShield) ShieldRegenCoroutine = StartCoroutine(ShieldRegen()); 
    }
}
