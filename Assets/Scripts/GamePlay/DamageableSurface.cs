using System;
using System.Collections;
using System.Collections.Generic;
using Managers;
using UnityEngine;

/// <summary>
/// Manages the damageable surface in the game.
/// </summary>
public class DamagableSurface : MonoBehaviour
{
    /// <summary>
    ///The amount of damage the player takes.
    /// </summary>
    public int damageToPlayer = 10;

    void Start()
    {
        // No need to find the health bar instance since it's a singleton
    }

    /// <summary>
    ///Called when the Collider other enters the trigger.
    /// </summary>
    /// <param name="other"></param>
    public void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag("Player")) return;

        GameManager.Instance.playerHealth.TakeDamage(damageToPlayer);
        // FindObjectOfType<UnitHealth>().Heal(damageToPlayer);

        // Update health bar using the singleton instance
        HealthBarScript.Instance.SetHealth(GameManager.Instance.playerHealth.Health);

        Debug.Log("Player Take Damage & UI change");
    }
}