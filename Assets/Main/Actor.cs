using System;
using System.Collections.Generic;
using Unity.Properties;
using Unity.VisualScripting;
using UnityEngine;

public class Actor: MonoBehaviour
{
    private int _health;

    public int Health
    {
        get => _health;
        private set => _health = Mathf.Max(value, 0);
    }

    protected int MaxHealth = 100;

    public event Action<int> OnHealthChanged;

    private readonly List<MonoBehaviour> inventory = new List<MonoBehaviour>();
    protected virtual void Awake() 
    {
        Health = MaxHealth;
    }

    public int GetMaxHealth()
    {
        return MaxHealth;
    }

    public int GetHealth()
    {
        return Health;
    }

    public void AddInventory(MonoBehaviour item)
    {
        if (item == null)
            return;

        inventory.Add(item);
    }

    public bool CheckInventory<T>()
    {
        foreach (MonoBehaviour item in inventory)
        {
            if (item is T)
                return true;
        }

        return false;
    }

    public T GetInventory<T>()
    {
        foreach (MonoBehaviour item in inventory)
        {
            if (item is T typedItem)
                return typedItem;
        }

        return default(T);
    }
    public bool GiveBody(int num, int max = 0)
    {
        num = Mathf.Clamp(num, -65535, 65535);

        if (max == 0)
            max = MaxHealth;

        if (num > 0)
        {
            if (Health >= max)
                return false;

            Health += num;
            if (num < 1) num = 1;

            if (Health > max)
            {
                Health = max;
            }

            OnHealthChanged?.Invoke(Health);
            return true;
        }

        else if (num < 0)
        {
            num *= max * -num / 100;
            if (Health < num)
            {
                Health = num;
                OnHealthChanged?.Invoke(Health);
                return true;
            }
        }
        
        return false;
    }

    public virtual int DamageActor(int damage)
    {
        Health -= damage;
        if (Health < 0)
        {
            Health = 0;
        }

        OnHealthChanged?.Invoke(Health);
        return damage;
    }

    public MainInventory FindInventory()
    {
        return GetComponent<MainInventory>();
    }

}
