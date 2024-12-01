using Audio;
using Managers;
using UnityEngine;

namespace Powerups
{
    /// <summary>
    /// The InstakillPowerup class is a script that is used to detect when the player picks up a Instakill powerup
    /// </summary>
    public class InstakillPowerup : PowerupPickup
    {
        public static event InstakillPowerupEventHandler InstakillPowerupObtained;

        /// <summary>
        /// Invokes an event when colliding with the player.
        /// </summary>
        /// <param name="other">The object collided with - Should be the player</param>
        public override void OnTriggerEnter(Collider other)
        {
            if(other.gameObject.layer == 7)
            {
                AudioManager.Instance.PlaySound(Random.Range(12, 16));
                InstakillPowerupObtained?.Invoke();
                AttributeManager.Instance.attackPower = 500;
            
                Debug.Log("Instakill Powerup picked up!");
                Destroy(gameObject);
            }
        }
    }
}