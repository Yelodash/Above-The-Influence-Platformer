using System;
using Powerups;
using UnityEngine;

public class Character : MonoBehaviour
{
    
    public int Health;
    public int MaxHealth;

    // Constructeur
    public Character(int health, int maxHealth)
    {
        Health = health;
        MaxHealth = maxHealth;
    }

    private void Update()
    {
        if (!gameObject.CompareTag("Player"))
        {
            return;
        }
        
        MaxHealth = Mathf.RoundToInt(AttributeManager.Instance.maxHealth);
        Health = Mathf.RoundToInt(AttributeManager.Instance.currentHealth);

        if (Health > MaxHealth) 
        { 
            AttributeManager.Instance.currentHealth = MaxHealth; 
            Health = MaxHealth;
        }
    }

    public void TakeDamage(int damageAmount)
    {
        
        Health -= damageAmount;

        
        Health = Mathf.Max(Health, 0);
    }

    
    public void Heal(int healAmount)
    {
        
        Health += healAmount;

        
        Health = Mathf.Min(Health, MaxHealth);
    }
}
