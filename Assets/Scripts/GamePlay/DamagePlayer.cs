using System.Collections;
using System.Collections.Generic;
using HealthSystem;
using Managers;
using Powerups;
using UnityEngine;

/// <summary>
///Manages the damageable surface in the game.
/// </summary>
public class DamagePlayer : MonoBehaviour
{
    /// <summary>
    ///The amount of damage the player takes.
    /// </summary>
    public int damageToPlayer; 

    /// <summary>
    ///Called before the first frame update
    /// </summary>
    private void Start()
    {
       
        var playerDef = AttributeManager.Instance.defence * AttributeManager.Instance.defenceMultiplier;
        damageToPlayer -= (int)playerDef;
    }
    
    /// <summary>
    /// Called when the Collider other enters the trigger.
    /// </summary>
    /// <param name="other"></param>
    public void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag("Player")) return;
        
        var unitHealth = other.GetComponent<UnitHealth>();
        
        if (unitHealth != null)
        {
            unitHealth.TakeDamage(damageToPlayer);
        }
            
        // Update health bar using the singleton instance
        HealthBarScript.Instance.SetHealth(GameManager.Instance.playerHealth.Health);

        Debug.Log("Player Take Damage & UI change");
    }
}