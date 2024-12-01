using Audio;
using Managers;
using UnityEngine;

namespace Powerups
{
    /// <summary>
    /// The PistolPowerup class is a script that is used to detect when the player picks up a pistol powerup
    /// </summary>
    public class PistolPowerup : PowerupPickup
    {
        public static event PistolPowerupEventHandler PistolPowerupObtained;
        public static event FixupAmmo FixupAmmo;

        /// <summary>
        /// Invokes an event when colliding with the player.
        /// </summary>
        /// <param name="other">The object collided with - Should be the player</param>
        public override void OnTriggerEnter(Collider other)
        {
            if(other.gameObject.layer == 7)
            {
                AudioManager.Instance.PlaySound(Random.Range(12, 16));
                FixupAmmo?.Invoke();
                PistolPowerupObtained?.Invoke();

                Debug.Log("Gun Powerup picked up!");
                Destroy(gameObject);
            }
        }
    }
}