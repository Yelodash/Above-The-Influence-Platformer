using Audio;
using Managers;
using UnityEngine;

namespace Powerups
{
    /// <summary>
    /// The HealthPowerup class is a script that is used to detect when the player picks up a health powerup
    /// </summary>
    public class HealthPowerup : PowerupPickup
    {
        public static event HealthPowerupEventHandler HealthPowerupObtained;

        /// <summary>
        /// Invokes an event when colliding with the player.
        /// </summary>
        /// <param name="other">The object collided with - Should be the player</param>
        public override void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer == 7)
            {
                AudioManager.Instance.PlaySound(Random.Range(12, 16));
                HealthPowerupObtained?.Invoke();
                AttributeManager.Instance.maxHealth *= PowerupManager.Instance.healthPowerupMultiplier;
                AttributeManager.Instance.currentHealth = AttributeManager.Instance.maxHealth;
            
                Debug.Log("Instakill Powerup picked up!");
                Destroy(gameObject);
            }
        }
    }
}
