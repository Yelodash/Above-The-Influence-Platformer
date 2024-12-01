/*
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Audio;
using UnityEngine.Serialization;

public class BackupHealth : MonoBehaviour, IHealth
{
    [SerializeField] private int health = 100;
    
    
    public  void TakeDamage(int damageAmount)
    {
        if (gameObject.CompareTag("Player"))
        {
            // Specific logic for player damage, if needed
        }

        AudioManager.Instance.PlaySoundAtPoint(Random.Range(33, 34), gameObject.transform.position);
        health -= damageAmount;

        if (health <= 0)
        {
            if (gameObject.CompareTag("Player"))
            {
                // Player-specific logic for handling death
                RespawnPlayer();
            }
            else
            {
                // Destroy non-player objects when health reaches 0
                Destroy(gameObject);
            }
        }
    }
    
    public void HealUnit(int healAmount)
    {
        health += healAmount;
    }
    
    private void RespawnPlayer()
    {
        if (!gameObject.CompareTag("Player"))
            return; // Only allow GameObjects with the "Player" tag to respawn

        Vector3 respawnPosition = CheckpointManager.Instance.GetLastCheckpointPosition();
        transform.position = respawnPosition;

        int initialMaxHealth = CheckpointManager.Instance.GetInitialMaxHealth();
        health = initialMaxHealth;
        
    }
}
*/
