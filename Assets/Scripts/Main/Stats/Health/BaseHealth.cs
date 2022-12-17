using System;
using UnityEngine;

public abstract class BaseHealth
{
    public int MaxHealth { get; private set; }
    public int CurrentHealth { get; private set; }

    public event Action<int> OnHealthChangedEvent;
    public event Action OnDeathEvent;
    
    public BaseHealth(int maxHealth)
    {
        MaxHealth = maxHealth;
        CurrentHealth = maxHealth;
    }

    public virtual void TakeDamage(int damageValue)
    {
        CurrentHealth -= damageValue;
        CurrentHealth = Mathf.Max(CurrentHealth, 0);
        OnHealthChangedEvent?.Invoke(CurrentHealth);

        if (IsDead())
        {
            OnDeathEvent?.Invoke();
        }
    }

    public virtual void RestoreHealth(int healValue)
    {
        CurrentHealth += healValue;
        CurrentHealth = Mathf.Min(CurrentHealth, MaxHealth);
        OnHealthChangedEvent?.Invoke(CurrentHealth);
    }

    public virtual bool IsDead()
    {
        return CurrentHealth <= 0;
    }
}
