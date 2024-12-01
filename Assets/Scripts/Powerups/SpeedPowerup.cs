using Audio;
using Managers;
using UnityEngine;

namespace Powerups
{
    /// <summary>
    /// The SpeedPowerup class is a script that is used to detect when the player picks up a speed powerup
    /// </summary>
    public class SpeedPowerup : PowerupPickup
    {
        public static event SpeedPowerupEventHandler SpeedPowerupObtained;

        /// <summary>
        /// Invokes an event when colliding with the player.
        /// </summary>
        /// <param name="other">The object collided with - Should be the player</param>
        public override void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer == 7)
            {
                AudioManager.Instance.PlaySound(Random.Range(12, 16));
                SpeedPowerupObtained?.Invoke();
                
                // Multiply all speed values by the speed powerup multiplier
                AttributeManager.Instance.walkSpeed *= PowerupManager.Instance.speedPowerupMultiplier;
                AttributeManager.Instance.sprintSpeed *= PowerupManager.Instance.speedPowerupMultiplier;
                AttributeManager.Instance.crouchWalkSpeed *= PowerupManager.Instance.speedPowerupMultiplier;

                Debug.Log("Speed Powerup picked up!");
                Destroy(gameObject);
            }
        }
    }
}