using System;
using UnityEngine;
using UnityEngine.Events;

public class lifeManager : MonoBehaviour, IDamageable
{
    public float startingHealth;
    public float health { get; protected set; }
    public bool isDead { get; protected set; }
    public UnityEvent OnDead;

    public virtual void OnDamage(float damage, Vector3 hitPoint, Vector3 hitNormal)
    {
        Debug.Log("Damage received: " + damage);
        health -= damage;
        
        if (health <= 0 && !isDead)
        {
            health = 0;
            Die();
        }
    }

    protected virtual void OnEnable()
    {
        isDead = false;
        health = startingHealth;
    }

    public virtual void OnHeal(float newHealth)
    {
        if (isDead)
        {
            return;
        }
        health += newHealth;
        if (health > startingHealth)
        {
            health = startingHealth;
        }
    }

    public virtual void Die()
    {
        isDead = true;
        OnDead?.Invoke();
        //if (OnDead != null)
        //{
        //    OnDead();
        //}
    }


}

