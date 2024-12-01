using System;
using System.Collections;
using System.Collections.Generic;
using Audio;
using Powerups;
using UnityEngine;
using Random = UnityEngine.Random;

namespace HealthSystem
{
    public class UnitHealth : Character, IHealth
    {
        //private int health;

        public UnitHealth(int health, int maxHealth) : base(health, maxHealth)
        {
            // Health = health;
            // MaxHealth = maxHealth;
        }

        private void Start()
        {
            if (AudioManager.Instance != null) ;
        }

        private void RespawnPlayer()
        {
            if (!gameObject.CompareTag("Player"))
            {
                return;
            }

            Vector3 respawnPosition = CheckpointManager.Instance.GetLastCheckpointPosition();
            transform.position = respawnPosition;

            // Update both currentHealth and Health to 100
            AttributeManager.Instance.currentHealth = 100;
            Health = 100;
    
            // Update health bar UI
            HealthBarScript.Instance.SetMaxHealth(100);
            HealthBarScript.Instance.SetHealth(100);

            Debug.Log($"Player respawned. Health: {Health}, MaxHealth: {100}"); 
        }

        public new void TakeDamage(int damageAmount)
        {
            if (gameObject.CompareTag("Player"))
            {
                // Specific logic for player damage, if needed
                AttributeManager.Instance.currentHealth -= damageAmount;
                Health -= damageAmount;
            }
            if (gameObject.CompareTag("Enemy"))
            {
                Health -= damageAmount;
                PlayEnemyHitAudio();
            }
            Health = Mathf.Max(Health, 0);
                
            // Health -= damageAmount;

            if (Health <= 0)
            {
                if (gameObject.CompareTag("Player"))
                {
                    // Player-specific logic for handling death
                    RespawnPlayer();
                }
                if (gameObject.CompareTag("Enemy"))
                {
                    // Destroy non-player objects when health reaches 0
                    Destroy(gameObject);
                }
            }
        }

        private void PlayEnemyHitAudio()
        {
            if (AudioManager.Instance == null)
            {
                return;
            }
            
            AudioManager.Instance.PlayOneShot(Random.Range(39, 49), gameObject.transform.position);
        }
        
        public new void HealUnit(int healAmount)
        {
            Health += healAmount;
            Health = Mathf.Min(Health, MaxHealth);
        }
    }
}
