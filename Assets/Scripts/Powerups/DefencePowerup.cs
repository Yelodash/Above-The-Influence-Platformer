using Audio;
using Managers;
using UnityEngine;

namespace Powerups
{
    /// <summary>
    /// The DefencePowerup class is a script that is used to detect when the player picks up a defence powerup.
    /// </summary>
    public class DefencePowerup : PowerupPickup
    {
        public static event DefencePowerupEventHandler DefencePowerupObtained;
    
        /// <summary>
        /// Invokes an event when colliding with the player.
        /// </summary>
        /// <param name="other">The object collided with - Should be the player</param>
        public override void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer == 7)
            {
                AudioManager.Instance.PlaySound(Random.Range(12, 16));

                DefencePowerupObtained?.Invoke();
                // If value is changed here it must be changed in the PowerupManager!!!
                AttributeManager.Instance.defenceMultiplier *= PowerupManager.Instance.defencePowerupMultiplier;
            
                Debug.Log("Defence Powerup picked up!");
                Destroy(gameObject);
            }
        }
    }
}